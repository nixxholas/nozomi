// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;

namespace Nozomi.Auth
{
    public class IdentityConfig
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        
        public IdentityConfig(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        
        /// <summary>
        /// Identity resources are data like user ID, name, or email address of a user.
        /// An identity resource has a unique name, and you can assign arbitrary claim types to it.
        /// These claims will then be included in the identity token for the user.
        /// The client will use the scope parameter to request access to an identity resource.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            // Defining a custom identity resource
            // http://docs.identityserver.io/en/latest/topics/resources.html#defining-custom-identity-resources
            
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResource
                {
                    Name = "crypto_default",
                    DisplayName = "Default Wallet Address",
                    Description = "Allow the service access to your default wallet hash.",
                    UserClaims = new[] { NozomiJwtClaimTypes.DefaultWallet },
                    ShowInDiscoveryDocument = true,
                    Required = true,
                    Emphasize = true
                },
                new IdentityResource
                {
                    Name = "role",
                    DisplayName = "Roles",
                    Description = "Allow the service access to your user roles.",
                    UserClaims = new[] { JwtClaimTypes.Role, ClaimTypes.Role, "roles" },
                    ShowInDiscoveryDocument = true,
                    Required = true,
                    Emphasize = true
                },
                // new IdentityResource("roles", new[] { "role" })
            };
        }

        public IEnumerable<ApiResource> GetApis()
        {
            return new []
            {
                // Nozomi.Web
                new ApiResource()
                {
                    Name = "nozomi.web", 
                    DisplayName = "Nozomi Web API",
                    
                    // secret for using introspection endpoint
                    ApiSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // include the following using claims in access token (in addition to subject id)
                    UserClaims = { 
                        IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile, 
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        JwtClaimTypes.Role,
                        NozomiAuthConstants.StandardScopes.DefaultCryptoAddress },

                    // this API has limit to its scopes
                    Scopes =
                    {
                        new Scope()
                        {
                            Name = "nozomi.web",
                            DisplayName = "Standard access to Nozomi API",
                        },
                        new Scope()
                        {
                            Name = "nozomi.web.full_access",
                            DisplayName = "Full access to Nozomi API",
                        },
                        new Scope
                        {
                            Name = "nozomi.web.read_only",
                            DisplayName = "Read only access to Nozomi API"
                        }
                    }
                },
                // Nozomi.Auth
                new ApiResource()
                {
                    Name = "nozomi.auth", 
                    DisplayName = "Nozomi Auth API",
                    
                    // secret for using introspection endpoint
                    ApiSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // include the following using claims in access token (in addition to subject id)
                    UserClaims = { 
                        IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile, 
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        JwtClaimTypes.Role,
                        NozomiAuthConstants.StandardScopes.DefaultCryptoAddress },

                    // this API has limit to its scopes
                    Scopes =
                    {
                        new Scope()
                        {
                            Name = "nozomi.auth",
                            DisplayName = "Standard access to Nozomi Auth API",
                        },
                        new Scope()
                        {
                            Name = "nozomi.auth.full_access",
                            DisplayName = "Full access to Nozomi Auth API",
                        }
                    }
                }
            };
        }

        public IEnumerable<Client> GetClients()
        {
            if (_hostingEnvironment.IsProduction())
                return new[]
                {
                    new Client {
                        ClientId = "nozomi.web",
                        ClientName = "Nozomi",
                        
                        AllowAccessTokensViaBrowser = true,
                        AllowedGrantTypes = GrantTypes.Implicit,
                    
                        AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, 
                            IdentityServerConstants.StandardScopes.Profile, 
                            IdentityServerConstants.StandardScopes.Email,
                            IdentityServerConstants.StandardScopes.Phone,
                            JwtClaimTypes.Role, "nozomi.auth", "nozomi.web", "nozomi.web.read_only",
                            NozomiAuthConstants.StandardScopes.DefaultCryptoAddress },
                        RedirectUris = {"https://nozomi.one/oidc-callback", "https://nozomi.one/oidc-silent-renew" },
                        PostLogoutRedirectUris = {"https://nozomi.one/"},
                        AllowedCorsOrigins = {"https://nozomi.one"},
                        AccessTokenLifetime = 3600
                    }
                };
            
            return new[]
            {
                new Client {
                    ClientId = "nozomi.web",
                    ClientName = "Nozomi Vue SPA",
                    AlwaysIncludeUserClaimsInIdToken = true, // Always include user claims in the tokens.

                    AllowAccessTokensViaBrowser = true,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile, 
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        JwtClaimTypes.Role, "nozomi.auth", "nozomi.web", "nozomi.web.read_only",
                        NozomiAuthConstants.StandardScopes.DefaultCryptoAddress },
                    RedirectUris =
                    {
                        "https://localhost:5001/oidc-callback", "https://localhost:5001/oidc-silent-renew" 
                        ,"https://localhost:443/oidc-callback", "https://localhost:443/oidc-silent-renew"
                    },
                    PostLogoutRedirectUris = {"https://localhost:5001/"},
                    AllowedCorsOrigins = {"https://localhost:5001"},
                    AccessTokenLifetime = 3600
                }
            };
        }
            
        public static List<User> TestUsers = new List<User>()
        {
            new User
            {
                Id = Guid.NewGuid().ToString(), 
                UserName = "alice", 
                EmailConfirmed = true,
                PasswordHash = "alice", 
                UserClaims = new List<UserClaim>(){
                    new UserClaim
                    {
                        ClaimType = JwtClaimTypes.Name, 
                        ClaimValue = "Alice Smith" 
                    },
                    new UserClaim
                    {
                        ClaimType = JwtClaimTypes.GivenName, 
                        ClaimValue = "Alice"
                    },
                    new UserClaim
                    {
                        ClaimType = JwtClaimTypes.FamilyName, 
                        ClaimValue = "Smith"
                    },
                    new UserClaim
                    {
                        ClaimType = JwtClaimTypes.Email,
                        ClaimValue = "AliceSmith@email.com"
                    },
                    new UserClaim
                    {
                        ClaimType = JwtClaimTypes.WebSite, 
                        ClaimValue = "http://alice.com"
                    },
                    new UserClaim
                    {
                        ClaimType = JwtClaimTypes.Address, 
                        ClaimValue = 
                            @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }"
                    }
                }
            }
        };
    }
}