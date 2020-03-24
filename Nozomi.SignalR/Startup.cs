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
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Api.Limiter.Handlers;
using Nozomi.Infra.SignalR.Hubs;
using Nozomi.Preprocessing.Options;
using Nozomi.Repo.Data;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Interfaces;
using Nozomi.SignalR.Extensions;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Nozomi.SignalR
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
            if (WebHostEnvironment.IsStaging() || WebHostEnvironment.IsProduction())
            {
                var vaultUrl = Configuration["vaultUrl"];
                var vaultToken = Configuration["vaultToken"];

                if (string.IsNullOrEmpty(vaultToken))
                    throw new SystemException("Invalid vault token.");

                services.AddHsts(opt =>
                {
                    opt.Preload = true;
                    opt.IncludeSubDomains = true;
                    opt.MaxAge = TimeSpan.FromDays(60);
                });

                var authMethod = new TokenAuthMethodInfo(vaultToken);
                var vaultClientSettings = new VaultClientSettings(
                    !string.IsNullOrWhiteSpace(vaultUrl) ? vaultUrl : "https://blackbox.nozomi.one:8200",
                    authMethod);
                var vaultClient = new VaultClient(vaultClientSettings);
                
                var vault = vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("signalr")
                    .GetAwaiter()
                    .GetResult().Data;
                
                services.ConfigureRedis((string) vault["redis"]);

                services.AddDbContextPool<NozomiDbContext>(options =>
                {
                    options.UseNpgsql((string) vault["main"], 
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
                
                services.ConfigureRedis(redisStr);

                services.AddDbContextPool<NozomiDbContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("Local:" + Environment.MachineName), 
                        nozomiDbContextBuilder => { nozomiDbContextBuilder.EnableRetryOnFailure(); }
                    );
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
            }
            
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                    options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                })
                .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                    ApiKeyAuthenticationOptions.DefaultScheme, o => { });
            services.AddAuthorization();
            
            services.AddControllers();

            services.AddSignalR()
                .AddMessagePackProtocol(options =>
                {
                    options.FormatterResolvers = new List<MessagePack.IFormatterResolver>()
                    {
                        MessagePack.Resolvers.StandardResolver.Instance
                    };
                });

            services.AddTransient<IComponentEvent, ComponentEvent>();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ComponentHub>("/components");
            });
        }
    }
}