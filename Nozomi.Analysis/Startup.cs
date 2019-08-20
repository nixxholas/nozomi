using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Analysis.StartupExtensions;
using Nozomi.Base.Core.Configurations;
using Nozomi.Repo.Data;
using Nozomi.Repo.Identity.Data;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;

namespace Nozomi.Analysis
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                // Greet the beloved dev
                Console.WriteLine(@"Welcome to the dev environment, your machine is named: " + Environment.MachineName);

                // Postgres DB Setup
                var str = Configuration.GetConnectionString("Local:" + @Environment.MachineName);

                services
                    .AddEntityFrameworkNpgsql()
                    .AddDbContext<NozomiDbContext>(options =>
                        {
                            options.UseNpgsql(str);
                            options.EnableSensitiveDataLogging();
                            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                        },
                        ServiceLifetime.Transient);

                services
                    .AddEntityFrameworkNpgsql()
                    .AddDbContext<NozomiAuthContext>(options =>
                    {
                        options.UseNpgsql(Configuration.GetConnectionString("LocalAuth:" + Environment.MachineName));
                        options.EnableSensitiveDataLogging(false);
                    });
                
                services.Configure<StripeSettings>(ss =>
                {
                    ss.SecretKey = "sk_test_rnlKG1tOlB0d4DlZgONFnVO300wCCutjx4";
                    ss.PublishableKey = "pk_test_XziapDdDCWhkxmIgjldplFaF00L7FSFhvi";
                });
            }
            else
            {
                var vaultToken = Configuration["vaultToken"];

                if (string.IsNullOrEmpty(vaultToken))
                    throw new SystemException("Invalid vault token.");

                var authMethod = new TokenAuthMethodInfo(vaultToken);
                var vaultClientSettings = new VaultClientSettings("http://vault.nozomi.one:8200", authMethod);
                var vaultClient = new VaultClient(vaultClientSettings);

                var nozomiVault = vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("nozomi")
                    .GetAwaiter()
                    .GetResult().Data;

                var mainDb = (string) nozomiVault["main"];
                if (string.IsNullOrEmpty(mainDb))
                    throw new SystemException("Invalid main database configuration");
                // Database
                services.AddDbContext<NozomiDbContext>(options =>
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
                }, ServiceLifetime.Transient);

                var authDb = (string) nozomiVault["auth"];
                if (string.IsNullOrEmpty(authDb))
                    throw new SystemException("Invalid auth database configuration");
                services.AddDbContext<NozomiAuthContext>(options =>
                {
                    options.UseNpgsql(authDb
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
                });

                var stripeVault = vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("stripe")
                    .GetAwaiter()
                    .GetResult().Data;

                var stripePriv = (string) stripeVault["testpriv"];
                if (string.IsNullOrEmpty(stripePriv))
                    throw new SystemException("Invalid main database configuration");
                var stripePub = (string) stripeVault["testpub"];
                if (string.IsNullOrEmpty(stripePub))
                    throw new SystemException("Invalid main database configuration");

                services.Configure<StripeSettings>(ss =>
                {
                    ss.SecretKey = stripePriv;
                    ss.PublishableKey = stripePub;
                });
            }

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.ConfigureRepoLayer();
            services.ConfigureEvents();
            services.ConfigureServiceLayer();
            services.ConfigureHostedServices();
            services.ConfigureNozomiAuth();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseAutoDbMigration(env);
        }
    }
}