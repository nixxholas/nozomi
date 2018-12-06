using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gelf.Extensions.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Core.Helpers.Native.Collections;
using Nozomi.Repo.Data;

namespace Nozomi.Ticker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateWebHostBuilder(args);
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            if (!string.IsNullOrEmpty(env) && env.Equals("Production"))
            {
                hostBuilder.ConfigureLogging((context, builder) =>
                {
                    // Read GelfLoggerOptions from appsettings.json
                    builder.Services.Configure<GelfLoggerOptions>(context.Configuration.GetSection("Graylog"));

                    // Optionally configure GelfLoggerOptions further.
                    builder.Services.PostConfigure<GelfLoggerOptions>(options =>
                        options.AdditionalFields["machine_name"] = Environment.MachineName);

                    // Read Logging settings from appsettings.json and add providers.
                    builder.AddConfiguration(context.Configuration.GetSection("Logging"))
                        .AddConsole()
                        //.AddDebug()
                        .AddGelf();
                });
            }

            var host = hostBuilder.Build();
            
            using (var scope = host.Services.CreateScope())
            {
                // Retrieve your DbContext isntance here
                var dbContext = scope.ServiceProvider.GetRequiredService<NozomiDbContext>();

                if (env != null && !env.Equals("Production"))
                {
                    dbContext.Database.EnsureDeleted();
                    dbContext.Database.EnsureCreated();
                }
                else
                {
                    dbContext.Database.Migrate();
                }
                // place your DB seeding code here
                //DbSeeder.Seed(dbContext);
            }
            
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(c => c.AddServerHeader = false)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>();
    }
}