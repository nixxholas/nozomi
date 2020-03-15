using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Nozomi.Api.Extensions;
using Nozomi.Infra.Api.Limiter.Events;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Infra.Api.Limiter.Services;
using Nozomi.Infra.Api.Limiter.Services.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.Data;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Interfaces;
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

                services.AddHsts(opt =>
                {
                    opt.Preload = true;
                    opt.IncludeSubDomains = true;
                    opt.MaxAge = TimeSpan.FromDays(60);
                });

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

            services.AddResponseCompression();
            services.AddControllers();

            // https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1&tabs=visual-studio#options
            // Calling AddHttpsRedirection is only necessary to change the values of HttpsPort or RedirectStatusCode.
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            });

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(GlobalApiVariables.CURRENT_API_VERSION, new OpenApiInfo {
                    Title = "Nozomi API", 
                    Version = GlobalApiVariables.CURRENT_API_REVISION.ToString()
                });
                
                // Define the Api Key scheme that's in use (i.e. Implicit Flow)
                config.AddSecurityDefinition("apikey", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("/auth-server/connect/authorize", UriKind.Relative),
                            Scopes = new Dictionary<string, string>
                            {
                                { "readAccess", "Access read operations" },
                                { "writeAccess", "Access write operations" }
                            }
                        }
                    }
                });
            });

            services.AddTransient<INozomiRedisEvent, NozomiRedisEvent>();
            services.AddTransient<IRequestEvent, RequestEvent>();

            services.AddTransient<IApiKeyEventsService, ApiKeyEventsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();

                app.UseResponseCompression();
            }

            app.UseHttpsRedirection();

            // ref: https://github.com/aspnet/Docs/issues/2384
            app.UseForwardedHeaders();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/{documentName}/swagger.json";
            });
            
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Nozomi API Documentation";
                c.RoutePrefix = "";
                c.SwaggerEndpoint($"/{GlobalApiVariables.CURRENT_API_VERSION}/swagger.json", 
                    $"Nozomi API rev. {GlobalApiVariables.CURRENT_API_REVISION}");
            });
        }
    }
}