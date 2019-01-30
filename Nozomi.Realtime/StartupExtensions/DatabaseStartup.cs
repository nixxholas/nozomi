using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Base.Core.Configurations;
using Nozomi.Repo.BCL.Context;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Repo.Identity.Data;
using Nozomi.Service.Identity.Services;
using Nozomi.Service.Identity.Services.Interfaces;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Enumerators;
using Nozomi.Service.Services.Enumerators.Interfaces;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Realtime.StartupExtensions
{
    public static class DatabaseStartup
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration,
            IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Greet the beloved dev
                Console.WriteLine(@"Welcome to the dev environment, your machine is named: " + 
                                  Environment.MachineName);
                
                // Postgres DB Setup
                var str = configuration.GetConnectionString("Local:" + @Environment.MachineName);

                services.AddDbContext<NozomiDbContext>(options =>
                {
                    options.UseNpgsql(str);
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                },
                    ServiceLifetime.Transient);
            
                // Redis
                services.AddDistributedRedisCache(option =>
                {
                    option.Configuration = "127.0.0.1:6379";
                    option.InstanceName = "nozomi-cache";
                });
            }
            else
            {
                // Database
                services.AddDbContext<NozomiDbContext>(options =>
                {
                    options.UseNpgsql(configuration.GetConnectionString("NozomiDb"));
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }, ServiceLifetime.Transient);
                
                // Redis
                services.AddDistributedRedisCache(option =>
                {
                    option.Configuration = configuration.GetConnectionString("RedisConfiguration");
                    option.InstanceName = "nozomi-cache";
                });
            }
            
            // Database
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            
            services.AddTransient<IUnitOfWork<NozomiDbContext>, UnitOfWork<NozomiDbContext>>();
            services.AddTransient<IDbContext, NozomiDbContext>();
            
            // Service Injections
            services.AddTransient<ICurrencyService, CurrencyService>();
            services.AddTransient<ICurrencyPairService, CurrencyPairService>();
            services.AddTransient<ICurrencyPairComponentService, CurrencyPairComponentService>();
            services.AddTransient<ICurrencyPairRequestService, CurrencyPairRequestService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<IRequestLogService, RequestLogService>();
            services.AddTransient<ISourceService, SourceService>();
            services.AddTransient<ITickerService, TickerService>();

            // Singleton service injections for in-memory-related processes.
            services.AddScoped<IComponentTypeService, ComponentTypeService>();
            services.AddScoped<ICurrencyPairTypeService, CurrencyPairTypeService>();
            services.AddScoped<IRequestPropertyTypeService, RequestPropertyTypeService>();
            services.AddScoped<IRequestTypeService, RequestTypeService>();
        }
    }
}