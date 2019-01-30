using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Base.Core.Configurations;
using Nozomi.Repo.Data;
using Nozomi.Repo.Identity.Data;

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
        }
    }
}