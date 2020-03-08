using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Nozomi.Preprocessing.Filters;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.Data;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Nozomi.Payment
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }
        
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
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
                    .AddDbContext<AuthDbContext>(options =>
                        {
                            options.UseNpgsql(str);
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

                var nozomiVault = vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("payments")
                    .GetAwaiter()
                    .GetResult().Data;

                var mainDb = (string) nozomiVault["main"];
                if (string.IsNullOrEmpty(mainDb))
                    throw new SystemException("Invalid main database configuration");
                // Database
                services.AddDbContext<AuthDbContext>(options =>
                {
                    options.UseNpgsql(mainDb
                        , builder =>
                        {
                            builder.EnableRetryOnFailure();
                        }
                    );
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }, ServiceLifetime.Transient);
            }
            
            services.AddScoped<ICurrencyEvent, CurrencyEvent>();
            services.AddScoped<ICurrencyPairEvent, CurrencyPairEvent>();
            services.AddScoped<ICurrencyTypeEvent, CurrencyTypeEvent>();
            services.AddScoped<IRequestEvent, RequestEvent>();
            services.AddTransient<IComponentService, ComponentService>();
            services.AddTransient<IRequestService, RequestService>();
            
            services.AddControllers(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var scope = 
                app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = scope.ServiceProvider.GetService<AuthDbContext>())
                context.Database.Migrate();
            
            app.UseEndpoints(endpoints =>
            {
                // MapControllers adds support for attribute-routed controllers.
                // MapAreaControllerRoute adds a conventional route for controllers
                // in an area.
                // MapControllerRoute adds a conventional route for controllers.
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });

            app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
        }
    }
}