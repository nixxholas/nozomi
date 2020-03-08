﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Infra.Auth.Events.UserEvent;
using Nozomi.Infra.Auth.Events.EmailSender;
using Nozomi.Infra.Auth.Services.Address;
using Nozomi.Infra.Auth.Services.QuotaClaims;
using Nozomi.Infra.Auth.Services.User;
using Nozomi.Infra.Blockchain.Auth.Events;
using Nozomi.Infra.Blockchain.Auth.Events.Interfaces;
using Nozomi.Infra.Payment.Events.Bootstripe;
using Nozomi.Infra.Payment.Services.Bootstripe;
using Nozomi.Infra.Payment.Services.DisputesHandling;
using Nozomi.Infra.Payment.Services.InvoicesHandling;
using Nozomi.Preprocessing.Events;
using Nozomi.Preprocessing.Events.Interfaces;
using Nozomi.Preprocessing.Options;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.Data;
using Stripe;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Nozomi.Auth
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        private readonly string NozomiSpecificOrigins = "_nozomiSpecificOrigins";

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
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
                
                // Setup all the 3rd party library options
                services.Configure<SendgridOptions>(options =>
                {
                    options.SendGridKey = "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";
                    options.SendGridUser = "Nozomi Auth";
                });
                services.Configure<StripeOptions>(options =>
                {
                    options.DefaultPlanId = Configuration["Stripe:DefaultPlanId"];
                    options.ProductId = Configuration["Stripe:ProductId"];
                    options.PublishableKey = Configuration["Stripe:PublishableKey"];
                    options.SecretKey = Configuration["Stripe:SecretKey"];
                });
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
                        , nozomiDbContextBuilder => { nozomiDbContextBuilder.EnableRetryOnFailure(); }
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
                        , authDbContextBuilder => { authDbContextBuilder.EnableRetryOnFailure(); }
                    );
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }, ServiceLifetime.Transient);

                // Vault props for Sendgrid
                var sendgridKey = (string) nozomiVault["sendgrid-key"];
                var sendgridUser = (string) nozomiVault["sendgrid-user"];
                // Setup all the 3rd party library options
                services.Configure<SendgridOptions>(options =>
                {
                    if (string.IsNullOrEmpty(sendgridKey))
                        throw new KeyNotFoundException("SendgridOptions: Invalid Sendgrid Key!");
                    options.SendGridKey = sendgridKey;
                    if (string.IsNullOrEmpty(sendgridUser))
                        throw new KeyNotFoundException("SendgridOptions: Invalid Sendgrid User!");
                    options.SendGridUser = sendgridUser;
                });

                var stripeDefaultPlanId = (string) nozomiVault["StripeDefaultPlanId"];
                var stripeProductId = (string) nozomiVault["StripeProductId"];
                var stripePublishableKey = (string) nozomiVault["StripePublishableKey"];
                var stripeSecretKey = (string) nozomiVault["StripeSecretKey"];
                services.Configure<StripeOptions>(options =>
                {
                    if (string.IsNullOrEmpty(stripeDefaultPlanId))
                        throw new KeyNotFoundException("StripeOptions: Invalid Stripe Default Plan Id!");
                    options.DefaultPlanId = stripeDefaultPlanId;
                    if (string.IsNullOrEmpty(stripeProductId))
                        throw new KeyNotFoundException("StripeOptions: Invalid Stripe target Product Id!");
                    options.ProductId = stripeProductId;
                    if (string.IsNullOrEmpty(stripePublishableKey))
                        throw new KeyNotFoundException("StripeOptions: Invalid Stripe Publishable Key!");
                    options.PublishableKey = stripePublishableKey;
                    if (string.IsNullOrEmpty(stripeSecretKey))
                        throw new KeyNotFoundException("StripeOptions: Invalid Stripe Secret!");
                    options.SecretKey = stripeSecretKey;
                });
            }

            // Cross Origin Requests for Nozomi Auth
            services.AddCors(options =>
            {
                options.AddPolicy(NozomiSpecificOrigins,
                    policyBuilder =>
                    {
                        if (HostingEnvironment.IsProduction())
                            policyBuilder.WithOrigins("https://nozomi.one", "https://www.nozomi.one")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        else
                            policyBuilder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                    });
            });

            if (HostingEnvironment.IsDevelopment())
                services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation()
                    .AddNewtonsoftJson(options =>
                        {
                            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            else
                services.AddControllersWithViews()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddRazorPages();

            services
                .AddEntityFrameworkNpgsql()
                .AddIdentity<User, Role>(options =>
                {
                    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;

                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

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

                var authSigningKey = (string) nozomiVault["auth-signing-key"];
                if (string.IsNullOrWhiteSpace(authSigningKey))
                    throw new Exception("Null auth signing key.");

                // Obtain the raw certificate encoded in base64str
                var authSigningCert = (string) nozomiVault["auth-signing-cert"];
                if (string.IsNullOrEmpty(authSigningCert))
                    throw new NullReferenceException("Auth signing cert from vault is empty.");

                var certificate = new X509Certificate2(
                    // https://stackoverflow.com/questions/25919387/converting-file-into-base64string-and-back-again
                    Convert.FromBase64String(authSigningCert)
                    , authSigningKey);

                // https://stackoverflow.com/questions/49042474/addsigningcredential-for-identityserver4
                builder.AddSigningCredential(certificate);
            }

            var authority = HostingEnvironment.IsProduction()
                ? "https://auth.nozomi.one"
                : "https://localhost:6001/";
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = authority;
                    options.RequireHttpsMetadata = false;

                    options.Audience = "nozomi.web";
                });

            // Database
            // services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            //
            // services.AddTransient<IUnitOfWork<AuthDbContext>, UnitOfWork<AuthDbContext>>();
            // services.AddTransient<IDbContext, AuthDbContext>();
            // services.AddTransient<NozomiDbContext, UnitOfWork<NozomiDbContext>>();
            // services.AddTransient<IDbContext, NozomiDbContext>();

            services.AddTransient<IAddressEvent, AddressEvent>();
            services.AddTransient<IValidatingEvent, ValidatingEvent>();

            services.AddTransient<IEmailSender, EmailSender>();
            
            //Infra.Auth Services & Events
            services.AddTransient<IUserEvent, UserEvent>();
            services.AddTransient<IAuthEmailSender, AuthEmailSender>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IQuotaClaimsService, QuotaClaimsService>();
            
            //Infra.Payment Services & Events
            services.AddTransient<IBootstripeEvent, BootstripeEvent>();
            services.AddScoped<IBootstripeService, BootstripeService>();
            services.AddScoped<IDisputesHandlingService, DisputesHandlingService>();
            services.AddScoped<IInvoicesHandlingService, InvoicesHandlingService>();
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

            app.UseStaticFiles();
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

            app.UseCookiePolicy();
            app.UseIdentityServer();

            app.UseRouting();

            // Cross origin requests DI
            // With endpoint routing, the CORS middleware must be configured to execute between the calls to
            // UseRouting and UseEndpoints. Incorrect configuration will cause the middleware to stop functioning correctly.
            app.UseCors(NozomiSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            // "default", "{controller=Home}/{action=Index}/{id?}"
            // app.UseMvcWithDefaultRoute();

            app.UseEndpoints(options =>
            {
                options.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                options.MapRazorPages();
            });
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