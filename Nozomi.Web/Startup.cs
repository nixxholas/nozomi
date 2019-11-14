using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.AspNetCore.SpaServices.Extensions;
// using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Filters;
using Nozomi.Repo.Data;
using Nozomi.Web.Extensions;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Nozomi.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public static IConfiguration Configuration { get; set; }

        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddOptions();

            services.AddResponseCompression();

            services.AddDbContextInjections();

            services.AddMemoryCache();

            services.AddHealthChecks();

            // Add framework services.
            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            if (HostingEnvironment.IsProduction())
            {
                services.AddHsts(opt =>
                {
                    opt.Preload = true;
                    opt.IncludeSubDomains = true;
                    opt.MaxAge = TimeSpan.FromDays(60);
                });
            }

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                // options.HttpsPort = 5001;
            });

            // In production, the Vue files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // UoW-Repository injection
            services.ConfigureRepoLayer();

            // Service layer injections
            services.ConfigureInfra();

            // Swashbuckle Swagger
            services.ConfigureSwagger();

            // Auth
            services.ConfigureNozomiAuth(HostingEnvironment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHealthChecks("/health");
            #if DEBUG
            app.UseDeveloperExceptionPage();

            // Webpack initialization with hot-reload.
//            app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
//            {
//                HotModuleReplacement = true,
//            });

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
                app.UseResponseCompression();
            }

            app.UseHttpsRedirection();

            // https://github.com/IdentityServer/IdentityServer4/issues/1331
            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                RequireHeaderSymmetry = false
            };

            forwardOptions.KnownNetworks.Clear();
            forwardOptions.KnownProxies.Clear();

            // ref: https://github.com/aspnet/Docs/issues/2384
            app.UseForwardedHeaders(forwardOptions);

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();

            app.UseAuthorization();

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                /*
                // If you want to enable server-side rendering (SSR),
                // [1] In AspNetCoreSpa.csproj, change the <BuildServerSideRenderer> property
                //     value to 'true', so that the SSR bundle is built during publish
                // [2] Uncomment this code block
                */

                //   spa.UseSpaPrerendering(options =>
                //    {
                //        options.BootModulePath = $"{spa.Options.SourcePath}/dist-server/main.bundle.js";
                //        options.BootModuleBuilder = env.IsDevelopment() ? new AngularCliBuilder(npmScript: "build:ssr") : null;
                //        options.ExcludeUrls = new[] { "/sockjs-node" };
                //        options.SupplyData = (requestContext, obj) =>
                //        {
                //          //  var result = appService.GetApplicationData(requestContext).GetAwaiter().GetResult();
                //          obj.Add("Cookies", requestContext.Request.Cookies);
                //        };
                //    });

                if (HostingEnvironment.IsDevelopment())
                {
                    //   spa.UseAngularCliServer(npmScript: "start");
                    //   OR
                    //   spa.UseProxyToSpaDevelopmentServer("http://localhost:5001");

                    spa.UseVueDevelopmentServer();
                }
            });

//            app.UseMvc(routes =>
//            {
//                routes.MapRoute(
//                    name: "default",
//                    template: "{controller=Home}/{action=Index}/{id?}");
//
//                routes.MapRoute(
//                    name: "Areas",
//                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
//
//                routes.MapSpaFallbackRoute(
//                    name: "spa-fallback",
//                    defaults: new { controller = "Home", action = "Index" });
//            });
        }
    }
}
