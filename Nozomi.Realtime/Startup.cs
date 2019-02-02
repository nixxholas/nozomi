using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Realtime.Infra.Service.Hubs;
using Nozomi.Realtime.StartupExtensions;

namespace Nozomi.Realtime
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        
        public IConfiguration _configuration { get; set; }

        private readonly IHostingEnvironment _environment;
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // EF Core, UoW & Repository layer injection
            services.ConfigureDatabase(_configuration, _environment);
            
            // SignalR injection
            services.ConfigureHubs();
            
            // HostedServices injection
            services.ConfigureTasks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSignalR(route =>
            {
                route.MapHub<TickerHub>(TickerHub._hubPath);
            });

            app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
        }
    }
}