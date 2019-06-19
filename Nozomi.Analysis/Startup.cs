using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Analysis.StartupExtensions;
using Nozomi.Repo.Data;
using Nozomi.Repo.Identity.Data;

namespace Nozomi.Analysis
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (HostingEnvironment.IsDevelopment())
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
                        options.EnableSensitiveDataLogging();
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
                // Database
                services.AddDbContext<NozomiDbContext>(options =>
                {
                    options.UseNpgsql(Configuration["NozomiDb"]);
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }, ServiceLifetime.Transient);

                services.AddDbContext<NozomiAuthContext>(options =>
                {
                    options.UseNpgsql(Configuration["NozomiAuthDb"]);
                    options.EnableSensitiveDataLogging(false);
                });
            }
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.ConfigureRepoLayer();
            services.ConfigureEvents();
            services.ConfigureServiceLayer();
            services.ConfigureHostedServices();
            services.ConfigureNozomiAuth();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            
            app.UseAutoDbMigration(env);
        }
    }
}