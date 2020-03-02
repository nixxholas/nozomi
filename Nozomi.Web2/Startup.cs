using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Filters;
using Nozomi.Service;
using Nozomi.Web2.Extensions;
using Swashbuckle.AspNetCore.SwaggerUI;
using VaultSharp;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.Token;
using VueCliMiddleware;

namespace Nozomi.Web2
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public static IConfiguration Configuration { get; set; }

        public static IWebHostEnvironment Environment { get; set; }

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

            services.AddOptions();

            services.AddResponseCompression();

            Dictionary<string, object> nozomiVault = null;

            // HASHICORP VAULT
            if (Environment.IsProduction() || Environment.IsStaging())
            {
                var vaultUrl = Startup.Configuration["vaultUrl"];
                var vaultToken = Startup.Configuration["vaultToken"];

                if (string.IsNullOrEmpty(vaultToken))
                    throw new SystemException("Invalid vault token.");

                var authMethod = new TokenAuthMethodInfo(vaultToken);
                var vaultClientSettings = new VaultClientSettings(
                    !string.IsNullOrWhiteSpace(vaultUrl) ? vaultUrl : "https://blackbox.nozomi.one:8200",
                    authMethod);
                var vaultClient = new VaultClient(vaultClientSettings);

                nozomiVault = vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("nozomi")
                    .GetAwaiter()
                    .GetResult().Data;
                
                if (nozomiVault.Equals(null))
                    throw new VaultApiException("Unable to obtain nozomi's configurations in the cubbyhole.");
            }

            services.AddDbContextInjections(nozomiVault);

            services.AddMemoryCache(); // Required for API Throttling

            // services.AddHealthChecks();

            // In production, the Vue files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });

            services.AddControllers(options => { options.Filters.Add(typeof(HttpGlobalExceptionFilter)); })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // Add AddRazorPages
            services.AddRazorPages();

            services.AddHttpContextAccessor();

            if (Environment.IsProduction() || Environment.IsStaging())
            {
                services.AddHsts(opt =>
                {
                    opt.Preload = true;
                    opt.IncludeSubDomains = true;
                    opt.MaxAge = TimeSpan.FromDays(60);
                });

                services.Configure<TrelloOptions>(option =>
                {
                    option.ApiKey = (string) nozomiVault["Trello:ApiKey"];
                    option.AuthToken = (string) nozomiVault["Trello:AuthToken"];
                });
            }
            else
            {
                services.Configure<TrelloOptions>(option =>
                {
                    option.ApiKey = Configuration["TrelloToken:ApiKey"];
                    option.AuthToken = Configuration["TrelloToken:AuthToken"];
                });
            }

            // https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1&tabs=visual-studio#options
            // Calling AddHttpsRedirection is only necessary to change the values of HttpsPort or RedirectStatusCode.
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            });

            // UoW-Repository injection
            services.ConfigureRepoLayer();

            // Service layer injections
            services.ConfigureInfra();

            // Swashbuckle Swagger
            services.ConfigureSwagger();

            // Auth
            services.ConfigureNozomiAuth();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                app.UseResponseCompression();
            }

            // Always enforce HTTPS for development and production
            app.UseHttpsRedirection();

            // ref: https://github.com/aspnet/Docs/issues/2384
            app.UseForwardedHeaders();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseCookiePolicy();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Nozomi API";
                // https://stackoverflow.com/questions/39116047/how-to-change-base-url-of-swagger-in-asp-net-core
                c.RoutePrefix = "docs";
                c.SwaggerEndpoint("/swagger/" +
                                  GlobalApiVariables.CURRENT_API_VERSION + "/swagger.json", "Nozomi API");
                c.IndexStream = () => GetType().Assembly
                    .GetManifestResourceStream(
                        "Nozomi.Web2.Resources.Index.html"); // requires file to be added as an embedded resource

                c.DocExpansion(DocExpansion.None);
                c.EnableDeepLinking();
                c.EnableFilter();
            });

            app.UseRouting();

            // For most apps, calls to UseAuthentication, UseAuthorization, and UseCors must
            // appear between the calls to UseRouting and UseEndpoints to be effective.
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // MapControllers adds support for attribute-routed controllers.
                // MapAreaControllerRoute adds a conventional route for controllers
                // in an area.
                // MapControllerRoute adds a conventional route for controllers.
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                // endpoints.MapControllers();

                // Health check up!!!
                // https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.0#basic-health-probe
                // endpoints.MapHealthChecks("/health");

                if (env.IsDevelopment())
                    endpoints.MapToVueCliProxy(
                        "{*path}",
                        new SpaOptions {SourcePath = "ClientApp"},
                        System.Diagnostics.Debugger.IsAttached ? "serve" : null,
                        regex: "Compiled successfully",
                        forceKill: true
                    );
                // else
                //     endpoints.MapFallbackToFile("index.html");

                // Add MapRazorPages if the app uses Razor Pages. Since Endpoint Routing includes support for many frameworks, adding Razor Pages is now opt -in.
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa => { spa.Options.SourcePath = "ClientApp"; });

            app.UseAutoDbMigration(Environment);
        }
    }
}