﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Infra.Syncing.Events;
using Nozomi.Infra.Syncing.Events.Interfaces;
using Nozomi.Infra.Syncing.HostedServices;
using Nozomi.Infra.Syncing.Services;
using Nozomi.Infra.Syncing.Services.Interfaces;
using Nozomi.Repo.Data;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Analysis;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Nozomi.Compute
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
                    .AddDbContext<NozomiDbContext>(options =>
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

                var nozomiVault = vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("compute-old")
                    .GetAwaiter()
                    .GetResult().Data;

                var mainDb = (string) nozomiVault["main"];
                if (string.IsNullOrEmpty(mainDb))
                    throw new SystemException("Invalid main database configuration");
                // Database
                services
                    .AddDbContext<NozomiDbContext>(options =>
                {
                    options.UseNpgsql(mainDb
                        , builder =>
                        {
                            builder.EnableRetryOnFailure();
                        }
                    );
                    options.EnableSensitiveDataLogging();
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }, ServiceLifetime.Transient);
            }
            
            // services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            //
            // services.AddTransient<NozomiDbContext, UnitOfWork<NozomiDbContext>>();
            // services.AddTransient<IDbContext, NozomiDbContext>();

            services.AddScoped<IAnalysedComponentEvent, AnalysedComponentEvent>();
            services.AddScoped<IAnalysedHistoricItemEvent, AnalysedHistoricItemEvent>();
            services.AddScoped<ICurrencyEvent, CurrencyEvent>();
            services.AddScoped<ICurrencyPairEvent, CurrencyPairEvent>();
            services.AddScoped<IProcessAnalysedComponentService, ProcessAnalysedComponentService>();
            services.AddScoped<IRequestEvent, RequestEvent>();
            services.AddScoped<IComponentEvent, ComponentEvent>();
            services.AddScoped<IXAnalysedComponentEvent, XAnalysedComponentEvent>();
            services.AddTransient<IAnalysedHistoricItemService, AnalysedHistoricItemService>();
            services.AddTransient<IComponentHistoricItemService, ComponentHistoricItemService>();
            services.AddTransient<IComponentService, ComponentService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddHostedService<AcAnalysisHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
        }
    }
}