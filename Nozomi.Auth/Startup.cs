// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Nozomi.Base.Auth.Models;
using Nozomi.Infra.Auth.Services.Address;
using Nozomi.Infra.Blockchain.Auth.Events;
using Nozomi.Infra.Blockchain.Auth.Events.Interfaces;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Context;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Nozomi.Auth
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                // Greet the beloved dev
                Console.WriteLine(@"Welcome to the dev environment, your machine is named: " + Environment.MachineName);
                
                // Postgres DB Setup
                var str = Configuration.GetConnectionString("Local:" + Environment.MachineName);
                services.AddDbContext<AuthDbContext>(options =>
                    options.UseNpgsql(str));
                var coreStr = Configuration.GetConnectionString("LocalCore:" + Environment.MachineName);
                services.AddDbContext<NozomiDbContext>(options =>
                    options.UseNpgsql(coreStr));
            }
            else
            {
                var vaultToken = Configuration["vaultToken"];

                if (string.IsNullOrEmpty(vaultToken))
                    throw new SystemException("Invalid vault token.");

                var authMethod = new TokenAuthMethodInfo(vaultToken);
                var vaultClientSettings = new VaultClientSettings("http://165.22.250.169:8200", authMethod);
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
                        , nozomiDbContextBuilder =>
                        {
                            nozomiDbContextBuilder.EnableRetryOnFailure();
                        }
                    );
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }, ServiceLifetime.Transient);

                var authDb = (string) nozomiVault["coreauth"];
                if (string.IsNullOrEmpty(authDb))
                    throw new SystemException("Invalid main database configuration");
                // Database
                services.AddDbContext<AuthDbContext>(options =>
                {
                    options.UseNpgsql(authDb
                        , authDbContextBuilder =>
                        {
                            authDbContextBuilder.EnableRetryOnFailure();
                        }
                    );
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }, ServiceLifetime.Transient);
            }

            services
                .AddEntityFrameworkNpgsql()
                .AddIdentity<User, Role>(options =>
                {
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2);

            var identityConfig = new IdentityConfig(HostingEnvironment);

            var builder = services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddInMemoryIdentityResources(identityConfig.GetIdentityResources())
                .AddInMemoryApiResources(identityConfig.GetApis())
                .AddInMemoryClients(identityConfig.GetClients())
                .AddAspNetIdentity<User>();

            if (HostingEnvironment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                var vaultToken = Configuration["vaultToken"];

                if (string.IsNullOrEmpty(vaultToken))
                    throw new SystemException("Invalid vault token.");

                var authMethod = new TokenAuthMethodInfo(vaultToken);
                var vaultClientSettings = new VaultClientSettings("http://165.22.250.169:8200", authMethod);
                var vaultClient = new VaultClient(vaultClientSettings);

                var nozomiVault = vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("nozomi")
                    .GetAwaiter()
                    .GetResult().Data;

                var cert = new X509Certificate2(Encoding.UTF8.GetBytes((string) nozomiVault["auth-signing-cert"])
                    , (string) nozomiVault["auth-signing-key"]);
                
                // https://stackoverflow.com/questions/49042474/addsigningcredential-for-identityserver4
                builder.AddSigningCredential(cert);
            }

            // Database
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<IUnitOfWork<AuthDbContext>, UnitOfWork<AuthDbContext>>();
            services.AddTransient<IDbContext, AuthDbContext>();
            services.AddTransient<IUnitOfWork<NozomiDbContext>, UnitOfWork<NozomiDbContext>>();
            services.AddTransient<IDbContext, NozomiDbContext>();

            services.AddScoped<IAddressEvent, AddressEvent>();
            services.AddScoped<IValidatingEvent, ValidatingEvent>();
            
            services.AddTransient<IAddressService, AddressService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseAutoDbMigration(HostingEnvironment);
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseIdentityServer();
            
            // "default", "{controller=Home}/{action=Index}/{id?}"
            app.UseMvcWithDefaultRoute();
        }
        
        private SigningCredentials CreateSigningCredential()
        {
            var credentials = new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.RsaSha256Signature);

            return credentials;
        }
        private RSA GetRSACryptoServiceProvider()
        {
            // https://stackoverflow.com/questions/54180171/cspkeycontainerinfo-requires-windows-cryptographic-api-capi-which-is-not-av
            return RSA.Create(4096);
        }
        private SecurityKey GetSecurityKey()
        {
            return new RsaSecurityKey(GetRSACryptoServiceProvider());
        }
    }
}