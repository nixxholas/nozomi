﻿using System;
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
using Nozomi.Data.WebModels.LoggingModels;
using Nozomi.Repo.Data;
using Nozomi.Repo.Repositories;
using Nozomi.Service.HostedServices;
using Nozomi.Service.HostedServices.RequestTypes;
using Nozomi.Service.HostedServices.RequestTypes.Interfaces;
using Nozomi.Service.Hubs;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;
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
                Console.WriteLine(@"Welcome to dev, your machine is named: " + Environment.MachineName);
                
                // Postgres DB Setup
                var str = Configuration.GetConnectionString("Local:" + @Environment.MachineName);

                services.AddDbContext<NozomiDbContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("Local:" + @Environment.MachineName));
                });
                
                // Redis Config
                var redisConfig = new ConfigurationOptions
                {
                    EndPoints =
                    {
                        { "localhost", 6379 }    
                    },
                    CommandMap = CommandMap.Create(new HashSet<string>
                    { 
                        // EXCLUDE a few commands
                        "INFO", "CONFIG", "CLUSTER",
                        "PING", "ECHO", "CLIENT"
                    }, available: false),
                    KeepAlive = 180,
                    //DefaultVersion = new Version(2, 8, 8),
                    Password = ""
                };
            
                // Redis Multiplexer
                services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfig));
            }
            else
            {
                // Database
                services.AddDbContext<NozomiDbContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("NozomiDb"));
                }, ServiceLifetime.Transient);
                
                // Redis Config
                var redisConfig = new ConfigurationOptions
                {
                    EndPoints =
                    {
                        { Configuration.GetConnectionString("RedisPath"), 6379 }
                    },
                    CommandMap = CommandMap.Create(new HashSet<string>
                    { 
                        // EXCLUDE a few commands
                        "INFO", "CONFIG", "CLUSTER",
                        "PING", "ECHO", "CLIENT"
                    }, available: false),
                    KeepAlive = 180,
                    //DefaultVersion = new Version(2, 8, 8),
                    Password = Configuration.GetConnectionString("RedisKey") ?? string.Empty
                };
            
                // Redis Multiplexer
                services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfig));
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
            services.AddTransient<IRequestLogService, RequestLogService>();
            services.AddTransient<ISourceService, SourceService>();

            services.AddTransient<IUnitOfWork<NozomiDbContext>, UnitOfWork<NozomiDbContext>>();
            services.AddTransient<IDbContext, NozomiDbContext>();

            // Hosted Services
            services.AddSingleton<IHostedService, HttpGetCurrencyPairRequestSyncingService>();
            
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