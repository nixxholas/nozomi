using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Repo.Auth.Data;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Identity.Requirements;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class IdentityServerStartup
    {
        public static void ConfigureNozomiAuth(this IServiceCollection services, IConfiguration configuration)
        {
//            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
//
//            // Environment-specific configuration
//            if (!string.IsNullOrEmpty(env) && env.Equals("Production", StringComparison.InvariantCultureIgnoreCase))
//            {
//            } else
//            {
//            }

            services
                .AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();
            
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "NozomiCookie";
                options.Cookie.HttpOnly = false;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessDenied";
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });
            
            // Configure Authentication
            // https://github.com/aspnet/Security/issues/1414 Redundant to add in this
            //
            // Findings have shown this affects IdentityServerAuthenticationService. Removing this will 
            // default all constants to .NET Core Identity's defaults.
//            services.AddAuthentication().AddCookie(options =>
//            {
//                // Cookie settings
//                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
//
//                // TODO: finish this
//                options.LoginPath = "/account/login";
//                options.LogoutPath = "/account/logout";
//                options.AccessDeniedPath = "/account/accessdenied";
//                options.SlidingExpiration = true;
//            });
            
            // Configure Authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy(ApiTokenRequirement.ApiTokenRequirementName, policy 
                => policy.AddRequirements(new ApiTokenRequirement()));
            });
            
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
        }
        
        public static void UseNozomiAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication();
        }
    }
}