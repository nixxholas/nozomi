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
using Microsoft.Extensions.Hosting;
using Nozomi.Base.Auth.Models;
using Nozomi.Infra.Api.Limiter.Events;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Infra.Api.Limiter.HostedServices;
using Nozomi.Infra.Api.Limiter.Services;
using Nozomi.Infra.Api.Limiter.Services.Interfaces;
using Nozomi.Infra.Auth.Events.UserEvent;
using Nozomi.Preprocessing.Extensions;
using Nozomi.Repo.Auth.Data;
using StackExchange.Redis;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Nozomi.Api.Limiter
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
            if (WebHostEnvironment.IsProduction())
            {
                var vaultUrl = Configuration["vaultUrl"];
                var vaultToken = Configuration["vaultToken"];

                if (string.IsNullOrEmpty(vaultToken))
                    throw new SystemException("Invalid vault token.");

                var authMethod = new TokenAuthMethodInfo(vaultToken);
                var vaultClientSettings = new VaultClientSettings(
                    !string.IsNullOrWhiteSpace(vaultUrl) ? vaultUrl : "https://blackbox.nozomi.one:8200",
                    authMethod);
                var vaultClient = new VaultClient(vaultClientSettings);
                
                var vault = vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("api-limiter")
                    .GetAwaiter()
                    .GetResult().Data;
                
                services.ConfigureRedisMultiplexers((string) vault["redis-api-user"], 
                    (string) vault["redis-api-event"]);

                services.AddDbContextPool<AuthDbContext>(options =>
                {
                    options.UseNpgsql((string) vault["auth"], 
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
                var redisEventsStr = Configuration.GetConnectionString("EventsLocalRedis:" 
                                                                       + Environment.MachineName);

                if (WebHostEnvironment.IsStaging())
                {
                    redisStr = Configuration.GetConnectionString("StageRedis");
                    redisEventsStr = Configuration.GetConnectionString("EventsStageRedis");
                }

                services.ConfigureRedisMultiplexers(redisStr, 
                    redisEventsStr);

                services.AddDbContextPool<AuthDbContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("LocalAuth:" + Environment.MachineName), 
                        nozomiDbContextBuilder => { nozomiDbContextBuilder.EnableRetryOnFailure(); }
                    );
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
            }
            
            services.AddControllers();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AuthDbContext>();

            // Service layer injections
            services.AddTransient<IApiKeyUserRedisEvent, ApiKeyUserRedisEvent>();
            services.AddTransient<INozomiRedisEvent, NozomiRedisEvent>();
            services.AddTransient<IUserEvent, UserEvent>();

            services.AddTransient<IApiKeyEventsService, ApiKeyEventsService>();
            services.AddTransient<INozomiRedisService, NozomiRedisService>();

            // Hosted service injections
            services.AddHostedService<ApiKeyEventHostedService>();
            services.AddHostedService<ApiKeyUserHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}