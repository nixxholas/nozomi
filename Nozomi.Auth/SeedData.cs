// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.BCL.Helpers.Crypto;
using Nozomi.Base.BCL.Helpers.Enumerator;
using Nozomi.Repo.Auth.Data;

namespace Nozomi.Auth
{
    public static class SeedData
    {
        public static void UseAutoDbMigration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var logger = serviceScope.ServiceProvider.GetService<ILogger<Startup>>();

                using (var context = serviceScope.ServiceProvider.GetService<AuthDbContext>())
                {
                    // Auto wipe
                    if (env.IsDevelopment())
                    {
                        context.Database.EnsureDeleted();
                    }

                    context.Database.Migrate();

                    var roleMgr = serviceScope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                    
                    var adminExists = roleMgr.RoleExistsAsync(RoleEnum.Administrator.GetDescription());
                    if (!adminExists.Result)
                    {
                        var adminRole = new Role
                        {
                            Name = RoleEnum.Administrator.GetDescription(),
                            NormalizedName = RoleEnum.Administrator.GetDescription().ToUpperInvariant()
                        };

                        var result = roleMgr.CreateAsync(adminRole).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                    }

                    var ownerExists = roleMgr.RoleExistsAsync(RoleEnum.Owner.GetDescription());
                    if (!ownerExists.Result)
                    {
                        var ownerRole = new Role
                        {
                            Name = RoleEnum.Owner.GetDescription(),
                            NormalizedName = RoleEnum.Owner.GetDescription().ToUpperInvariant()
                        };

                        var result = roleMgr.CreateAsync(ownerRole).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                    }
                    
                    var staffExists = roleMgr.RoleExistsAsync(RoleEnum.Staff.GetDescription());
                    if (!staffExists.Result)
                    {
                        var staffRole = new Role
                        {
                            Name = RoleEnum.Staff.GetDescription(),
                            NormalizedName = RoleEnum.Staff.GetDescription().ToUpperInvariant()
                        };

                        var result = roleMgr.CreateAsync(staffRole).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                    }
                    
                    var userRoleExists = roleMgr.RoleExistsAsync(RoleEnum.User.GetDescription());
                    if (!userRoleExists.Result)
                    {
                        var userRole = new Role
                        {
                            Name = RoleEnum.User.GetDescription(),
                            NormalizedName = RoleEnum.User.GetDescription().ToUpperInvariant()
                        };

                        var result = roleMgr.CreateAsync(userRole).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                    }
                    
                    var corpUserExists = roleMgr.RoleExistsAsync(RoleEnum.CorporateUser.GetDescription());
                    if (!corpUserExists.Result)
                    {
                        var corpUserRole = new Role
                        {
                            Name = RoleEnum.CorporateUser.GetDescription(),
                            NormalizedName = RoleEnum.CorporateUser.GetDescription().ToUpperInvariant()
                        };

                        var result = roleMgr.CreateAsync(corpUserRole).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                    }
                    
                    var userMgr = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    var alice = userMgr.FindByNameAsync("alice").Result;
                    if (alice == null)
                    {
                        alice = new User
                        {
                            UserName = "alice"
                        };
                        var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(alice, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                            new Claim(JwtClaimTypes.Address,
                                @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                                IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                            new Claim(NozomiJwtClaimTypes.StripeCustomerId, "cus_GiPXYNorAJsVIL"), 
                            new Claim(NozomiJwtClaimTypes.StripeCustomerDefaultPaymentId, "pm_1GAyifBrK0ZsVPPfIAwyWRdB"),
                            new Claim(NozomiJwtClaimTypes.StripeCustomerPaymentMethodId, "pm_1GAyifBrK0ZsVPPfIAwyWRdB"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        context.ApiKeys.Add(new ApiKey
                        {
                            Label = "First Dev Key",
                            Value = Randomizer.GenerateRandomCryptographicKey(32),
                            UserId = alice.Id
                        });
                        context.SaveChanges();

                        Console.WriteLine("alice created");

                        var ownerAddResult = userMgr.AddToRoleAsync(alice, RoleEnum.Owner.GetDescription()).Result;
                        if (!ownerAddResult.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        
                        Console.WriteLine("alice added to owners");
                    }
                    else
                    {
                        Console.WriteLine("alice already exists");
                    }

                    var stripeDummy = userMgr.FindByNameAsync("stripe").Result;
                    if (stripeDummy == null)
                    {
                        stripeDummy = new User
                        {
                            UserName = "stripe"
                        };
                        var result = userMgr.CreateAsync(stripeDummy, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(stripeDummy, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "Stripe Dummy"),
                            new Claim(JwtClaimTypes.GivenName, "Stripe"),
                            new Claim(JwtClaimTypes.FamilyName, "Dummy"),
                            new Claim(JwtClaimTypes.Email, "Stripe@stripe.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://stripe.com"),
                            new Claim(JwtClaimTypes.Address,
                                @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                                IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                            new Claim(NozomiJwtClaimTypes.StripeCustomerId, "cus_00000000000000"),
                            new Claim(NozomiJwtClaimTypes.StripeSubscriptionId, "sub_00000000000000"), 
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        context.ApiKeys.Add(new ApiKey
                        {
                            Label = "First Dev Key",
                            Value = Randomizer.GenerateRandomCryptographicKey(32),
                            UserId = stripeDummy.Id
                        });
                        context.SaveChanges();

                        Console.WriteLine("stripe created");

                        var ownerAddResult = userMgr.AddToRoleAsync(stripeDummy, RoleEnum.Owner.GetDescription()).Result;
                        if (!ownerAddResult.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        
                        Console.WriteLine("stripe added to owners");
                    }
                    else
                    {
                        Console.WriteLine("stripe already exists");
                    }
                    
                    var nicholas = userMgr.FindByNameAsync("nicholas").Result;
                    if (nicholas == null)
                    {
                        nicholas = new User
                        {
                            UserName = "nicholas"
                        };
                        var result = userMgr.CreateAsync(nicholas, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(nicholas, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "Nicholas Chen"),
                            new Claim(JwtClaimTypes.GivenName, "Nicholas"),
                            new Claim(JwtClaimTypes.FamilyName, "Chen"),
                            new Claim(JwtClaimTypes.Email, "nicholas@nozomi.one"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://nozomi.one"),
                            new Claim(JwtClaimTypes.Address,
                                @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                                IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                            new Claim(NozomiJwtClaimTypes.StripeCustomerId, "cus_GiBLTRABM7wu1N"),
                            new Claim(NozomiJwtClaimTypes.StripeCustomerDefaultPaymentId, "pm_1GAkzXBrK0ZsVPPf1tCLQwVR"),
                            new Claim(NozomiJwtClaimTypes.StripeCustomerPaymentMethodId, "pm_1GAkzXBrK0ZsVPPf1tCLQwVR"), 
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        context.ApiKeys.Add(new ApiKey
                        {
                            Label = "First Dev Key",
                            Value = Randomizer.GenerateRandomCryptographicKey(32),
                            UserId = nicholas.Id
                        });
                        context.SaveChanges();

                        Console.WriteLine("nicholas created");
                    }
                    else
                    {
                        Console.WriteLine("nicholas already exists");
                    }
                }
            }
        }
    }
}