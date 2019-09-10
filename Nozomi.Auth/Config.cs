// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityModel;
using Nozomi.Base.Auth.Global;

namespace Nozomi.Auth
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
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
                walletAddressProfile
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource()
                {
                    Name = "nozomiapi", 
                    DisplayName = "Nozomi Web API",
                    
                    // secret for using introspection endpoint
                    ApiSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // include the following using claims in access token (in addition to subject id)
                    UserClaims = { JwtClaimTypes.Name, JwtClaimTypes.Email, ExtendedJwtClaimTypes.DefaultWallet },

                    // this API defines two scopes
                    Scopes =
                    {
                        new Scope()
                        {
                            Name = "nozomiapi.full_access",
                            DisplayName = "Full access to Nozomi API",
                        },
                        new Scope
                        {
                            Name = "nozomiapi.read_only",
                            DisplayName = "Read only access to Nozomi API"
                        }
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256())},

                    AllowedScopes = {"api1"}
                },

                // MVC client using hybrid flow
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = {new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256())},

                    RedirectUris = {"http://localhost:5001/signin-oidc"},
                    FrontChannelLogoutUri = "http://localhost:5001/signout-oidc",
                    PostLogoutRedirectUris = {"http://localhost:5001/signout-callback-oidc"},

                    AllowOfflineAccess = true,
                    AllowedScopes = {"openid", "profile", "api1"}
                },

                // SPA client using code flow + pkce
                new Client
                {
                    ClientId = "spa",
                    ClientName = "SPA Client",
                    ClientUri = "http://identityserver.io",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =
                    {
                        "http://localhost:5002/index.html",
                        "http://localhost:5002/callback.html",
                        "http://localhost:5002/silent.html",
                        "http://localhost:5002/popup.html",
                    },

                    PostLogoutRedirectUris = {"http://localhost:5002/index.html"},
                    AllowedCorsOrigins = {"http://localhost:5002"},

                    AllowedScopes = {"openid", "profile", "api1"}
                }
            };
        }
    }
}