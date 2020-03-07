using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Nozomi.Infra.Api.Limiter.Events;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Infra.Api.Limiter.Services;
using Nozomi.Infra.Api.Limiter.Services.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Api
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
            services.AddControllers();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(GlobalApiVariables.CURRENT_API_VERSION, new OpenApiInfo {
                    Title = "Nozomi API", 
                    Version = GlobalApiVariables.CURRENT_API_REVISION.ToString()
                });
            });

            services.AddTransient<INozomiRedisEvent, NozomiRedisEvent>();
            services.AddTransient<IRequestEvent, RequestEvent>();

            services.AddTransient<IApiKeyEventsService, ApiKeyEventsService>();
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

            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{GlobalApiVariables.CURRENT_API_VERSION}/swagger.json", 
                    $"Nozomi API rev. {GlobalApiVariables.CURRENT_API_REVISION}");
            });
        }
    }
}