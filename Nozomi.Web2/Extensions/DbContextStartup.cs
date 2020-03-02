using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nozomi.Repo.Data;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Nozomi.Web2.Extensions
{
    public static class DbContextStartup
    {
        public static void AddDbContextInjections(this IServiceCollection services)
        {
            if (!Startup.Environment.IsProduction())
            {
                // Greet the beloved dev
                Console.WriteLine(@"Welcome to the dev environment, your machine is named: " + Environment.MachineName);

                // Postgres DB Setup
                var str = Startup.Configuration.GetConnectionString("Local:" + @Environment.MachineName);

                services
                    // This causes memory size errors
                    // https://stackoverflow.com/questions/58406143/unexpected-cache-entry-must-specify-a-value-for-size-when-sizelimit-is-set-mes
                    //.AddEntityFrameworkNpgsql()
                    .AddDbContextPool<NozomiDbContext>(options =>
                    {
                        options.UseNpgsql(str);
                        options.EnableSensitiveDataLogging();
                        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    });
            }
            else
            {
                var vaultUrl = Startup.Configuration["vaultUrl"];
                var vaultToken = Startup.Configuration["vaultToken"];

                if (string.IsNullOrEmpty(vaultToken))
                    throw new SystemException("Invalid vault token.");

                var authMethod = new TokenAuthMethodInfo(vaultToken);
                var vaultClientSettings = new VaultClientSettings(
                    !string.IsNullOrWhiteSpace(vaultUrl) ? vaultUrl : "https://blackbox.nozomi.one:8200",
                    authMethod);
                var vaultClient = new VaultClient(vaultClientSettings);

                var nozomiVault = vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("nozomi")
                    .GetAwaiter()
                    .GetResult().Data;

                var mainDb = (string) nozomiVault["main"];
                if (string.IsNullOrEmpty(mainDb))
                    throw new SystemException("Invalid main database configuration");
                // Database
                services
                    // This causes memory size errors
                    // https://stackoverflow.com/questions/58406143/unexpected-cache-entry-must-specify-a-value-for-size-when-sizelimit-is-set-mes
                    // .AddEntityFrameworkNpgsql()
                    .AddDbContextPool<NozomiDbContext>(options =>
                    {
                        options.UseNpgsql(mainDb
                            , builder =>
                            {
                                builder.EnableRetryOnFailure();
//                            builder.ProvideClientCertificatesCallback(certificates =>
//                            {
//                                var cert = new X509Certificate2("ca-certificate.crt");
//                                certificates.Add(cert);
//                            });
                            }
                        );
                        options.EnableSensitiveDataLogging(false);
                        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    });
            }

            // return services;
        }
    }
}