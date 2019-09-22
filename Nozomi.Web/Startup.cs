using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Preprocessing;
using Nozomi.Repo.Data;
using Nozomi.Repo.Identity.Data;
using Nozomi.Web.StartupExtensions;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Nozomi.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
             // Environment Inclusion
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var webEnv = Environment.GetEnvironmentVariable("WEB_ENVIRONMENT") ?? string.Empty;

            if (!string.IsNullOrEmpty(env) && (!env.Equals("production", StringComparison.OrdinalIgnoreCase)
                && !webEnv.Equals("Production", StringComparison.OrdinalIgnoreCase)))
            {
                // Greet the beloved dev
                Console.WriteLine(@"Welcome to the dev environment, your machine is named: " + Environment.MachineName);

                // Postgres DB Setup
                var str = Configuration.GetConnectionString("Local:" + @Environment.MachineName);

                services
                    .AddEntityFrameworkNpgsql()
                    .AddDbContext<NozomiDbContext>(options =>
                {
                    options.UseNpgsql(str);
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                },
                    ServiceLifetime.Transient);

                services
                    .AddEntityFrameworkNpgsql()
                    .AddDbContext<NozomiAuthContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("LocalAuth:" + Environment.MachineName));
                    options.EnableSensitiveDataLogging(false);
                });
            }
            else
            {
                var vaultToken = Configuration["vaultToken"];

                if (string.IsNullOrEmpty(vaultToken))
                    throw new SystemException("Invalid vault token.");

                var authMethod = new TokenAuthMethodInfo(vaultToken);
                var vaultClientSettings = new VaultClientSettings("http://165.22.250.169:8200", authMethod);
                var vaultClient = new VaultClient(vaultClientSettings);

                var nozomiVault = vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("nozomi")
                    .GetAwaiter()
                    .GetResult().Data;

                var mainDb = (string) nozomiVault["main"];
                if (string.IsNullOrEmpty(mainDb))
                    throw new SystemException("Invalid main database configuration");
                // Database
                services.AddDbContext<NozomiDbContext>(options =>
                {
                    options.UseNpgsql(mainDb
                        , builder =>
                        {
                            builder.EnableRetryOnFailure();
//                            builder.ProvideClientCertificatesCallback(certificates =>
//                            {
//                                var cert = new X509Certificate2("ca-certificate.crt");
//                                certificates.Add(cert);
//                            });
                        }
                    );
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }, ServiceLifetime.Transient);

                var authDb = (string) nozomiVault["auth"];
                if (string.IsNullOrEmpty(authDb))
                    throw new SystemException("Invalid auth database configuration");
                services.AddDbContext<NozomiAuthContext>(options =>
                {
                    options.UseNpgsql(authDb
                        , builder =>
                        {
                            builder.EnableRetryOnFailure();
//                            builder.ProvideClientCertificatesCallback(certificates =>
//                            {
//                                var cert = new X509Certificate2("ca-certificate.crt");
//                                certificates.Add(cert);
//                            });
                        }
                    );
                    options.EnableSensitiveDataLogging(false);
                });
            }

            // Add framework services.
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Audience = "auth.web.api";
                    options.Authority = "https://localhost:44364";
                });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy =
                    new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build();
            });

            // UoW-Repository injection
            services.ConfigureRepoLayer();

            // Service layer injections
            services.ConfigureEvents();

            // Swashbuckle Swagger
            services.ConfigureSwagger();

            // Auth
            services.ConfigureNozomiAuth();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            #if DEBUG
            app.UseDeveloperExceptionPage();

            // Webpack initialization with hot-reload.
            app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
            {
                HotModuleReplacement = true,
            });

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    {
                        Console.WriteLine($"Something went wrong: {contextFeature.Error}");

                        await context.Response.WriteAsync(new
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        }.ToString());
                    }
                });
            });
            #endif

            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                // https://stackoverflow.com/questions/39116047/how-to-change-base-url-of-swagger-in-asp-net-core
                c.RoutePrefix = "docs";
                c.SwaggerEndpoint("/swagger/" + GlobalApiVariables.CURRENT_API_VERSION + "/swagger.json", "Nozomi API");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "Areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
