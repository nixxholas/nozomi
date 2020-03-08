using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Nozomi.Compute2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // CreateHostBuilder(args).Build().Run();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, configuration) =>
                {
                    configuration
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("System", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .WriteTo.Console(
                            outputTemplate:
                            "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                            theme: AnsiConsoleTheme.Literate);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel((context, options) => { options.AllowSynchronousIO = true; })
                        .UseStartup<Startup>();
                });
        
        static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
                 WebHost.CreateDefaultBuilder(args)
                     .UseContentRoot(Directory.GetCurrentDirectory())
                     .UseSerilog((context, configuration) =>
                     {
                         configuration
                             .MinimumLevel.Debug()
                             .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                             .MinimumLevel.Override("System", LogEventLevel.Warning)
                             .Enrich.FromLogContext()
                             .WriteTo.Console(
                                 outputTemplate:
                                 "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                                 theme: AnsiConsoleTheme.Literate);
                     })
                     .UseKestrel(options =>
                     {
                         options.AddServerHeader = false;
                     })
                     .UseStartup<Startup>();
    }
}