using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CounterCore.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nozomi.Repo.Data;
using Nozomi.Repo.Repositories;
using Nozomi.Service.HostedServices;
using Nozomi.Service.Hubs;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;
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
                Console.WriteLine(@"Welcome to dev, your machine is named: " + Environment.MachineName);
                var str = Configuration.GetConnectionString("Local:" + @Environment.MachineName);

                services.AddDbContext<NozomiDbContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("Local:" + @Environment.MachineName));
                });
            }
            else
            {
                // Database
                services.AddDbContext<NozomiDbContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("NozomiDb"));
                });
            }
            
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            // Scopes
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Service Injections
            services.AddTransient<ICurrencyService, CurrencyService>();
            services.AddTransient<ICurrencyPairService, CurrencyPairService>();
            services.AddTransient<ICurrencyPairComponentService, CurrencyPairComponentService>();
            services.AddTransient<ICurrencyPairRequestService, CurrencyPairRequestService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<ISourceService, SourceService>();
            
            
            
            services.AddSignalR()
                .AddMessagePackProtocol();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSignalR(route =>
            {
                route.MapHub<TickerHub>("/ticker");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}