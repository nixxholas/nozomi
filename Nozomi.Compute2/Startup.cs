using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nozomi.Infra.Compute.Events;
using Nozomi.Infra.Compute.Events.Interfaces;
using Nozomi.Infra.Compute.Services;
using Nozomi.Infra.Compute.Services.Interfaces;
using Nozomi.Repo.Compute.Data;
using Nozomi.Repo.Data;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Nozomi.Compute2
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            if (WebHostEnvironment.IsDevelopment())
            {
                // Greet the beloved dev
                Console.WriteLine(@"Welcome to the dev environment, your machine is named: " + Environment.MachineName);

                // Postgres DB Setup
                var str = Configuration.GetConnectionString("Local:" + @Environment.MachineName);
                var computeStr = Configuration.GetConnectionString("LocalCompute:" 
                                                                   + @Environment.MachineName);

                services
                    .AddEntityFrameworkNpgsql()
                    .AddDbContextPool<NozomiDbContext>(options =>
                        {
                            options.UseNpgsql(str);
                            options.EnableSensitiveDataLogging();
                            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                        });
                
                services
                    .AddEntityFrameworkNpgsql()
                    .AddDbContext<NozomiComputeDbContext>(options =>
                        {
                            options.UseNpgsql(computeStr);
                            options.EnableSensitiveDataLogging();
                            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                        },
                        ServiceLifetime.Transient);
            }
            else
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

                var nozomiVault = vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("nozomi")
                    .GetAwaiter()
                    .GetResult().Data;

                var mainDb = (string) nozomiVault["main"];
                if (string.IsNullOrEmpty(mainDb))
                    throw new SystemException("Invalid main database configuration");
                // Database
                services
                    .AddDbContextPool<NozomiDbContext>(options =>
                {
                    options.UseNpgsql(mainDb
                        , builder =>
                        {
                            builder.EnableRetryOnFailure();
                        }
                    );
                    options.EnableSensitiveDataLogging();
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
                
                var computeDb = (string) nozomiVault["compute"];
                if (string.IsNullOrEmpty(computeDb))
                    throw new SystemException("Invalid main database configuration");
                services
                    .AddEntityFrameworkNpgsql()
                    .AddDbContext<NozomiComputeDbContext>(options =>
                        {
                            options.UseNpgsql(computeDb
                                , builder =>
                                {
                                    builder.EnableRetryOnFailure();
                                }
                            );
                            options.EnableSensitiveDataLogging();
                            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                        },
                        ServiceLifetime.Transient);
            }
            
            // Service injections
            services.AddScoped<IComputeEvent, ComputeEvent>();
            services.AddScoped<IComputeValueEvent, ComputeValueEvent>();

            services.AddTransient<IComputeService, ComputeService>();
            services.AddTransient<IComputeValueService, ComputeValueService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => 
                    { await context.Response.WriteAsync("Hello World!"); });
            });
        }
    }
}