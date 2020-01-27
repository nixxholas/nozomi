using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

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

                    if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")) 
                        && Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                            .Equals("production", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var hasHttpsPortConfigured = int.TryParse(Environment.GetEnvironmentVariable("HTTPS_PORT")
                            , out var port);
                        var certPassword = Environment.GetEnvironmentVariable("SSLCERT_PASSWORD");
                        if (!string.IsNullOrEmpty(certPassword))
                            certPassword = "123456"; // Deefault
                        
                        if (!hasHttpsPortConfigured)
                        {
                            port = 5001; // Default port

                            Console.WriteLine("HTTPS port not configured! Self configuring to 443.");
                        }
                        
                        options.Listen(IPAddress.Loopback, port, listenOptions =>
                        {
                            var cert = new X509Certificate2("nozomi.pfx", certPassword);

                            listenOptions.UseHttps(cert);
                        });
                    }
                })
                .UseStartup<Startup>();
    }
}
