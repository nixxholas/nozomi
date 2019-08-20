using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Repo.Data;
using Nozomi.Repo.Identity.Data;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Identity.Services;
using Nozomi.Service.Identity.Services.Interfaces;

namespace Nozomi.Analysis.StartupExtensions
{
    public static class StartupDatabase
    {
        public static void UseAutoDbMigration(this IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
//                if (env.IsProduction())
//                {
                    var stripeService = serviceScope.ServiceProvider.GetService<IStripeService>();
                    stripeService.ConfigureStripePlans();
//                }
                
                var logger = serviceScope.ServiceProvider.GetService<ILogger<Startup>>();
                
                using (var context = serviceScope.ServiceProvider.GetService<NozomiAuthContext>())
                {
                    if (context.Database.GetPendingMigrations().Any())
                        context.Database.Migrate();
                    
                    using (var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>())
                    {
                        // Iterating Enumerator values.
                        // https://stackoverflow.com/questions/972307/how-to-loop-through-all-enum-values-in-c
                        var roles = Enum.GetValues(typeof(RoleEnum)).Cast<RoleEnum>();

                        foreach (var role in roles)
                        {
                            var roleStr = role.GetDescription();

                            if (roleManager.FindByNameAsync(roleStr).Result == null)
                            {
                                var newRole = new Role
                                {
                                    Name = roleStr
                                };

                                var res = roleManager.CreateAsync(newRole).Result;

                                if (!res.Succeeded)
                                {
                                    logger.LogCritical($"Error seeding role {newRole.Name}.");
                                }
                            }
                        }
                    }

                    // Seed users
                    using (var userManager = serviceScope.ServiceProvider.GetService<NozomiUserManager>())
                    {
                        // Seed big brother
                        if (userManager.FindByEmailAsync("nixholas@outlook.com").Result == null)
                        {
                            var boss = new User
                            {
                                UserName = "nixholas",
                                NormalizedUserName = "NIXHOLAS",
                                NormalizedEmail = "NIXHOLAS@OUTLOOK.COM",
                                Email = "nixholas@outlook.com",
                                StripeCustomerId = "cus_ELCsKKBzzjNc2I",
                                EmailConfirmed = true
                            };

                            var res = userManager.CreateAsync(boss, "P@ssw0rd").Result;

                            if (!res.Succeeded)
                            {
                                logger.LogCritical($"Error seeding da boss!!!");
                            }

                            var roleRes = userManager.AddToRoleAsync(boss, RoleEnum.Owner.GetDescription()).Result;

                            if (!roleRes.Succeeded)
                            {
                                logger.LogCritical($"Error seeding da boss role!!!");
                            }
                        }

                        if (userManager.FindByEmailAsync("nicholas@counter.network").Result == null)
                        {
                            var boss = new User
                            {
                                UserName = "nicholas",
                                NormalizedUserName = "NICHOLAS",
                                NormalizedEmail = "NICHOLAS@NOZOMI.ONE",
                                Email = "nicholas@nozomi.one",
                                StripeCustomerId = "cus_ELCsKKBzzjNc2I",
                                EmailConfirmed = true
                            };

                            var res = userManager.CreateAsync(boss, "P@ssw0rd").Result;

                            if (!res.Succeeded)
                            {
                                logger.LogCritical($"Error seeding da boss!!!");
                            }

                            var roleRes = userManager.AddToRoleAsync(boss, RoleEnum.Owner.GetDescription()).Result;

                            if (!roleRes.Succeeded)
                            {
                                logger.LogCritical($"Error seeding da boss role!!!");
                            }
                        }
                    }
                }

                using (var context = serviceScope.ServiceProvider.GetService<NozomiDbContext>())
                {
                    if (env.IsProduction()) {
                        if (context.Database.GetPendingMigrations().Any())
                        {
                            context.Database.Migrate();
                        }
                    }

                    try
                    {
                        if (!context.CurrencyTypes.Any())
                        {
                            context.CurrencyTypes.AddRange(
                                new CurrencyType()
                                {
                                    TypeShortForm = "FIAT",
                                    Name = "FIAT Cash"
                                },
                                new CurrencyType()
                                {
                                    TypeShortForm = "CRYPTO",
                                    Name = "Cryptocurrency",
                                    AnalysedComponents = new List<AnalysedComponent>()
                                    {
                                        // Crypto Market Cap
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.MarketCap,
                                            Delay = 3000,
                                            UIFormatting = "$ 0 a",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            DeletedAt = null
                                        }
                                    }
                                });
                            
                            context.SaveChanges();
                        }

                        if (!context.Currencies.Any() && context.CurrencyTypes.Any())
                        {
                            context.Currencies.AddRange(
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("FIAT",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Abbreviation = "USD",
                                    Slug = "USD",
                                    Name = "United States Dollar"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("FIAT",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Abbreviation = "EUR",
                                    Slug = "EUR",
                                    Name = "Euro"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Abbreviation = "ETH",
                                    Slug = "ETH",
                                    Name = "Ethereum",
                                    Denominations = 18,
                                    DenominationName = "Wei",
                                    AnalysedComponents = new List<AnalysedComponent>()
                                    {
                                        // ============================ START OF ETHEREUM ============================ //
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.MarketCap,
                                            Delay = 3000,
                                            UIFormatting = "$ 0 a",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.MarketCapChange,
                                            Delay = 3000,
                                            UIFormatting = "$ 0 a",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.MarketCapPctChange,
                                            Delay = 3000,
                                            UIFormatting = "$ 0 a",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.MarketCapHourlyPctChange,
                                            Delay = 3000,
                                            UIFormatting = "$ 0 a",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        // ETH Current Average Price
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                            Delay = 3000,
                                            UIFormatting = "$ 0[.]00",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        // ETH Hourly Average Price
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                            Delay = 3000,
                                            UIFormatting = "$ 0[.]00",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        // ============================ END OF ETHEREUM ============================ //
                                    },
                                    Requests = new List<Request>()
                                    {
                                        // ETH Etherscan Request
                                        new Request
                                        {
                                            Guid = Guid.Parse("d13fc276-8077-49d2-ba38-998c58895df9"),
                                            RequestType = RequestType.HttpGet,
                                            DataPath = "https://api.etherscan.io/api",
                                            Delay = 5000,
                                            RequestProperties = new List<RequestProperty>()
                                            {
                                                // ETH BFX Etherscan Request Property
                                                new RequestProperty
                                                {
                                                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                                                    Key = "module",
                                                    Value = "stats",
                                                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    ModifiedAt =
                                                        new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    DeletedAt = null
                                                },
                                                // ETH BFX Etherscan Request Property
                                                new RequestProperty
                                                {
                                                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                                                    Key = "action",
                                                    Value = "ethsupply",
                                                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    ModifiedAt =
                                                        new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    DeletedAt = null
                                                },
                                                new RequestProperty
                                                {
                                                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                                                    Key = "apikey",
                                                    Value = "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224",
                                                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    ModifiedAt =
                                                        new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    DeletedAt = null
                                                }
                                            },
                                            RequestComponents = new List<RequestComponent>()
                                            {
                                                // ETH BFX Etherscan Request for Circulating Supply
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.CirculatingSupply,
                                                    Identifier = null,
                                                    IsDenominated = true,
                                                    QueryComponent = "result",
                                                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    ModifiedAt =
                                                        new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    DeletedAt = null
                                                }
                                            }
                                        }
                                    }
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Abbreviation = "KNC",
                                    Slug = "KNC",
                                    Name = "Kyber Network Crystal",
                                    Denominations = 18,
                                    AnalysedComponents = new List<AnalysedComponent>()
                                    {
                                        // ============================ START OF KYBER ============================ //
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.MarketCap,
                                            Delay = 3000,
                                            UIFormatting = "$ 0 a",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.MarketCapChange,
                                            Delay = 3000,
                                            UIFormatting = "$ 0 a",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.MarketCapPctChange,
                                            Delay = 3000,
                                            UIFormatting = "$ 0 a",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.MarketCapHourlyPctChange,
                                            Delay = 3000,
                                            UIFormatting = "$ 0 a",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        // KNC Current Average Price
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                            Delay = 3000,
                                            UIFormatting = "$ 0[.]00",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        // KNC Hourly Average Price
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                            Delay = 3000,
                                            UIFormatting = "$ 0[.]00",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        // ============================ END OF KYBER ============================ //
                                    },
                                    Requests = new List<Request>()
                                    {
                                        // KNC Etherscan Request
                                        new Request
                                        {
                                            Guid = Guid.Parse("b7b9642e-357a-451c-9741-bf5a7fcb0ad1"),
                                            RequestType = RequestType.HttpGet,
                                            DataPath = "https://api.etherscan.io/api",
                                            Delay = 5000,
                                            RequestProperties = new List<RequestProperty>()
                                            {
                                                new RequestProperty
                                                {
                                                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                                                    Key = "module",
                                                    Value = "stats",
                                                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    ModifiedAt =
                                                        new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    DeletedAt = null
                                                },
                                                new RequestProperty
                                                {
                                                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                                                    Key = "action",
                                                    Value = "tokensupply",
                                                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    ModifiedAt =
                                                        new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    DeletedAt = null
                                                },
                                                new RequestProperty
                                                {
                                                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                                                    Key = "contractaddress",
                                                    Value = "0xdd974d5c2e2928dea5f71b9825b8b646686bd200",
                                                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    ModifiedAt =
                                                        new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    DeletedAt = null
                                                },
                                                new RequestProperty
                                                {
                                                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                                                    Key = "apikey",
                                                    Value = "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224",
                                                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    ModifiedAt =
                                                        new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    DeletedAt = null
                                                }
                                            },
                                            RequestComponents = new List<RequestComponent>()
                                            {
                                                // KNC BFX Etherscan Request for Circulating Supply
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.CirculatingSupply,
                                                    Identifier = null,
                                                    IsDenominated = true,
                                                    QueryComponent = "result",
                                                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    ModifiedAt =
                                                        new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    DeletedAt = null,
                                                }
                                            }
                                        }
                                    }
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Abbreviation = "BTC",
                                    Slug = "BTC",
                                    Name = "Bitcoin",
                                    Denominations = 8,
                                    DenominationName = "Sat",
                                    AnalysedComponents = new List<AnalysedComponent>()
                                    {
                                        // ============================ START OF BITCOIN ============================ //
                                        // BTC Market Cap
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.MarketCap,
                                            Delay = 3000,
                                            UIFormatting = "$ 0 a",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            DeletedAt = null
                                        },
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.MarketCapChange,
                                            Delay = 3000,
                                            UIFormatting = "$ 0 a",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.MarketCapPctChange,
                                            Delay = 3000,
                                            UIFormatting = "$ 0 a",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.MarketCapHourlyPctChange,
                                            Delay = 3000,
                                            UIFormatting = "$ 0 a",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        // KNC Current Average Price
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                            Delay = 3000,
                                            UIFormatting = "$ 0[.]00",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        // KNC Hourly Average Price
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                            Delay = 3000,
                                            UIFormatting = "$ 0[.]00",
                                            CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                            IsDenominated = true,
                                            DeletedAt = null
                                        },
                                        // ============================ END OF BITCOIN ============================ //
                                    },
                                    Requests = new List<Request>()
                                    {
                                        // BTC Bitpay Insight Request
                                        new Request
                                        {
                                            Guid = Guid.Parse("31ceeb18-1d89-43d2-b215-0488d9417c67"),
                                            RequestType = RequestType.HttpGet,
                                            DataPath = "https://insight.bitpay.com/api/status?q=getBlockCount",
                                            Delay = 90000,
                                            RequestComponents = new List<RequestComponent>()
                                            {
                                                // POLO BTC Bitpay Insight for Blockcount
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.BlockCount,
                                                    Identifier = null,
                                                    QueryComponent = "info/blocks",
                                                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    ModifiedAt =
                                                        new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    DeletedAt = null
                                                },
                                                // POLO BTC Bitpay Insight for network difficulty
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.Difficulty,
                                                    Identifier = null,
                                                    QueryComponent = "info/difficulty",
                                                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    ModifiedAt =
                                                        new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    DeletedAt = null
                                                },
                                            }
                                        },
                                        // BTC Coinranking Request
                                        new Request
                                        {
                                            Guid = Guid.Parse("7f10715f-b5cc-4e52-9fa8-011311a5a2ca"),
                                            RequestType = RequestType.HttpGet,
                                            DataPath = "https://api.coinranking.com/v1/public/coin/1?base=USD",
                                            Delay = 90000,
                                            RequestComponents = new List<RequestComponent>()
                                            {
                                                // BTC Coinranking for circulating supply
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.CirculatingSupply,
                                                    Identifier = null,
                                                    QueryComponent = "data/coin/circulatingSupply",
                                                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    ModifiedAt =
                                                        new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                    DeletedAt = null
                                                }
                                            }
                                        }
                                    }
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Abbreviation = "BCN",
                                    Slug = "BCN",
                                    Name = "Bytecoin"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Abbreviation = "BTS",
                                    Slug = "BTS",
                                    Name = "BitShares"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Abbreviation = "USDT",
                                    Slug = "USDT",
                                    Name = "Tether"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("FIAT",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Abbreviation = "SGD",
                                    Slug = "SGD",
                                    Name = "Singapore Dollar"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Abbreviation = "LTC",
                                    Slug = "LTC",
                                    Name = "Litecoin"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "XRP",
                                    Abbreviation = "XRP",
                                    Slug = "XRP"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Bitcoin Cash",
                                    Abbreviation = "BCH",
                                    Slug = "BCH"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "EOS",
                                    Abbreviation = "EOS",
                                    Slug = "EOS"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Binance Coin",
                                    Abbreviation = "BNB",
                                    Slug = "BNB"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Stellar",
                                    Abbreviation = "XLM",
                                    Slug = "XLM"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Cardano",
                                    Abbreviation = "ADA",
                                    Slug = "ADA"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "TRON",
                                    Abbreviation = "TRX",
                                    Slug = "TRX"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Bitcoin SV",
                                    Abbreviation = "BSV",
                                    Slug = "BSV"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Monero",
                                    Abbreviation = "XMR",
                                    Slug = "XMR"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Dash",
                                    Abbreviation = "DASH",
                                    Slug = "DASH"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "IOTA",
                                    Abbreviation = "MIOTA",
                                    Slug = "MIOTA"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Tezos",
                                    Abbreviation = "XTZ",
                                    Slug = "XTZ"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Cosmos",
                                    Abbreviation = "ATOM",
                                    Slug = "ATOM"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Ethereum Classic",
                                    Abbreviation = "ETC",
                                    Slug = "ETC"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "NEM",
                                    Abbreviation = "XEM",
                                    Slug = "XEM"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "NEO",
                                    Abbreviation = "NEO",
                                    Slug = "NEO"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Maker",
                                    Abbreviation = "MKR",
                                    Slug = "MKR"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Ontology",
                                    Abbreviation = "ONT",
                                    Slug = "ONT"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Zcash",
                                    Abbreviation = "ZEC",
                                    Slug = "ZEC"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Basic Attention Token",
                                    Abbreviation = "BAT",
                                    Slug = "BAT"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Crypto.com Chain",
                                    Abbreviation = "CRO",
                                    Slug = "CRO"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Bitcoin Gold",
                                    Abbreviation = "BTG",
                                    Slug = "BTG"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "VeChain",
                                    Abbreviation = "VET",
                                    Slug = "VET"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Chainlink",
                                    Abbreviation = "LINK",
                                    Slug = "LINK"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "USD Coin",
                                    Abbreviation = "USDC",
                                    Slug = "USDC"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Dogecoin",
                                    Abbreviation = "DOGE",
                                    Slug = "DOGE"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Aurora",
                                    Abbreviation = "AOA",
                                    Slug = "AOA"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "OmiseGO",
                                    Abbreviation = "OMG",
                                    Slug = "OMG"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Qtum",
                                    Abbreviation = "QTUM",
                                    Slug = "QTUM"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Decred",
                                    Abbreviation = "DCR",
                                    Slug = "DCR"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Waves",
                                    Abbreviation = "WAVES",
                                    Slug = "WAVES"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "BitTorrent",
                                    Abbreviation = "BTT",
                                    Slug = "BTT"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Holo",
                                    Abbreviation = "HOT",
                                    Slug = "HOT"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "TrueUSD",
                                    Abbreviation = "TUSD",
                                    Slug = "TUSD"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Lisk",
                                    Abbreviation = "LSK",
                                    Slug = "LSK"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Nano",
                                    Abbreviation = "NANO",
                                    Slug = "NANO"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Augur",
                                    Abbreviation = "REP",
                                    Slug = "REP"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Bitcoin Diamond",
                                    Abbreviation = "BCD",
                                    Slug = "BCD"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "0x",
                                    Abbreviation = "ZRX",
                                    Slug = "ZRX"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Ravencoin",
                                    Abbreviation = "RVN",
                                    Slug = "RVN"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "DigiByte",
                                    Abbreviation = "DGB",
                                    Slug = "DGB"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "ICON",
                                    Abbreviation = "ICX",
                                    Slug = "ICX"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Verge",
                                    Abbreviation = "XVG",
                                    Slug = "XVG"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Paxos Standard Token",
                                    Abbreviation = "PAX",
                                    Slug = "PAX"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Pundi X",
                                    Abbreviation = "NPXS",
                                    Slug = "NPXS"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Zilliqa",
                                    Abbreviation = "ZIL",
                                    Slug = "ZIL"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Huobi Token",
                                    Abbreviation = "HT",
                                    Slug = "HT"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Aeternity",
                                    Abbreviation = "AE",
                                    Slug = "AE"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "IOST",
                                    Abbreviation = "IOST",
                                    Slug = "IOST"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Siacoin",
                                    Abbreviation = "SC",
                                    Slug = "SC"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "ABBC Coin",
                                    Abbreviation = "ABBC",
                                    Slug = "ABBC"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Komodo",
                                    Abbreviation = "KMD",
                                    Slug = "KMD"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Enjin Coin",
                                    Abbreviation = "ENJ",
                                    Slug = "ENJ"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Steem",
                                    Abbreviation = "STEEM",
                                    Slug = "STEEM"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Bytom",
                                    Abbreviation = "BTM",
                                    Slug = "BTM"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Qubitica",
                                    Abbreviation = "QBIT",
                                    Slug = "QBIT"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "THETA",
                                    Abbreviation = "THETA",
                                    Slug = "THETA"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Stratis",
                                    Abbreviation = "STRAT",
                                    Slug = "STRAT"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "ThoreCoin",
                                    Abbreviation = "THR",
                                    Slug = "THR"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "MaidSafeCoin",
                                    Abbreviation = "MAID",
                                    Slug = "MAID"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Dent",
                                    Abbreviation = "DENT",
                                    Slug = "DENT"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Insight Chain",
                                    Abbreviation = "INB",
                                    Slug = "INB"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "KuCoin Shares",
                                    Abbreviation = "KCS",
                                    Slug = "KCS"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "SOLVE",
                                    Abbreviation = "SOLVE",
                                    Slug = "SOLVE"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Waltonchain",
                                    Abbreviation = "WTC",
                                    Slug = "WTC"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Golem",
                                    Abbreviation = "GNT",
                                    Slug = "GNT"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "aelf",
                                    Abbreviation = "ELF",
                                    Slug = "ELF"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Status",
                                    Abbreviation = "SNT",
                                    Slug = "SNT"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Crypto.com",
                                    Abbreviation = "MCO",
                                    Slug = "MCO"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Mixin",
                                    Abbreviation = "XIN",
                                    Slug = "XIN"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Cryptonex",
                                    Abbreviation = "CNX",
                                    Slug = "CNX"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Theta Fuel",
                                    Abbreviation = "TFUEL",
                                    Slug = "TFUEL"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Ardor",
                                    Abbreviation = "ARDR",
                                    Slug = "ARDR"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "VestChain",
                                    Abbreviation = "VEST",
                                    Slug = "VEST"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Dai",
                                    Abbreviation = "DAI",
                                    Slug = "DAI"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Factom",
                                    Abbreviation = "FCT",
                                    Slug = "FCT"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Project Pai",
                                    Abbreviation = "PAI",
                                    Slug = "PAI"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "WAX",
                                    Abbreviation = "WAX",
                                    Slug = "WAX"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "TrueChain",
                                    Abbreviation = "TRUE",
                                    Slug = "TRUE"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Ark",
                                    Abbreviation = "ARK",
                                    Slug = "ARK"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Horizen",
                                    Abbreviation = "ZEN",
                                    Slug = "ZEN"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "DigixDAO",
                                    Abbreviation = "DGD",
                                    Slug = "DGD"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Clams",
                                    Abbreviation = "CLAM",
                                    Slug = "CLAM"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "MonaCoin",
                                    Abbreviation = "MONA",
                                    Slug = "MONA"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "GXChain",
                                    Abbreviation = "GXC",
                                    Slug = "GXC"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Decentraland",
                                    Abbreviation = "MANA",
                                    Slug = "MANA"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Orbs",
                                    Abbreviation = "ORBS",
                                    Slug = "ORBS"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Aion",
                                    Abbreviation = "AION",
                                    Slug = "AION"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Metaverse ETP",
                                    Abbreviation = "ETP",
                                    Slug = "ETP"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Elastos",
                                    Abbreviation = "ELA",
                                    Slug = "ELA"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Loopring",
                                    Abbreviation = "LRC",
                                    Slug = "LRC"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Loom Network",
                                    Abbreviation = "LOOM",
                                    Slug = "LOOM"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Santiment Network Token",
                                    Abbreviation = "SAN",
                                    Slug = "SAN"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Revain",
                                    Abbreviation = "R",
                                    Slug = "R"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "NULS",
                                    Abbreviation = "NULS",
                                    Slug = "NULS"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Populous",
                                    Abbreviation = "PPT",
                                    Slug = "PPT"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Matic Network",
                                    Abbreviation = "MATIC",
                                    Slug = "MATIC"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "LATOKEN",
                                    Abbreviation = "LA",
                                    Slug = "LA"
                                },
                                new Currency
                                {
                                    CurrencyTypeId = context.CurrencyTypes
                                        .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                            StringComparison.InvariantCultureIgnoreCase))
                                        .Select(ct => ct.Id)
                                        .SingleOrDefault(),
                                    Name = "Zcoin",
                                    Abbreviation = "XZC",
                                    Slug = "XZC"
                                });
                            
                            context.SaveChanges();
                        }

                        if (!context.Sources.Any())
                        {
                            context.Sources.AddRange(
                                new Source()
                                {
                                    Abbreviation = "BFX",
                                    Name = "Bitfinex",
                                    APIDocsURL = "https://docs.bitfinex.com/docs/introduction"
                                },
                                new Source()
                                {
                                    Abbreviation = "HAKO",
                                    Name = "Coinhako",
                                    APIDocsURL = "None"
                                },
                                new Source()
                                {
                                    Abbreviation = "BNA",
                                    Name = "Binance",
                                    APIDocsURL =
                                        "https://github.com/binance-exchange/binance-official-api-docs/blob/master/rest-api.md"
                                },
                                new Source()
                                {
                                    Abbreviation = "ECB",
                                    Name = "European Central Bank",
                                    APIDocsURL =
                                        "https://www.ecb.europa.eu/stats/policy_and_exchange_rates/euro_reference_exchange_rates/html/index.en.html"
                                },
                                new Source()
                                {
                                    Abbreviation = "AVG",
                                    Name = "AlphaVantage",
                                    APIDocsURL = "https://www.alphavantage.co/documentation/"
                                },
                                new Source
                                {
                                    Abbreviation = "POLO",
                                    Name = "Poloniex",
                                    APIDocsURL = "https://docs.poloniex.com/#public-http-api-methods"
                                });
                            
                            context.SaveChanges();
                        }

                        if (context.Sources.Any())
                        {
                            var bfx = context.Sources
                                .SingleOrDefault(s =>
                                    s.Abbreviation.Equals("BFX", StringComparison.InvariantCultureIgnoreCase));
                            var hako = context.Sources
                                .SingleOrDefault(s =>
                                    s.Abbreviation.Equals("HAKO", StringComparison.InvariantCultureIgnoreCase));
                            var bna = context.Sources
                                .SingleOrDefault(s =>
                                    s.Abbreviation.Equals("BNA", StringComparison.InvariantCultureIgnoreCase));
                            var ecb = context.Sources
                                .SingleOrDefault(s =>
                                    s.Abbreviation.Equals("ECB", StringComparison.InvariantCultureIgnoreCase));
                            var avg = context.Sources
                                .SingleOrDefault(s =>
                                    s.Abbreviation.Equals("AVG", StringComparison.InvariantCultureIgnoreCase));
                            var polo = context.Sources
                                .SingleOrDefault(s =>
                                    s.Abbreviation.Equals("POLO", StringComparison.InvariantCultureIgnoreCase));

                            if (!context.CurrencyPairs.Any())
                            {
                                Debug.Assert(bfx != null, nameof(bfx) + " != null");

                                context.CurrencyPairs.AddRange(
                                    new CurrencyPair()
                                    {
                                        CurrencyPairType = CurrencyPairType.TRADEABLE,
                                        APIUrl = "https://api.ethfinex.com/v2/ticker/tETHUSD",
                                        DefaultComponent = "0",
                                        SourceId = bfx.Id,
                                        MainCurrencyAbbrv = "ETH",
                                        CounterCurrencyAbbrv = "USD",
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Calculates volume ONLY for this exact Currency pair on this exchange.
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.DailyVolume,
                                                Delay = 1000,
                                                UIFormatting = "$ 0 a",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 500,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 500,
                                                UIFormatting = "0[.]0",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            }  
                                        },
                                        Requests = new List<Request>()
                                        {
                                            // BFX ETHUSD
                                            new Request()
                                            {
                                                Guid = Guid.Parse("096e9def-1c0f-4d1c-aa7b-273499f2cbda"),
                                                RequestType = RequestType.HttpGet,
                                                DataPath = "https://api-pub.bitfinex.com/v2/tickers?symbols=ALL",
                                                Delay = 5000,
                                                RequestComponents = new List<RequestComponent>()
                                                {
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.DailyVolume,
                                                        Identifier = "0=>tETHUSD",
                                                        QueryComponent = "8",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Ask,
                                                        Identifier = "0=>tETHUSD",
                                                        QueryComponent = "1",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent()
                                                    {
                                                        ComponentType = ComponentType.Bid,
                                                        Identifier = "0=>tETHUSD",
                                                        QueryComponent = "3",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.TRADEABLE,
                                        APIUrl = "https://api.ethfinex.com/v2/ticker/tKNCUSD",
                                        DefaultComponent = "0",
                                        SourceId = bfx.Id,
                                        MainCurrencyAbbrv = "KNC",
                                        CounterCurrencyAbbrv = "USD",
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Calculates volume ONLY for this exact Currency pair on this exchange.
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.DailyVolume,
                                                Delay = 1000,
                                                UIFormatting = "0 a",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 500,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 500,
                                                UIFormatting = "0[.]0",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            }
                                        },
                                        Requests = new List<Request>()
                                        {
                                            // BFX KNCUSD
                                            new Request()
                                            {
                                                Guid = Guid.Parse("534ccff8-b6ff-4cce-961b-8458ef0ca5af"),
                                                RequestType = RequestType.HttpGet,
                                                DataPath = "https://api-pub.bitfinex.com/v2/tickers?symbols=ALL",
                                                Delay = 5000,
                                                RequestComponents = new List<RequestComponent>()
                                                {
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.DailyVolume,
                                                        Identifier = "0=>tKNCUSD",
                                                        QueryComponent = "8",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Ask,
                                                        Identifier = "0=>tKNCUSD",
                                                        QueryComponent = "1",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent()
                                                    {
                                                        ComponentType = ComponentType.Bid,
                                                        Identifier = "0=>tKNCUSD",
                                                        QueryComponent = "3",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new CurrencyPair()
                                    {
                                        CurrencyPairType = CurrencyPairType.TRADEABLE,
                                        APIUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
                                        DefaultComponent = "Cube",
                                        SourceId = ecb.Id,
                                        MainCurrencyAbbrv = "EUR",
                                        CounterCurrencyAbbrv = "USD",
                                        Requests = new List<Request>()
                                        {
                                            // ECB EURUSD
                                            new Request()
                                            {
                                                Guid = Guid.Parse("1d8ba5ea-9d3a-4b02-b2d8-84ccd0851e69"),
                                                RequestType = RequestType.HttpGet,
                                                ResponseType = ResponseType.XML,
                                                DataPath =
                                                    "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
                                                Delay = 86400000,
                                                RequestComponents = new List<RequestComponent>()
                                                {
                                                    new RequestComponent()
                                                    {
                                                        ComponentType = ComponentType.Ask,
                                                        Identifier = null,
                                                        QueryComponent = "gesmes:Envelope/Cube/Cube/Cube/0=>@rate",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.TRADEABLE,
                                        APIUrl = "https://www.alphavantage.co/query",
                                        DefaultComponent = "Realtime Currency Exchange Rate/5. Exchange Rate",
                                        SourceId = avg.Id,
                                        MainCurrencyAbbrv = "EUR",
                                        CounterCurrencyAbbrv = "USD",
                                        Requests = new List<Request>()
                                        {
                                            // AlphaVantage EURUSD
                                            new Request()
                                            {
                                                Guid = Guid.Parse("48ad7cb2-b2b7-41be-8540-64136b72883c"),
                                                RequestType = RequestType.HttpGet,
                                                ResponseType = ResponseType.Json,
                                                DataPath = "https://www.alphavantage.co/query",
                                                Delay = 5000,
                                                RequestComponents = new List<RequestComponent>()
                                                {
                                                    new RequestComponent()
                                                    {
                                                        ComponentType = ComponentType.Ask,
                                                        Identifier = null,
                                                        QueryComponent =
                                                            "['Realtime Currency Exchange Rate']/['5. Exchange Rate']",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    }
                                                },
                                                RequestProperties = new List<RequestProperty>()
                                                {
                                                    new RequestProperty()
                                                    {
                                                        RequestPropertyType = RequestPropertyType.HttpQuery,
                                                        Key = "apikey",
                                                        Value = "TV5HJJHNP8094BRO",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestProperty()
                                                    {
                                                        RequestPropertyType = RequestPropertyType.HttpQuery,
                                                        Key = "function",
                                                        Value = "CURRENCY_EXCHANGE_RATE",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestProperty()
                                                    {
                                                        RequestPropertyType = RequestPropertyType.HttpQuery,
                                                        Key = "from_currency",
                                                        Value = "EUR",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestProperty()
                                                    {
                                                        RequestPropertyType = RequestPropertyType.HttpQuery,
                                                        Key = "to_currency",
                                                        Value = "USD",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                        APIUrl = "https://poloniex.com/public?command=returnTicker",
                                        DefaultComponent = "BTC_BCN/lowestAsk",
                                        SourceId = polo.Id,
                                        MainCurrencyAbbrv = "BTC",
                                        CounterCurrencyAbbrv = "BCN",
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Calculates volume ONLY for this exact Currency pair on this exchange.
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.DailyVolume,
                                                Delay = 1000,
                                                UIFormatting = "0 a",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 500,
                                                UIFormatting = "0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 500,
                                                UIFormatting = "0[.]0",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            }  
                                        },
                                        Requests = new List<Request>()
                                        {
                                            // POLO BTCBCN
                                            new Request()
                                            {
                                                Guid = Guid.Parse("419db9ee-0510-47d1-8b14-620e2c86dcb4"),
                                                RequestType = RequestType.HttpGet,
                                                ResponseType = ResponseType.Json,
                                                DataPath = "https://poloniex.com/public?command=returnTicker",
                                                Delay = 5000,
                                                RequestComponents = new List<RequestComponent>()
                                                {
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.DailyVolume,
                                                        Identifier = null,
                                                        QueryComponent = "BTC_BCN/baseVolume",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Ask,
                                                        Identifier = null,
                                                        QueryComponent = "BTC_BCN/lowestAsk",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Bid,
                                                        Identifier = null,
                                                        QueryComponent = "BTC_BCN/highestBid",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                        APIUrl = "https://poloniex.com/public?command=returnTicker",
                                        DefaultComponent = "BTC_BTS/lowestAsk",
                                        SourceId = polo.Id,
                                        MainCurrencyAbbrv = "BTC",
                                        CounterCurrencyAbbrv = "BTS",
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Calculates volume ONLY for this exact Currency pair on this exchange.
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.DailyVolume,
                                                Delay = 1000,
                                                UIFormatting = "0 a",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 500,
                                                UIFormatting = "0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 500,
                                                UIFormatting = "0[.]0",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            }
                                        },
                                        Requests = new List<Request>()
                                        {
                                            // POLO BTCBTS
                                            new Request()
                                            {
                                                Guid = Guid.Parse("b729acf9-a83c-4e76-8af8-a2ac7efc28c2"),
                                                RequestType = RequestType.HttpGet,
                                                ResponseType = ResponseType.Json,
                                                DataPath = "https://poloniex.com/public?command=returnTicker",
                                                Delay = 5000,
                                                RequestComponents = new List<RequestComponent>()
                                                {
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.DailyVolume,
                                                        Identifier = null,
                                                        QueryComponent = "BTC_BTS/baseVolume",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Ask,
                                                        Identifier = null,
                                                        QueryComponent = "BTC_BTS/lowestAsk",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Bid,
                                                        Identifier = null,
                                                        QueryComponent = "BTC_BTS/highestBid",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.TRADEABLE,
                                        APIUrl = "https://api.bitfinex.com/v1/pubticker/etheur",
                                        DefaultComponent = "0",
                                        SourceId = bfx.Id,
                                        MainCurrencyAbbrv = "ETH",
                                        CounterCurrencyAbbrv = "EUR",
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Calculates volume ONLY for this exact Currency pair on this exchange.
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.DailyVolume,
                                                Delay = 1000,
                                                UIFormatting = "0 a",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 500,
                                                UIFormatting = "0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 500,
                                                UIFormatting = "0[.]0",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            }  
                                        },
                                        Requests = new List<Request>()
                                        {
                                            // BFX ETHEUR
                                            new Request()
                                            {
                                                Guid = Guid.Parse("ee593665-c6c5-454a-8831-b7e28265a1c8"),
                                                RequestType = RequestType.HttpGet,
                                                ResponseType = ResponseType.Json,
                                                DataPath = "https://api.bitfinex.com/v1/pubticker/etheur",
                                                Delay = 2000,
                                                RequestComponents = new List<RequestComponent>()
                                                {
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.DailyVolume,
                                                        Identifier = null,
                                                        QueryComponent = "volume",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Ask,
                                                        Identifier = null,
                                                        QueryComponent = "ask",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent()
                                                    {
                                                        ComponentType = ComponentType.Bid,
                                                        Identifier = null,
                                                        QueryComponent = "bid",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new CurrencyPair()
                                    {
                                        CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                        APIUrl = "https://poloniex.com/public?command=returnTicker",
                                        DefaultComponent = "USDT_BTC/lowestAsk",
                                        SourceId = polo.Id,
                                        MainCurrencyAbbrv = "BTC",
                                        CounterCurrencyAbbrv = "USDT",
                                        Requests = new List<Request>()
                                        {
                                            // POLO BTCUSDT
                                            new Request()
                                            {
                                                Guid = Guid.Parse("e47e6062-e727-41ed-a0c1-750b1a792dd7"),
                                                RequestType = RequestType.HttpGet,
                                                ResponseType = ResponseType.Json,
                                                DataPath = "https://poloniex.com/public?command=returnTicker",
                                                Delay = 5000,
                                                RequestComponents = new List<RequestComponent>()
                                                {
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Ask,
                                                        Identifier = null,
                                                        QueryComponent = "USDT_BTC/lowestAsk",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Bid,
                                                        Identifier = null,
                                                        QueryComponent = "USDT_BTC/highestBid",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                        APIUrl = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                        DefaultComponent = "b",
                                        SourceId = bna.Id,
                                        MainCurrencyAbbrv = "ETH",
                                        CounterCurrencyAbbrv = "BTC",
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Calculates volume ONLY for this exact Currency pair on this exchange.
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.DailyVolume,
                                                Delay = 1000,
                                                UIFormatting = "0 a",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 500,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 500,
                                                UIFormatting = "0[.]0",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            }
                                        },
                                        Requests = new List<Request>()
                                        {
                                            // Binance's Websocket-based ticker data stream
                                            new Request
                                            {
                                                Guid = Guid.Parse("6f9d8fe7-71f4-42b8-ac31-526f559549a3"),
                                                RequestType = RequestType.WebSocket,
                                                ResponseType = ResponseType.Json,
                                                DataPath = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                                Delay = 0,
                                                RequestComponents = new List<RequestComponent>()
                                                {
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.DailyVolume,
                                                        Identifier = "data/s=>ETHBTC",
                                                        QueryComponent = "v",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Ask,
                                                        Identifier = "data/s=>ETHBTC",
                                                        QueryComponent = "a",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Bid,
                                                        Identifier = "data/s=>ETHBTC",
                                                        QueryComponent = "b",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                        APIUrl = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                        DefaultComponent = "b",
                                        SourceId = bna.Id,
                                        MainCurrencyAbbrv = "KNC",
                                        CounterCurrencyAbbrv = "ETH",
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Calculates volume ONLY for this exact Currency pair on this exchange.
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.DailyVolume,
                                                Delay = 1000,
                                                UIFormatting = "0 a",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 500,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 500,
                                                UIFormatting = "0[.]0",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            }  
                                        },
                                        Requests = new List<Request>()
                                        {
                                            new Request
                                            {
                                                Guid = Guid.Parse("dc33dc82-26e5-4eef-af44-78e1efce2d1f"),
                                                RequestType = RequestType.WebSocket,
                                                ResponseType = ResponseType.Json,
                                                DataPath = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                                Delay = 0,
                                                RequestComponents = new List<RequestComponent>()
                                                {
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.DailyVolume,
                                                        Identifier = "data/s=>KNCETH",
                                                        QueryComponent = "v",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Ask,
                                                        Identifier = "data/s=>KNCETH",
                                                        QueryComponent = "a",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Bid,
                                                        Identifier = "data/s=>KNCETH",
                                                        QueryComponent = "b",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                        APIUrl = "https://www.coinhako.com/api/v1/price/currency/BTCSGD",
                                        DefaultComponent = "data/buy_price",
                                        SourceId = hako.Id,
                                        MainCurrencyAbbrv = "BTC",
                                        CounterCurrencyAbbrv = "SGD",
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Coinhako BTCSGD
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            // Coinhako BTCSGD
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            // Coinhako BTCSGD
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 10000,
                                                UIFormatting = "0[.]0",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            }
                                        },
                                        Requests = new List<Request>()
                                        {
                                            // Coinhako BTCSGD
                                            new Request()
                                            {
                                                Guid = Guid.Parse("c162e683-cceb-4a03-aa24-f095b4d9db1f"),
                                                RequestType = RequestType.HttpGet,
                                                ResponseType = ResponseType.Json,
                                                DataPath = "https://www.coinhako.com/api/v1/price/currency/BTCSGD",
                                                Delay = 10000,
                                                RequestComponents = new List<RequestComponent>()
                                                {
                                                    // Coinhako BTCSGD
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Bid,
                                                        Identifier = null,
                                                        QueryComponent = "data/buy_price",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Ask,
                                                        Identifier = null,
                                                        QueryComponent = "data/sell_price",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                        APIUrl = "https://www.coinhako.com/api/v1/price/currency/BTCUSD",
                                        DefaultComponent = "data/buy_price",
                                        SourceId = hako.Id,
                                        MainCurrencyAbbrv = "BTC",
                                        CounterCurrencyAbbrv = "USD",
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Coinhako BTCUSD
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            // Coinhako BTCUSD
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            // Coinhako BTCUSD
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 10000,
                                                UIFormatting = "0[.]0",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            }
                                        },
                                        Requests = new List<Request>()
                                        {
                                            // Coinhako BTCUSD
                                            new Request()
                                            {
                                                Guid = Guid.Parse("fd199860-f699-4414-ba14-fdae9e856b5e"),
                                                RequestType = RequestType.HttpGet,
                                                ResponseType = ResponseType.Json,
                                                DataPath = "https://www.coinhako.com/api/v1/price/currency/BTCUSD",
                                                Delay = 10000,
                                                RequestComponents = new List<RequestComponent>()
                                                {
                                                    // Coinhako BTCUSD
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Bid,
                                                        Identifier = null,
                                                        QueryComponent = "data/buy_price",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Ask,
                                                        Identifier = null,
                                                        QueryComponent = "data/sell_price",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                        APIUrl = "https://www.coinhako.com/api/v1/price/currency/ETHSGD",
                                        DefaultComponent = "data/buy_price",
                                        SourceId = hako.Id,
                                        MainCurrencyAbbrv = "ETH",
                                        CounterCurrencyAbbrv = "SGD",
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Coinhako ETHSGD
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            // Coinhako ETHSGD
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            // Coinhako ETHSGD
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 10000,
                                                UIFormatting = "0[.]0",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            }
                                        },
                                        Requests = new List<Request>()
                                        {
                                            // Coinhako ETHSGD
                                            new Request()
                                            {
                                                Guid = Guid.Parse("49be3d33-d7b8-47aa-abf0-ee8765100b21"),
                                                RequestType = RequestType.HttpGet,
                                                ResponseType = ResponseType.Json,
                                                DataPath = "https://www.coinhako.com/api/v1/price/currency/ETHSGD",
                                                Delay = 10000,
                                                RequestComponents = new List<RequestComponent>()
                                                {
                                                    // Coinhako ETHSGD
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Bid,
                                                        Identifier = null,
                                                        QueryComponent = "data/buy_price",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Ask,
                                                        Identifier = null,
                                                        QueryComponent = "data/sell_price",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                        APIUrl = "https://www.coinhako.com/api/v1/price/currency/ETHUSD",
                                        DefaultComponent = "data/buy_price",
                                        SourceId = hako.Id,
                                        MainCurrencyAbbrv = "ETH",
                                        CounterCurrencyAbbrv = "USD",
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Coinhako ETHUSD
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            // Coinhako ETHUSD
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            // Coinhako ETHUSD
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 10000,
                                                UIFormatting = "0[.]0",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            }
                                        },
                                        Requests = new List<Request>()
                                        {
                                            // Coinhako ETHUSD
                                            new Request()
                                            {
                                                Guid = Guid.Parse("ceb4e033-ebbb-45d9-9312-951f09228c30"),
                                                RequestType = RequestType.HttpGet,
                                                ResponseType = ResponseType.Json,
                                                DataPath = "https://www.coinhako.com/api/v1/price/currency/ETHUSD",
                                                Delay = 10000,
                                                RequestComponents = new List<RequestComponent>()
                                                {
                                                    // Coinhako ETHUSD
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Bid,
                                                        Identifier = null,
                                                        QueryComponent = "data/buy_price",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Ask,
                                                        Identifier = null,
                                                        QueryComponent = "data/sell_price",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                        APIUrl = "https://www.coinhako.com/api/v1/price/currency/LTCSGD",
                                        DefaultComponent = "data/buy_price",
                                        SourceId = hako.Id,
                                        MainCurrencyAbbrv = "LTC",
                                        CounterCurrencyAbbrv = "SGD",
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Coinhako LTCSGD
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            // Coinhako LTCSGD
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            // Coinhako LTCSGD
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 10000,
                                                UIFormatting = "0[.]0",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            }
                                        },
                                        Requests = new List<Request>()
                                        {
                                            // Coinhako LTCSGD
                                            new Request()
                                            {
                                                Guid = Guid.Parse("58bf3728-1887-4460-bf61-6b898be360f3"),
                                                RequestType = RequestType.HttpGet,
                                                ResponseType = ResponseType.Json,
                                                DataPath = "https://www.coinhako.com/api/v1/price/currency/LTCSGD",
                                                Delay = 10000,
                                                RequestComponents = new List<RequestComponent>()
                                                {
                                                    // Coinhako LTCSGD
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Bid,
                                                        Identifier = null,
                                                        QueryComponent = "data/buy_price",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Ask,
                                                        Identifier = null,
                                                        QueryComponent = "data/sell_price",
                                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0,
                                                            DateTimeKind.Utc),
                                                        DeletedAt = null
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                        APIUrl = "https://www.coinhako.com/api/v1/price/currency/LTCUSD",
                                        DefaultComponent = "data/buy_price",
                                        SourceId = hako.Id,
                                        MainCurrencyAbbrv = "LTC",
                                        CounterCurrencyAbbrv = "USD",
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Coinhako LTCUSD
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            // Coinhako LTCUSD
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                Delay = 10000,
                                                UIFormatting = "$ 0[.]00",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            },
                                            // Coinhako LTCUSD
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 10000,
                                                UIFormatting = "0[.]0",
                                                CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                                DeletedAt = null
                                            }
                                        },
                                        Requests = new List<Request>()
                                        {
                                            // Coinhako LTCUSD
                                            new Request()
                                            {
                                                Guid = Guid.Parse("92121fbb-8f01-45de-bfab-fe17aeac7174"),
                                                RequestType = RequestType.HttpGet,
                                                ResponseType = ResponseType.Json,
                                                DataPath = "https://www.coinhako.com/api/v1/price/currency/LTCUSD",
                                                Delay = 10000,
                                                RequestComponents = new List<RequestComponent>()
                                                {
                                                    // Coinhako LTCUSD
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Bid,
                                                        Identifier = null,
                                                        QueryComponent = "data/buy_price"
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Ask,
                                                        Identifier = null,
                                                        QueryComponent = "data/sell_price"
                                                    }
                                                }
                                            }
                                        }
                                    });

                                context.SaveChanges();
                            }

                            if (!context.CurrencySources.Any() && context.Currencies.Any())
                            {
                                var usd = context.Currencies
                                    .SingleOrDefault(c =>
                                        c.Slug.Equals("USD", StringComparison.InvariantCultureIgnoreCase));
                                var eur = context.Currencies
                                    .SingleOrDefault(c =>
                                        c.Slug.Equals("EUR", StringComparison.InvariantCultureIgnoreCase));
                                var eth = context.Currencies
                                    .SingleOrDefault(c =>
                                        c.Slug.Equals("ETH", StringComparison.InvariantCultureIgnoreCase));
                                var knc = context.Currencies
                                    .SingleOrDefault(c =>
                                        c.Slug.Equals("KNC", StringComparison.InvariantCultureIgnoreCase));
                                var btc = context.Currencies
                                    .SingleOrDefault(c =>
                                        c.Slug.Equals("BTC", StringComparison.InvariantCultureIgnoreCase));
                                var bcn = context.Currencies
                                    .SingleOrDefault(c =>
                                        c.Slug.Equals("BCN", StringComparison.InvariantCultureIgnoreCase));
                                var bts = context.Currencies
                                    .SingleOrDefault(c =>
                                        c.Slug.Equals("BTS", StringComparison.InvariantCultureIgnoreCase));
                                var usdt = context.Currencies
                                    .SingleOrDefault(c =>
                                        c.Slug.Equals("USDT", StringComparison.InvariantCultureIgnoreCase));
                                var sgd = context.Currencies
                                    .SingleOrDefault(c =>
                                        c.Slug.Equals("SGD", StringComparison.InvariantCultureIgnoreCase));
                                var ltc = context.Currencies
                                    .SingleOrDefault(c =>
                                        c.Slug.Equals("LTC", StringComparison.InvariantCultureIgnoreCase));

                                context.CurrencySources.AddRange(
                                    // Bitfinex USD
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = usd.Id,
                                        SourceId = bfx.Id
                                    },
                                    // Bitfinex EUR
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = eur.Id,
                                        SourceId = bfx.Id
                                    },
                                    // Bitfinex ETH
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = eth.Id,
                                        SourceId = bfx.Id
                                    },
                                    // Bitfinex KNC
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = knc.Id,
                                        SourceId = bfx.Id
                                    },
                                    // Binance KNC
                                    new CurrencySource
                                    {
                                        CurrencyId = knc.Id,
                                        SourceId = bna.Id
                                    },
                                    // Binance ETH
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = eth.Id,
                                        SourceId = bna.Id
                                    },
                                    // Binance BTC
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = btc.Id,
                                        SourceId = bna.Id
                                    },
                                    // European Central Bank EUR
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = eur.Id,
                                        SourceId = ecb.Id
                                    },
                                    // European Central Bank USD
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = usd.Id,
                                        SourceId = ecb.Id
                                    },
                                    // AlphaVantage EUR
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = eur.Id,
                                        SourceId = avg.Id
                                    },
                                    // AlphaVantage USD
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = usd.Id,
                                        SourceId = avg.Id
                                    },
                                    // Poloniex BTC
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = btc.Id,
                                        SourceId = polo.Id
                                    },
                                    // Poloniex BCN
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = bcn.Id,
                                        SourceId = polo.Id
                                    },
                                    // Poloniex BTS
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = bts.Id,
                                        SourceId = polo.Id
                                    },
                                    // Poloniex USDT
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = usdt.Id,
                                        SourceId = polo.Id
                                    },
                                    // Coinhako BTC
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = btc.Id,
                                        SourceId = hako.Id
                                    },
                                    // Coinhako SGD
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = sgd.Id,
                                        SourceId = hako.Id
                                    },
                                    // Coinhako ETH
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = bcn.Id,
                                        SourceId = hako.Id
                                    },
                                    // Coinhako USD
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = usd.Id,
                                        SourceId = hako.Id
                                    },
                                    // Coinhako LTC
                                    new CurrencySource
                                    {
                                        CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                                        DeletedAt = null,
                                        CurrencyId = ltc.Id,
                                        SourceId = hako.Id
                                    });
                            }

                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }
    }
}