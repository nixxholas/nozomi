using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Nozomi.Api.Extensions;
using Nozomi.Api.Filters;
using Nozomi.Infra.Api.Limiter.Events;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Infra.Api.Limiter.Handlers;
using Nozomi.Infra.Api.Limiter.Services;
using Nozomi.Infra.Api.Limiter.Services.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Options;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.Data;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Analysis;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.ReDoc;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Nozomi.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();

                // https://github.com/IdentityServer/IdentityServer4/issues/1331
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.RequireHeaderSymmetry = false;
                options.ForwardLimit = 2;
            });
            
            if (WebHostEnvironment.IsStaging() || WebHostEnvironment.IsProduction())
            {
                var vaultUrl = Configuration["vaultUrl"];
                var vaultToken = Configuration["vaultToken"];

                if (string.IsNullOrEmpty(vaultToken))
                    throw new SystemException("Invalid vault token.");

                // HSTS restrictions
                services.AddHsts(opt =>
                {
                    opt.Preload = true;
                    opt.IncludeSubDomains = true;
                    opt.MaxAge = TimeSpan.FromDays(60);
                });
                
                // Response Caching
                // Cloudflare - max-age of 1 week
                services.AddResponseCaching();

                var authMethod = new TokenAuthMethodInfo(vaultToken);
                var vaultClientSettings = new VaultClientSettings(
                    !string.IsNullOrWhiteSpace(vaultUrl) ? vaultUrl : "https://blackbox.nozomi.one:8200",
                    authMethod);
                var vaultClient = new VaultClient(vaultClientSettings);
                
                var vault = vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("api")
                    .GetAwaiter()
                    .GetResult().Data;
                
                services.ConfigureRedis((string) vault["redis"]);

                services.AddDbContextPool<NozomiDbContext>(options =>
                {
                    options.UseNpgsql((string) vault["main"], 
                        nozomiDbContextBuilder => { nozomiDbContextBuilder.EnableRetryOnFailure(); }
                    );
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
            }
            else
            {
                Console.WriteLine(@"Welcome to the dev environment, your machine is named: " + Environment.MachineName);
                
                var redisStr = Configuration.GetConnectionString("LocalRedis:" + Environment.MachineName);
                
                services.ConfigureRedis(redisStr);

                services.AddDbContextPool<NozomiDbContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("Local:" + Environment.MachineName), 
                        nozomiDbContextBuilder => { nozomiDbContextBuilder.EnableRetryOnFailure(); }
                    );
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
            }

            services.AddMemoryCache(); // Required for API Throttling

            services.AddResponseCompression();
            services.AddControllers();

            // https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1&tabs=visual-studio#options
            // Calling AddHttpsRedirection is only necessary to change the values of HttpsPort or RedirectStatusCode.
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
            })
                .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                    ApiKeyAuthenticationOptions.DefaultScheme, o => { });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    ApiKeyAuthenticationOptions.DefaultScheme);

                defaultAuthorizationPolicyBuilder = 
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(GlobalApiVariables.CURRENT_API_VERSION, new OpenApiInfo {
                    Title = "Nozomi API", 
                    Version = GlobalApiVariables.CURRENT_API_REVISION.ToString(),
                    Extensions = new Dictionary<string, IOpenApiExtension>
                    {
                        // Get the custom logo ready bitches
                        // https://stackoverflow.com/questions/54335549/adding-x-logo-vendor-extension-using-swashbuckle-asp-net-core-for-redoc
                        { "x-logo", new OpenApiObject
                            {
                                // TODO: Fix full routing
                                { "url", new OpenApiString("api-images/logo.svg") },
                                { "backgroundColor" , new OpenApiString("#FFFFFF") },
                                { "altText", new OpenApiString("Nozomi") }
                            }
                        }
                    }
                });

                config.EnableAnnotations(); // [SwaggerTag] support

                // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization
                config.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                // or use the generic method, e.g. c.OperationFilter<AppendAuthorizeToSummaryOperationFilter<MyCustomAttribute>>();
                
                // [SwaggerRequestExample] & [SwaggerResponseExample]
                // version < 3.0 like this: c.OperationFilter<ExamplesOperationFilter>(); 
                // version 3.0 like this: c.AddSwaggerExamples(services.BuildServiceProvider());
                // version > 4.0 like this:
                config.ExampleFilters();
                config.OperationFilter<AddResponseHeadersFilter>(); // [SwaggerResponseHeader]
                
                // Locate the XML file being generated by .NET Core
                var filePath = Path.Combine(AppContext.BaseDirectory, 
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                config.IncludeXmlComments(filePath);
                
                // Define the Api Key scheme that's in use (i.e. Implicit Flow)
                config.AddSecurityDefinition(ApiKeyAuthenticationOptions.DefaultScheme, new OpenApiSecurityScheme
                {
                    Description = "Nozomi's custom authorization header using the Api Key scheme. Example: \"{token}\"",
                    In = ParameterLocation.Header,
                    Name = ApiKeyAuthenticationOptions.HeaderKey,
                    Type = SecuritySchemeType.ApiKey,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("/connect/validate", UriKind.Relative),
                            Scopes = new Dictionary<string, string>
                            {
                                { "readAccess", "Access read operations" },
                                { "writeAccess", "Access write operations" }
                            }
                        }
                    }
                });

                // add Security information to each operation for OAuth2
                config.OperationFilter<SecurityRequirementsOperationFilter>();

                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme, 
                                Id = ApiKeyAuthenticationOptions.DefaultScheme
                            }
                        },
                        new[] { "readAccess", "writeAccess" }
                    }
                });
            });
            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

            services.AddTransient<IAnalysedComponentEvent, AnalysedComponentEvent>();
            services.AddTransient<IAnalysedHistoricItemEvent, AnalysedHistoricItemEvent>();
            services.AddTransient<IComponentEvent, ComponentEvent>();
            services.AddTransient<IComponentHistoricItemEvent, ComponentHistoricItemEvent>();
            services.AddTransient<IComponentTypeEvent, ComponentTypeEvent>();
            services.AddTransient<INozomiRedisEvent, NozomiRedisEvent>();
            services.AddTransient<IRequestEvent, RequestEvent>();
            services.AddTransient<IRequestPropertyEvent, RequestPropertyEvent>();
            services.AddTransient<IWebsocketCommandEvent, WebsocketCommandEvent>();
            services.AddTransient<IWebsocketCommandPropertyEvent, WebsocketCommandPropertyEvent>();

            services.AddTransient<IApiKeyEventsService, ApiKeyEventsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Treating this web app like a CDN lol
                // https://www.tutorialsteacher.com/core/aspnet-core-static-file
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(Directory.GetCurrentDirectory(), @"Images")),
                    RequestPath = new PathString("/api-images")
                });
                
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();

                app.UseResponseCompression();

                // Treating this web app like a CDN lol
                // https://www.tutorialsteacher.com/core/aspnet-core-static-file
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider("/app/Images"),
                    RequestPath = new PathString("/api-images")
                });
            }

            app.UseHttpsRedirection();

            // ref: https://github.com/aspnet/Docs/issues/2384
            app.UseForwardedHeaders();

            app.UseRouting();
            
            // Response caching to match certain uses
            // Cloudflare - max-age of 1 week
            app.UseResponseCaching();
            
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl = 
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(7) // Cloudflare's Certificate Transparency requirements.
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] = 
                    new [] { "Accept-Encoding" };

                await next();
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/{documentName}/swagger.json";
                c.SerializeAsV2 = true;
            });
            
            // app.UseSwaggerUI(c =>
            // {
            //     c.DocumentTitle = "Nozomi API Documentation";
            //     c.RoutePrefix = "";
            //     c.SwaggerEndpoint($"/{GlobalApiVariables.CURRENT_API_VERSION}/swagger.json", 
            //         $"Nozomi API rev. {GlobalApiVariables.CURRENT_API_REVISION}");
            //     c.OAuthClientSecret(ApiKeyAuthenticationOptions.HeaderKey);
            // });

            app.UseReDoc(c => { 
                c.DocumentTitle = "Nozomi API";
                c.RoutePrefix = "";
                c.SpecUrl($"/{GlobalApiVariables.CURRENT_API_VERSION}/swagger.json");
                c.ConfigObject = new ConfigObject
                {
                    HideDownloadButton = true,
                    HideLoading = true
                };
                c.NativeScrollbars();
                // c.OAuthClientSecret(ApiKeyAuthenticationOptions.HeaderKey);
            });
        }
    }
}