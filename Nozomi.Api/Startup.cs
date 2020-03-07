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
using Microsoft.OpenApi.Models;
using Nozomi.Api.Extensions;
using Nozomi.Infra.Api.Limiter.Events;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Infra.Api.Limiter.Services;
using Nozomi.Infra.Api.Limiter.Services.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.Data;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;
using Swashbuckle.AspNetCore.Filters;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Nozomi.Api
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

                var authMethod = new TokenAuthMethodInfo(vaultToken);
                var vaultClientSettings = new VaultClientSettings(
                    !string.IsNullOrWhiteSpace(vaultUrl) ? vaultUrl : "https://blackbox.nozomi.one:8200",
                    authMethod);
                var vaultClient = new VaultClient(vaultClientSettings);
                
                var vault = vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("api")
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
            
            services.AddControllers();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(GlobalApiVariables.CURRENT_API_VERSION, new OpenApiInfo {
                    Title = "Nozomi API", 
                    Version = GlobalApiVariables.CURRENT_API_REVISION.ToString()
                });

                config.OperationFilter<AddResponseHeadersFilter>();
                
                config.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using an API Key. Example: \"{token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                config.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            services.AddTransient<INozomiRedisEvent, NozomiRedisEvent>();
            services.AddTransient<IRequestEvent, RequestEvent>();

            services.AddTransient<IApiKeyEventsService, ApiKeyEventsService>();
            services.AddTransient<IRequestService, RequestService>();
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

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/{documentName}/swagger.json";
            });
            
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Nozomi API Documentation";
                c.RoutePrefix = "";
                c.SwaggerEndpoint($"/{GlobalApiVariables.CURRENT_API_VERSION}/swagger.json", 
                    $"Nozomi API rev. {GlobalApiVariables.CURRENT_API_REVISION}");
            });
        }
    }
}