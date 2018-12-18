using System;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Base.Identity.Debugging;
using Nozomi.Base.Identity.Models;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Repo.Identity.Data;
using Nozomi.Service.Identity;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Identity.Stores;
using Nozomi.Service.Identity.Stores.Interfaces;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class IdentityServerStartup
    {
        public static void ConfigureIdentityServer(this IServiceCollection services)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Environment-specific configuration
            if (!string.IsNullOrEmpty(env) && env.Equals("Production", StringComparison.InvariantCultureIgnoreCase))
            {
            } else
            {
            }
            
            // Identity-related service injections
            services.AddTransient<INozomiUserStore, NozomiUserStore>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<NozomiAuthContext>()
                .AddUserManager<NozomiUserManager>()
                .AddSignInManager<NozomiSignInManager>()
                .AddUserStore<NozomiUserStore>()
                .AddRoleStore<NozomiRoleStore>()
                .AddDefaultTokenProviders();
            
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                // TODO: finish this
                //options.LoginPath = "/Identity/Account/Login";
                //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            
            services.AddIdentityServer()
                //.AddSigningCredential(cert)
                .AddResourceStore<ResourceStore>()
                .AddClientStore<ClientStore>()
                .AddAspNetIdentity<User>()
                .AddProfileService<NozomiProfileService>();
            
            services.AddTransient<IClientStore, ClientStore>();
            services.AddTransient<IResourceStore, ResourceStore>();
            
        }
    }
}