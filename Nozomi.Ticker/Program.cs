using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Gelf.Extensions.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Native.Collections;
using Nozomi.Repo.Data;
using SlackLogger;

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
                        .AddDebug()
                        .AddEventSourceLogger()
                        .AddGelf()
                        .AddSlack();
                    
//                    builder.AddSlack(options =>
//                    {
//                        options.WebhookUrl = context.Configuration.GetSection("Logging").GetSection("Slack")["WebhookUrl"];
//                        options.LogLevel = LogLevel.Information;
//                        options.NotificationLevel = LogLevel.None;
//                        //options.Environment = env.EnvironmentName;
//                        options.Channel = "#mychannel";
//                        options.SanitizeOutputFunction = output => Regex.Replace(output, "@[^\\.@-]", "");
//                    });
                });
            }

            var host = hostBuilder.Build();
            
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var root = config.Build();
                    var vault = root["KeyVault:Vault"];
                    if(!string.IsNullOrEmpty(vault))
                    {
                        config.AddAzureKeyVault(
                            $"https://{root["KeyVault:Vault"]}.vault.azure.net/",
                            root["KeyVault:ClientId"],
                            root["KeyVault:ClientSecret"]);
                    }
                })
                .UseKestrel(c => c.AddServerHeader = false)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>();
    }
}