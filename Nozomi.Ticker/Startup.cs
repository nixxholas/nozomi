﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Base.BCL.Helpers.Routing;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Options;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.Data;
using Nozomi.Service.Hubs;
using Nozomi.Service.Middleware;
using Nozomi.Ticker.StartupExtensions;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Nozomi.Ticker
{
    public class Startup
    {
        public Startup(IConfiguration configuration, Microsoft.Extensions.Hosting.IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        
        public Microsoft.Extensions.Hosting.IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Environment Inclusion
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            if (!string.IsNullOrEmpty(env) && !env.Equals("production", StringComparison.OrdinalIgnoreCase))
            {
                // Greet the beloved dev
                Console.WriteLine(@"Welcome to the dev environment, your machine is named: " + Environment.MachineName);
                
                // Postgres DB Setup                
                services
                    .AddEntityFrameworkNpgsql()
                    .AddDbContext<NozomiDbContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("Local:" + @Environment.MachineName));
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                },
                    ServiceLifetime.Transient);

                services
                    .AddEntityFrameworkNpgsql()
                    .AddDbContext<AuthDbContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("LocalAuth:" + Environment.MachineName));
                    options.EnableSensitiveDataLogging(false);
                });
            
                // Redis
//                services.AddDistributedRedisCache(option =>
//                {
//                    option.Configuration = "127.0.0.1:6379";
//                    option.InstanceName = "nozomi-cache";
//                });
            
                // Stripe
                services.Configure<StripeOptions>(ss =>
                {
                    ss.SecretKey = Configuration.GetConnectionString("Stripe:TestPriv");
                    ss.PublishableKey = Configuration.GetConnectionString("Stripe:TestPub");
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
                services.AddDbContext<AuthDbContext>(options =>
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
            
                // Redis
//                services.AddDistributedRedisCache(option =>
//                {
//                    option.Configuration = Configuration.GetConnectionString("RedisConfiguration");
//                    option.InstanceName = "nozomi-cache";
//                });
                
                var stripeVault = vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("stripe")
                    .GetAwaiter()
                    .GetResult().Data;
                
                var stripePrivConf = (string) stripeVault["testpriv"];
                var stripePubConf = (string) stripeVault["testpub"];
                if (string.IsNullOrEmpty(stripePrivConf) || string.IsNullOrEmpty(stripePubConf))
                    throw new SystemException("Invalid stripe configuration");
            
                // Stripe
                services.Configure<StripeOptions>(ss =>
                {
                    ss.SecretKey = stripePrivConf;
                    ss.PublishableKey = stripePubConf;
                });
            }
            
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.Secure = CookieSecurePolicy.Always;
            });
            
            services.AddSignalR()
                .AddMessagePackProtocol();
            
            services.ConfigureCors();

            services.AddSession();
            
            services.AddControllersWithViews()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddSessionStateTempDataProvider()
                .AddRazorPagesOptions(o=>
                {
                    o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
                });
            
            // https://stackoverflow.com/questions/36358751/how-do-you-enforce-lowercase-routing-in-asp-net-core
            services.AddRouting(option =>
            {
                option.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
                option.LowercaseUrls = true;
            });

            // https://stackoverflow.com/questions/38184583/how-to-add-ihttpcontextaccessor-in-the-startup-class-in-the-di-in-asp-net-core-1
            //services.AddHttpContextAccessor();
            
            // Repository Layer
            services.ConfigureRepoLayer();
            
            // Events Layer
            services.ConfigureEvents();
            services.Configure<SendgridOptions>(Configuration);
            
            // Service Layer
            services.ConfigureServiceLayer();

            // Hosted Service Layer
            services.ConfigureHostedServices();

            services.ConfigureNozomiAuth(Configuration);
            
            services.ConfigureSwagger();
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseNozomiAuth();
            app.UseCookiePolicy();
            
            // Setup the hot collections
            app.ConfigureStatics(HostingEnvironment);

            app.UseSignalR(route =>
            {
                route.MapHub<NozomiStreamHub>("/ticker");
                route.MapHub<NozomiSourceStreamHub>("/source");
            });

            app.UseSession();
            
            app.UseNozomiExceptionMiddleware();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                // https://stackoverflow.com/questions/39116047/how-to-change-base-url-of-swagger-in-asp-net-core
                c.RoutePrefix = "docs";
                c.SwaggerEndpoint("/swagger/" + GlobalApiVariables.CURRENT_API_VERSION + "/swagger.json", "Nozomi API");
            });
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=home}/{action=index}/{id?}");
                
                endpoints.MapControllerRoute(
                    name: "Areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}