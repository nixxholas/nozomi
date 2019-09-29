using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Repo.Identity.Data;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Web.StartupExtensions
{
    public static class StartupIdentity
    {
        public static void ConfigureNozomiAuth(this IServiceCollection services, string env)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:6001/";
                    options.RequireHttpsMetadata = true;
                    options.ApiName = "nozomi.spa";
                    options.ApiSecret = "super-secret";
                });

            // Turn off the JWT claim type mapping to allow well-known claims (e.g. ‘sub’ and ‘idp’) to flow through unmolested
//            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
//
//            if (!env.Equals("production", StringComparison.OrdinalIgnoreCase))
//                IdentityModelEventSource.ShowPII = true;
//
//            services.AddAuthentication(options =>
//                {
//                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//                    options.DefaultChallengeScheme = "oidc";
//                })
//                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
//                {
//                    options.SlidingExpiration = false;
//                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
//                })
//                .AddOpenIdConnect("oidc", options =>
//                {
//                    options.SignInScheme = "Cookies";
//
//                    if (env.Equals("production", StringComparison.OrdinalIgnoreCase))
//                    {
//                        options.Authority = "https://auth.nozomi.one";
//                        options.RequireHttpsMetadata = true;
//                    }
//                    else // Even if it is null, just do localhost..
//                    {
//                        options.Authority = "https://localhost:6001";
//                        options.RequireHttpsMetadata = true;
//                    }
//
//                    options.CallbackPath = "/auth-oidc";
//
//                    options.ClientId = "nozomi.vue";
//                    options.ClientSecret = "super-secret";
//                    options.ResponseType = "code id_token";
//
//                    options.SaveTokens = true;
//                    options.GetClaimsFromUserInfoEndpoint = true;
//
//                    options.Scope.Add("nozomi.web.read_only");
//                    options.Scope.Add("offline_access");
//                    //options.ClaimActions.MapJsonKey("website", "website");
//                });
        }
    }
}
