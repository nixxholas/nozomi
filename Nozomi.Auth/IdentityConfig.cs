// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Hosting;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;

namespace Nozomi.Auth
{
    public class IdentityConfig
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        
        public IdentityConfig(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        
        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            // Defining a custom identity resource
            // http://docs.identityserver.io/en/latest/topics/resources.html#defining-custom-identity-resources
            var walletAddressProfile = new IdentityResource(
                name: "nozomi.address",
                displayName: "Wallet address",
                claimTypes: new[] { "walletHash" });
                    
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                walletAddressProfile,
                new IdentityResource
                {
                    Name = "roles",
                    DisplayName = "Roles",
                    Description = "Allow the service access to your user roles.",
                    UserClaims = new[] { JwtClaimTypes.Role, ClaimTypes.Role },
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
                        JwtClaimTypes.Id, 
                        JwtClaimTypes.Name, 
                        JwtClaimTypes.Email, 
                        JwtClaimTypes.Role,
                        JwtClaimTypes.PhoneNumber,
                        ExtendedJwtClaimTypes.DefaultWallet },

                    // this API defines two scopes
                    Scopes =
                    {
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
                }
            };
        }

        public IEnumerable<Client> GetClients()
        {
            if (_hostingEnvironment.IsProduction())
                return new[]
                {
                    new Client {
                        ClientId = "nozomi.spa",
                        ClientName = "Nozomi",
                    
                        AllowAccessTokensViaBrowser = true,
                        AllowedGrantTypes = GrantTypes.Implicit,
                    
                        AllowedScopes = { "openid", "profile", "email", "roles", "nozomi.web.read_only" },
                        RedirectUris = {"https://nozomi.one/oidc-callback", "https://nozomi.one/oidc-silent-renew" },
                        PostLogoutRedirectUris = {"https://nozomi.one/"},
                        AllowedCorsOrigins = {"https://nozomi.one"},
                        AccessTokenLifetime = 3600
                    }
                };
            
            return new[]
            {
                new Client {
                    ClientId = "nozomi.spa",
                    ClientName = "Nozomi Vue SPA",
                    AlwaysIncludeUserClaimsInIdToken = true, // Always include user claims in the tokens.

                    AllowAccessTokensViaBrowser = true,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile, 
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        "roles", "nozomi.web.read_only" },
                    RedirectUris = {"https://localhost:5001/oidc-callback", "https://localhost:5001/oidc-silent-renew"},
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