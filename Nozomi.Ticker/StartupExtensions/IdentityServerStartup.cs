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
using Nozomi.Base.Identity;
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
                .AddClaimsPrincipalFactory<NozomiUserClaimsPrincipalFactory>()
                .AddDefaultTokenProviders();
            
            // Configure Authentication
            // https://github.com/aspnet/Security/issues/1414 Redundant to add in this
            //
            // Findings have shown this affects IdentityServerAuthenticationService. Removing this will 
            // default all constants to .NET Core Identity's defaults.
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = NozomiAuthConstants.ApplicationScheme;
                options.DefaultAuthenticateScheme = NozomiAuthConstants.ApplicationScheme;
                options.DefaultChallengeScheme = NozomiAuthConstants.ApplicationScheme;
                options.DefaultForbidScheme = NozomiAuthConstants.ApplicationScheme;
                options.DefaultSignInScheme = NozomiAuthConstants.ApplicationScheme;
                options.DefaultSignOutScheme = NozomiAuthConstants.ApplicationScheme;
            }).AddCookie(options =>
            {
                options.Cookie.Name = NozomiAuthConstants.ApplicationScheme;
                
                // Cookie settings
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                // TODO: finish this
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
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

            services.AddIdentityServer()
                .AddRequiredPlatformServices()
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
            app.UseIdentityServer();
        }
    }
}