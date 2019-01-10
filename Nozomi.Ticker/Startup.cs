using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nozomi.Base.Core.Configurations;
using Nozomi.Base.Core.Helpers.Routing;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Data.WebModels.LoggingModels;
using Nozomi.Repo.Data;
using Nozomi.Repo.Identity.Data;
using Nozomi.Service.HostedServices;
using Nozomi.Service.HostedServices.RequestTypes;
using Nozomi.Service.HostedServices.RequestTypes.Interfaces;
using Nozomi.Service.HostedServices.StaticUpdater;
using Nozomi.Service.Hubs;
using Nozomi.Service.Identity.Stores;
using Nozomi.Service.Middleware;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;
using Nozomi.Ticker.Areas;
using Nozomi.Ticker.StartupExtensions;
using StackExchange.Redis;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Nozomi.Ticker
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
            
            if (!string.IsNullOrEmpty(env) && !env.Equals("production", StringComparison.OrdinalIgnoreCase))
            {
                // Greet the beloved dev
                Console.WriteLine(@"Welcome to the dev environment, your machine is named: " + Environment.MachineName);
                
                // Postgres DB Setup
                var str = Configuration.GetConnectionString("Local:" + @Environment.MachineName);

                services.AddDbContext<NozomiDbContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("Local:" + @Environment.MachineName));
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                },
                    ServiceLifetime.Transient);

                services.AddDbContext<NozomiAuthContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("LocalAuth:" + Environment.MachineName));
                    options.EnableSensitiveDataLogging(false);
                });
            
                // Redis
                services.AddDistributedRedisCache(option =>
                {
                    option.Configuration = "127.0.0.1:6379";
                    option.InstanceName = "nozomi-cache";
                });
            
                // Stripe
                services.Configure<StripeSettings>(ss =>
                {
                    ss.SecretKey = Configuration.GetConnectionString("Stripe:TestPriv");
                    ss.PublishableKey = Configuration.GetConnectionString("Stripe:TestPub");
                });
            }
            else
            {
                // Database
                services.AddDbContext<NozomiDbContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("NozomiDb"));
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }, ServiceLifetime.Transient);

                services.AddDbContext<NozomiAuthContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("NozomiAuthDb"));
                    options.EnableSensitiveDataLogging(false);
                });
            
                // Redis
                services.AddDistributedRedisCache(option =>
                {
                    option.Configuration = Configuration.GetConnectionString("RedisConfiguration");
                    option.InstanceName = "nozomi-cache";
                });
            
                // Stripe
                services.Configure<StripeSettings>(ss =>
                {
                    ss.SecretKey = Configuration.GetConnectionString("StripePrivate");
                    ss.PublishableKey = Configuration.GetConnectionString("StripePublic");
                });
            }
            
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
                options.Secure = CookieSecurePolicy.Always;
            });
            
            services.AddSignalR()
                .AddMessagePackProtocol();
            
            services.ConfigureCors();

            services.AddSession();
            
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddSessionStateTempDataProvider();
            
            // https://stackoverflow.com/questions/36358751/how-do-you-enforce-lowercase-routing-in-asp-net-core
            services.AddRouting(option =>
            {
                option.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
                option.LowercaseUrls = true;
            });

            // https://stackoverflow.com/questions/38184583/how-to-add-ihttpcontextaccessor-in-the-startup-class-in-the-di-in-asp-net-core-1
            //services.AddHttpContextAccessor();
            
            // Repository Layer
            services.ConfigureRepoLayer();
            
            // Service Layer
            services.ConfigureServiceLayer();

            // Hosted Service Layer
            services.ConfigureHostedServices();

            services.ConfigureNozomiAuth(Configuration);
            
            services.ConfigureSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseNozomiExceptionMiddleware();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseNozomiAuth();
            app.UseCookiePolicy();
            app.UseAutoDbMigration(env);
            
            // Setup the hot collections
            app.ConfigureStatics();

            app.UseSignalR(route =>
            {
                route.MapHub<TickerHub>("/ticker");
            });

            app.UseSession();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=home}/{action=index}/{id?}");
            });

            app.UseMiddleware<NozomiExceptionMiddleware>();
            

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                // https://stackoverflow.com/questions/39116047/how-to-change-base-url-of-swagger-in-asp-net-core
                c.RoutePrefix = "docs";
                c.SwaggerEndpoint("/swagger/" + GlobalApiVariables.CURRENT_API_VERSION + "/swagger.json", "Nozomi API v1");
            });
        }
    }
}