// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Nozomi.Auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
//            var seed = args.Any(x => x == "/seed");
//            if (seed) args = args.Except(new[] {"/seed"}).ToArray();

            var host = CreateWebHostBuilder(args).Build();

//            if (seed)
//            {
//                var config = host.Services.GetRequiredService<IConfiguration>();
//                var connectionString = config.GetConnectionString("DefaultConnection");
//                SeedData.EnsureSeedData(connectionString);
//                return;
//            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    options.AddServerHeader = false;
                    
                    // options.Listen(IPAddress.Any, 8080);         // http:*:80
                    
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    var isDevelopment = environment == Environments.Development;
                    var validateSSL = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SSLCERT_PATH"));
                    
                    // HTTPS Configuration
                    if (!System.Diagnostics.Debugger.IsAttached && !isDevelopment && validateSSL)
                    {
                        var hasHttpsPortConfigured = int.TryParse(Environment.GetEnvironmentVariable("HTTPS_PORT")
                            , out var httpsPort);
                        if (!hasHttpsPortConfigured)
                        {
                            httpsPort = 5001; // Default port

                            Console.WriteLine("HTTPS port not configured! Self configuring to 5001.");
                        }
                        
                        var certPath = Environment.GetEnvironmentVariable("SSLCERT_PATH");
                        if (string.IsNullOrEmpty(certPath)) {
                            certPath = "nozomi.pfx"; // Deefault

                            Console.WriteLine("SSLCERT_PATH not configured! Self configuring to nozomi.pfx");
                        }
                        
                        var certPassword = Environment.GetEnvironmentVariable("SSLCERT_PASSWORD");
                        if (string.IsNullOrEmpty(certPassword))
                        {
                            certPassword = "290597"; // Deefault

                            Console.WriteLine("SSLCERT_PASSWORD not configured! Self configuring to the defaults.");
                        }
                        
                        options.Listen(IPAddress.Any, httpsPort, listenOptions =>
                        {
                            var cert = new X509Certificate2(certPath, certPassword);

                            listenOptions.UseHttps(cert);
                        });
                    }
                })
                .UseSerilog((context, configuration) =>
                {
                    configuration
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("System", LogEventLevel.Warning)
                        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.File(@"identityserver4_log.txt")
                        .WriteTo.Console(
                            outputTemplate:
                            "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                            theme: AnsiConsoleTheme.Literate);
                });
        }
    }
}