// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
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
            if (HostingEnvironment.IsDevelopment() || HostingEnvironment.IsStaging())
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
                var vaultUrl = Configuration["vaultUrl"];
                var vaultToken = Configuration["vaultToken"];

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

            services.AddMvc(options => { options.EnableEndpointRouting = false; })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.Configure<IdentityOptions>(options =>
                options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role);

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
                var vaultUrl = Configuration["vaultUrl"];
                var vaultToken = Configuration["vaultToken"];

                if (string.IsNullOrEmpty(vaultToken))
                    throw new SystemException("Invalid vault token.");

                var authMethod = new TokenAuthMethodInfo(vaultToken);
                var vaultClientSettings = new VaultClientSettings(
                    !string.IsNullOrWhiteSpace(vaultUrl) ? vaultUrl : "https://blackbox.nozomi.one:8200", 
                    authMethod);
                var vaultClient = new VaultClient(vaultClientSettings);

                var nozomiVault = 
                    vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("nozomi")
                    .GetAwaiter()
                    .GetResult().Data;
                
                Console.WriteLine(nozomiVault);

                var authSigningKey = (string) nozomiVault["auth-signing-key"];
                if (string.IsNullOrWhiteSpace(authSigningKey))
                    throw new Exception("Null auth signing key.");

                // Obtain the raw certificate encoded in base64str
                var authSigningCert = (string) nozomiVault["auth-signing-cert"];
                if ((HostingEnvironment.IsProduction() && string.IsNullOrEmpty(authSigningCert)) 
                    || string.IsNullOrEmpty(File.ReadAllText("noz-web.raw")))
                    throw new NullReferenceException("Auth signing cert from vault is empty.");
                
                var certificate = new X509Certificate2(
                    // https://stackoverflow.com/questions/25919387/converting-file-into-base64string-and-back-again
                    HostingEnvironment.IsProduction()
                        ? Convert.FromBase64String(authSigningCert)
                        : Convert.FromBase64String(File.ReadAllText("noz-web.raw"))
                    , authSigningKey);
                
                // https://stackoverflow.com/questions/49042474/addsigningcredential-for-identityserver4
                builder.AddSigningCredential(certificate);
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
                // app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseAutoDbMigration(HostingEnvironment);
            
            // Reverse proxy bypass for OpenID compatibility
            // https://github.com/IdentityServer/IdentityServer4/issues/1331#issuecomment-317049214
            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                RequireHeaderSymmetry = false
            };

            forwardOptions.KnownNetworks.Clear();
            forwardOptions.KnownProxies.Clear();

            // ref: https://github.com/aspnet/Docs/issues/2384
            app.UseForwardedHeaders(forwardOptions);
            
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
            return RSA.Create(2048);
        }
        private SecurityKey GetSecurityKey()
        {
            return new RsaSecurityKey(GetRSACryptoServiceProvider());
        }
    }
}