using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Nozomi.Web2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    options.AddServerHeader = false;

                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    var isDevelopment = environment == Environments.Development;
                    if (!System.Diagnostics.Debugger.IsAttached && !isDevelopment)
                    {
                        var hasHttpsPortConfigured = int.TryParse(Environment.GetEnvironmentVariable("HTTPS_PORT")
                            , out var port);
                        if (!hasHttpsPortConfigured)
                        {
                            port = 5001; // Default port

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
                        
                        options.Listen(IPAddress.Loopback, port, listenOptions =>
                        {
                            var cert = new X509Certificate2(certPath, certPassword);

                            listenOptions.UseHttps(cert);
                        });
                    }
                })
                .UseStartup<Startup>();
    }
}
