using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Nozomi.Base.Identity.Debugging;
using Nozomi.Base.Identity.Models;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Repo.Identity.Data;
using Nozomi.Service.Identity;
using Nozomi.Service.Identity.Factories;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Identity.Stores;
using Nozomi.Service.Identity.Stores.Interfaces;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class IdentityServerStartup
    {
        public static void ConfigureNozomiAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Environment-specific configuration
            if (!string.IsNullOrEmpty(env) && env.Equals("Production", StringComparison.InvariantCultureIgnoreCase))
            {
            } else
            {
            }

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<NozomiAuthContext>()
                .AddUserManager<NozomiUserManager>()
                .AddSignInManager<NozomiSignInManager>()
                .AddUserStore<NozomiUserStore>()
                .AddRoleStore<NozomiRoleStore>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, NozomiUserClaimsPrincipalFactory>();
            
            // Configure Authentication
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
                    options.DefaultScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
                    options.DefaultChallengeScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
                });
            
            // Configure Authorization
            services.AddAuthorization();
            
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
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/account/accessdenied";
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
        
        public static void UseNozomiAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication();
        }
    }
}