using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Infra.Auth.Events.UserEvent;
using Nozomi.Infra.Auth.Services.QuotaClaims;
using Nozomi.Infra.Auth.Services.User;
using Nozomi.Infra.Payment.Events.Bootstripe;
using Nozomi.Infra.Payment.Services.Bootstripe;
using Nozomi.Infra.Payment.Services.DisputesHandling;
using Nozomi.Infra.Payment.Services.InvoicesHandling;
using Nozomi.Infra.Payment.Services.SubscriptionHandling;
using Nozomi.Preprocessing.Filters;
using Nozomi.Repo.Auth.Data;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Nozomi.Payment
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; }
        
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();

                // https://github.com/IdentityServer/IdentityServer4/issues/1331
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.RequireHeaderSymmetry = false;
                options.ForwardLimit = 2;
            });
            
            if (HostingEnvironment.IsDevelopment())
            {
                // Greet the beloved dev
                Console.WriteLine(@"Welcome to the dev environment, your machine is named: " + Environment.MachineName);

                // Postgres DB Setup
                var str = Configuration.GetConnectionString("Local:" + @Environment.MachineName);

                services
                    // .AddEntityFrameworkNpgsql()
                    .AddDbContext<AuthDbContext>(options =>
                        {
                            options.UseNpgsql(str);
                            options.EnableSensitiveDataLogging();
                            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                        },
                        ServiceLifetime.Transient);
            }
            else
            {
                services.AddHsts(opt =>
                {
                    opt.Preload = true;
                    opt.IncludeSubDomains = true;
                    opt.MaxAge = TimeSpan.FromDays(60);
                });
                
                var vaultUrl = Configuration["vaultUrl"];
                var vaultToken = Configuration["vaultToken"];

                if (string.IsNullOrEmpty(vaultToken))
                    throw new SystemException("Invalid vault token.");

                var authMethod = new TokenAuthMethodInfo(vaultToken);
                var vaultClientSettings = new VaultClientSettings(
                    !string.IsNullOrWhiteSpace(vaultUrl) ? vaultUrl : "https://blackbox.nozomi.one:8200", 
                    authMethod);
                var vaultClient = new VaultClient(vaultClientSettings);

                var nozomiVault = vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync("payments")
                    .GetAwaiter()
                    .GetResult().Data;

                // Always attempt to configure stripe first
                services.Configure<StripeOptions>(options =>
                {
                    if (string.IsNullOrEmpty((string) nozomiVault["stripe-product-id"]))
                        throw new ApplicationException("Invalid Stripe target Product Id!");
                    options.ProductId = (string) nozomiVault["stripe-product-id"];
                    if (string.IsNullOrEmpty((string) nozomiVault["stripe-default-plan-id"]))
                        throw new ApplicationException("Invalid Stripe default Plan Id!");
                    options.DefaultPlanId = (string) nozomiVault["stripe-default-plan-id"];
                    if (string.IsNullOrEmpty((string) nozomiVault["stripe-publishable-key"]))
                        throw new ApplicationException("Invalid Stripe Publishable Key!");
                    options.PublishableKey = (string) nozomiVault["stripe-publishable-key"];
                    if (string.IsNullOrEmpty((string) nozomiVault["stripe-secret-key"]))
                        throw new ApplicationException("Invalid Stripe Secret Key!");
                    options.SecretKey = (string) nozomiVault["stripe-secret-key"];
                    if (string.IsNullOrEmpty((string) nozomiVault["stripe-webhook-secret"]))
                        throw new ApplicationException("Invalid Stripe Webhook secret!");
                    options.WebhookSecret = (string) nozomiVault["stripe-webhook-secret"];
                });
                
                var mainDb = (string) nozomiVault["main"];
                if (string.IsNullOrEmpty(mainDb))
                    throw new SystemException("Invalid main database configuration");
                // Database
                services.AddDbContext<AuthDbContext>(options =>
                {
                    options.UseNpgsql(mainDb
                        , builder =>
                        {
                            builder.EnableRetryOnFailure();
                        }
                    );
                    options.EnableSensitiveDataLogging(false);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }, ServiceLifetime.Transient);
            }
            
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IBootstripeEvent, BootstripeEvent>();
            services.AddTransient<ICurrencyEvent, CurrencyEvent>();
            services.AddTransient<ICurrencyPairEvent, CurrencyPairEvent>();
            services.AddTransient<ICurrencyTypeEvent, CurrencyTypeEvent>();
            services.AddTransient<IRequestEvent, RequestEvent>();
            services.AddTransient<IUserEvent, UserEvent>();

            services.AddScoped<IBootstripeService, BootstripeService>();
            services.AddScoped<IComponentService, ComponentService>();
            services.AddScoped<IDisputesHandlingService, DisputesHandlingService>();
            services.AddScoped<IInvoicesHandlingService, InvoicesHandlingService>();
            services.AddScoped<IQuotaClaimsService, QuotaClaimsService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<ISubscriptionsHandlingService, SubscriptionsHandlingService>();
            services.AddScoped<IUserService, UserService>();

            services.AddControllers(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1&tabs=visual-studio#options
            // Calling AddHttpsRedirection is only necessary to change the values of HttpsPort or RedirectStatusCode.
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
                app.UseHsts();

                // ref: https://github.com/aspnet/Docs/issues/2384
                app.UseForwardedHeaders();
            }

            using (var scope = 
                app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = scope.ServiceProvider.GetService<AuthDbContext>())
                context.Database.Migrate();

            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                // MapControllers adds support for attribute-routed controllers.
                // MapAreaControllerRoute adds a conventional route for controllers
                // in an area.
                // MapControllerRoute adds a conventional route for controllers.
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });

            app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
        }
    }
}