using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Nozomi.Base.Auth.Global;

namespace Nozomi.Web2.Extensions
{
    public static class StartupIdentity
    {
        public static void ConfigureNozomiAuth(this IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var authority = Startup.Environment.IsProduction() ? "https://auth.nozomi.one" : "https://localhost:6001/";
            // IdentityModelEventSource.ShowPII =
            //     !Startup.Environment.IsProduction(); //To show detail of error and see the problem

            // https://stackoverflow.com/questions/46091301/roles-not-being-populated-by-addjwtbearer-using-identityserver4-and-dotnetcore-2#46094800
            services.Configure<IdentityOptions>(options =>
                options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddOpenIdConnect(
                    o =>
                    {
                        o.Authority = authority;
                        o.ClientId = "nozomi.web";
                        // o.ClientSecret = "secret";
                        o.RequireHttpsMetadata = false;
                        o.GetClaimsFromUserInfoEndpoint = true;
                        // https://stackoverflow.com/questions/46579376/identityserver4-how-to-access-user-email
                        o.TokenValidationParameters = new TokenValidationParameters
                        {
                            RoleClaimType = JwtClaimTypes.Role
                        };

                        // https://stackoverflow.com/questions/46579376/identityserver4-how-to-access-user-email
                        o.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
                        o.Scope.Add(IdentityServerConstants.StandardScopes.Profile);
                        o.Scope.Add(IdentityServerConstants.StandardScopes.Email);
                        o.Scope.Add(IdentityServerConstants.StandardScopes.Phone);
                        o.Scope.Add("roles");
                        o.Scope.Add("nozomi.web");
                        o.Scope.Add("nozomi.web.read_only");
                        o.Scope.Add(JwtClaimTypes.Role);
                        o.Scope.Add(NozomiAuthConstants.StandardScopes.DefaultCryptoAddress);
                    })
                .AddJwtBearer(o =>
                {
                    o.Authority = authority;
                    o.Audience = "nozomi.web";
                    o.RequireHttpsMetadata = false;
                    // https://stackoverflow.com/questions/46579376/identityserver4-how-to-access-user-email
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        RoleClaimType = JwtClaimTypes.Role,
                        // ValidAudiences = new List<string>
                        // {
                        //     "nozomi.spa",
                        //     "nozomi.web"
                        // },
                        // ValidateAudience = true
                    };
                    o.SaveToken = true;
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