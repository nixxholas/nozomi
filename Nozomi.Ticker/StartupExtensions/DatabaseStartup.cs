using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.WebModels;
using Nozomi.Repo.Data;
using Nozomi.Repo.Identity.Data;
using Nozomi.Service.Identity.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Identity.Services.Interfaces;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class DatabaseStartup
    {
        public static void UseAutoDbMigration(this IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var stripeService = serviceScope.ServiceProvider.GetService<IStripeService>();
                stripeService.ConfigureStripePlans();

                var logger = serviceScope.ServiceProvider.GetService<ILogger<Startup>>();
                
                using (var context = serviceScope.ServiceProvider.GetService<NozomiAuthContext>())
                {
                    // Auto wipe
                    if (env.IsDevelopment())
                    {
                        context.Database.EnsureDeleted();
                    }
                    
                    context.Database.Migrate();
                    
                    // Seed roles
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
                    }
                }
                
                using (var context = serviceScope.ServiceProvider.GetService<NozomiDbContext>())
                {
                    // Auto Wipe
                    if (env.IsDevelopment())
                    {
                        context.Database.EnsureDeleted();
                    }

                    context.Database.Migrate();

                    if (!context.Sources.Any())
                    {
                        context.AddRange(
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
                            });
                        context.SaveChanges();
                    }

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
                                Name = "Cryptocurrency"
                            });

                        context.SaveChanges();
                    }

                    if (context.Sources.Any())
                    {
                        var bfxSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("BFX"));
                        var bnaSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("BNA"));
                        var ecbSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("ECB"));
                        var avgSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("AVG"));

                        if (!context.Currencies.Any() && context.CurrencyTypes.Any())
                        {
                            var fiatType = context.CurrencyTypes.SingleOrDefault(ct => ct.TypeShortForm.Equals("FIAT"));
                            var cryptoType =
                                context.CurrencyTypes.SingleOrDefault(ct => ct.TypeShortForm.Equals("CRYPTO"));

                            if (fiatType != null && cryptoType != null && bfxSource != null && bnaSource != null
                                && ecbSource != null && avgSource != null)
                                context.Currencies.AddRange(
                                    new Currency()
                                    {
                                        CurrencyTypeId = fiatType.Id,
                                        Abbrv = "USD",
                                        Name = "United States Dollar",
                                        CurrencySourceId = bfxSource.Id,
                                        WalletTypeId = 0
                                    },
                                    new Currency()
                                    {
                                        CurrencyTypeId = cryptoType.Id,
                                        Abbrv = "ETH",
                                        Name = "Ethereum",
                                        CurrencySourceId = bfxSource.Id,
                                        WalletTypeId = 1 // As per CNWallet
                                    },
                                    new Currency()
                                    {
                                        CurrencyTypeId = cryptoType.Id,
                                        Abbrv = "KNC",
                                        Name = "Kyber Network Coin",
                                        CurrencySourceId = bfxSource.Id,
                                        WalletTypeId = 4 // As per CNWallet
                                    },
                                    new Currency()
                                    {
                                        CurrencyTypeId = cryptoType.Id,
                                        Abbrv = "KNC",
                                        Name = "Kyber Network Coin",
                                        CurrencySourceId = bnaSource.Id,
                                        WalletTypeId = 4 // As per CNWallet
                                    },
                                    new Currency()
                                    {
                                        CurrencyTypeId = cryptoType.Id,
                                        Abbrv = "ETH",
                                        Name = "Ethereum",
                                        CurrencySourceId = bnaSource.Id,
                                        WalletTypeId = 1 // As per CNWallet
                                    },
                                    new Currency()
                                    {
                                        CurrencyTypeId = fiatType.Id,
                                        Abbrv = "EUR",
                                        Name = "Euro",
                                        CurrencySourceId = ecbSource.Id,
                                        WalletTypeId = 0
                                    },
                                    new Currency()
                                    {
                                        CurrencyTypeId = fiatType.Id,
                                        Abbrv = "USD",
                                        Name = "United States Dollar",
                                        CurrencySourceId = ecbSource.Id,
                                        WalletTypeId = 0
                                    },
                                    new Currency()
                                    {
                                        CurrencyTypeId = fiatType.Id,
                                        Abbrv = "EUR",
                                        Name = "Euro",
                                        CurrencySourceId = avgSource.Id,
                                        WalletTypeId = 0
                                    },
                                    new Currency()
                                    {
                                        CurrencyTypeId = fiatType.Id,
                                        Abbrv = "USD",
                                        Name = "United States Dollar",
                                        CurrencySourceId = avgSource.Id,
                                        WalletTypeId = 0
                                    }
                                );

                            context.SaveChanges();
                        }

                        if (!context.CurrencyPairs.Any())
                        {
                            var currencyPairs = new List<CurrencyPair>()
                            {
                                new CurrencyPair()
                                {
                                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                                    APIUrl = "https://api.ethfinex.com/v2/ticker/tETHUSD",
                                    DefaultComponent = "0",
                                    CurrencySourceId = bfxSource.Id
                                },
                                new CurrencyPair()
                                {
                                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                                    APIUrl = "https://api.ethfinex.com/v2/ticker/tKNCUSD",
                                    DefaultComponent = "0",
                                    CurrencySourceId = bfxSource.Id
                                },
                                new CurrencyPair()
                                {
                                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                                    APIUrl = "https://api.binance.com/api/v3/ticker/bookTicker?symbol=KNCETH",
                                    DefaultComponent = "askPrice",
                                    CurrencySourceId = bnaSource.Id
                                },
                                new CurrencyPair()
                                {
                                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                                    APIUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
                                    DefaultComponent = "Cube",
                                    CurrencySourceId = ecbSource.Id
                                },
                                new CurrencyPair()
                                {
                                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                                    APIUrl = "https://www.alphavantage.co/query",
                                    DefaultComponent = "Realtime Currency Exchange Rate/5. Exchange Rate",
                                    CurrencySourceId = avgSource.Id
                                }
                            };

                            context.CurrencyPairs.AddRange(currencyPairs);

                            context.SaveChanges();

                            if (!context.CurrencyPairRequests.Any() && context.CurrencyPairs.Any())
                            {
                                var currencyPairRequests = new List<CurrencyPairRequest>()
                                {
                                    new CurrencyPairRequest()
                                    {
                                        Guid = Guid.NewGuid(),
                                        RequestType = RequestType.HttpGet,
                                        DataPath = "https://api.ethfinex.com/v2/ticker/tETHUSD",
                                        CurrencyPairId = currencyPairs[0].Id,
                                        Delay = 5000,
                                        RequestComponents = new List<RequestComponent>()
                                        {
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Ask,
                                                QueryComponent = "2",
                                                CreatedAt = DateTime.Now,
                                                ModifiedAt = DateTime.Now,
                                                DeletedAt = null
                                            },
                                            new RequestComponent()
                                            {
                                                ComponentType = ComponentType.Bid,
                                                QueryComponent = "0",
                                                CreatedAt = DateTime.Now,
                                                ModifiedAt = DateTime.Now,
                                                DeletedAt = null
                                            }
                                        }
                                    },
                                    new CurrencyPairRequest()
                                    {
                                        Guid = Guid.NewGuid(),
                                        RequestType = RequestType.HttpGet,
                                        DataPath = "https://api.ethfinex.com/v2/ticker/tKNCUSD",
                                        CurrencyPairId = currencyPairs[1].Id,
                                        Delay = 5000,
                                        RequestComponents = new List<RequestComponent>()
                                        {
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Bid,
                                                QueryComponent = "0",
                                                CreatedAt = DateTime.Now,
                                                ModifiedAt = DateTime.Now,
                                                DeletedAt = null
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Ask,
                                                QueryComponent = "2",
                                                CreatedAt = DateTime.Now,
                                                ModifiedAt = DateTime.Now,
                                                DeletedAt = null
                                            }
                                        }
                                    },
                                    new CurrencyPairRequest()
                                    {
                                        Guid = Guid.NewGuid(),
                                        RequestType = RequestType.HttpGet,
                                        DataPath = "https://api.binance.com/api/v3/ticker/bookTicker?symbol=KNCETH",
                                        CurrencyPairId = currencyPairs[2].Id,
                                        Delay = 5000,
                                        RequestComponents = new List<RequestComponent>()
                                        {
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Ask,
                                                QueryComponent = "askPrice",
                                                CreatedAt = DateTime.Now,
                                                ModifiedAt = DateTime.Now,
                                                DeletedAt = null
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Bid,
                                                QueryComponent = "bidPrice",
                                                CreatedAt = DateTime.Now,
                                                ModifiedAt = DateTime.Now,
                                                DeletedAt = null
                                            }
                                        }
                                    },
                                    new CurrencyPairRequest()
                                    {
                                        Guid = Guid.NewGuid(),
                                        RequestType = RequestType.HttpGet,
                                        ResponseType = ResponseType.XML,
                                        DataPath = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
                                        CurrencyPairId = currencyPairs[3].Id,
                                        Delay = 86400000,
                                        RequestComponents = new List<RequestComponent>()
                                        {
                                            new RequestComponent()
                                            {
                                                ComponentType = ComponentType.Ask,
                                                QueryComponent = "gesmes:Envelope/Cube/Cube/Cube/0=>@rate",
                                                CreatedAt = DateTime.Now,
                                                ModifiedAt = DateTime.Now,
                                                DeletedAt = null
                                            }
                                        }
                                    },
                                    new CurrencyPairRequest()
                                    {
                                        Guid = Guid.NewGuid(),
                                        RequestType = RequestType.HttpGet,
                                        ResponseType = ResponseType.Json,
                                        DataPath = "https://www.alphavantage.co/query",
                                        CurrencyPairId = currencyPairs[4].Id,
                                        Delay = 5000,
                                        RequestComponents = new List<RequestComponent>()
                                        {
                                            new RequestComponent()
                                            {
                                                ComponentType = ComponentType.Ask,
                                                QueryComponent = "['Realtime Currency Exchange Rate']/['5. Exchange Rate']",
                                                CreatedAt = DateTime.Now,
                                                ModifiedAt = DateTime.Now,
                                                DeletedAt = null
                                            }
                                        },
                                        RequestProperties = new List<RequestProperty>()
                                        {
                                            new RequestProperty()
                                            {
                                                RequestPropertyType = RequestPropertyType.HttpQuery,
                                                Key = "apikey",
                                                Value = "TV5HJJHNP8094BRO"
                                            },
                                            new RequestProperty()
                                            {
                                                RequestPropertyType = RequestPropertyType.HttpQuery,
                                                Key = "function",
                                                Value = "CURRENCY_EXCHANGE_RATE"
                                            },
                                            new RequestProperty()
                                            {
                                                RequestPropertyType = RequestPropertyType.HttpQuery,
                                                Key = "from_currency",
                                                Value = "USD"
                                            },
                                            new RequestProperty()
                                            {
                                                RequestPropertyType = RequestPropertyType.HttpQuery,
                                                Key = "to_currency",
                                                Value = "CNY"
                                            }
                                        }
                                    }
                                };

                                context.CurrencyPairRequests.AddRange(currencyPairRequests);

                                context.SaveChanges();
                            }

                            if (!context.PartialCurrencyPairs.Any() && context.CurrencyPairs.Any() &&
                                context.Currencies.Any())
                            {
                                var usdBfx = context.Currencies.Include(c => c.CurrencySource)
                                    .SingleOrDefault(c =>
                                        c.Abbrv.Equals("USD") &&
                                        c.CurrencySource.Abbreviation.Equals(bfxSource.Abbreviation));
                                var ethBfx = context.Currencies.Include(c => c.CurrencySource)
                                    .SingleOrDefault(c =>
                                        c.Abbrv.Equals("ETH") &&
                                        c.CurrencySource.Abbreviation.Equals(bfxSource.Abbreviation));
                                var kncBfx = context.Currencies.Include(c => c.CurrencySource)
                                    .SingleOrDefault(c =>
                                        c.Abbrv.Equals("KNC") &&
                                        c.CurrencySource.Abbreviation.Equals(bfxSource.Abbreviation));
                                var kncBna = context.Currencies.Include(c => c.CurrencySource)
                                    .SingleOrDefault(c =>
                                        c.Abbrv.Equals("KNC") &&
                                        c.CurrencySource.Abbreviation.Equals(bnaSource.Abbreviation));
                                var ethBna = context.Currencies.Include(c => c.CurrencySource)
                                    .SingleOrDefault(c =>
                                        c.Abbrv.Equals("ETH") &&
                                        c.CurrencySource.Abbreviation.Equals(bnaSource.Abbreviation));
                                var eurECB = context.Currencies.Include(c => c.CurrencySource)
                                    .SingleOrDefault(c =>
                                        c.Abbrv.Equals("EUR") &&
                                        c.CurrencySource.Abbreviation.Equals(ecbSource.Abbreviation));
                                var usdECB = context.Currencies.Include(c => c.CurrencySource)
                                    .SingleOrDefault(c =>
                                        c.Abbrv.Equals("USD") &&
                                        c.CurrencySource.Abbreviation.Equals(ecbSource.Abbreviation));
                                var eurAVG = context.Currencies.Include(c => c.CurrencySource)
                                    .SingleOrDefault(c =>
                                        c.Abbrv.Equals("EUR") &&
                                        c.CurrencySource.Abbreviation.Equals(avgSource.Abbreviation));
                                var usdAVG = context.Currencies.Include(c => c.CurrencySource)
                                    .SingleOrDefault(c =>
                                        c.Abbrv.Equals("USD") &&
                                        c.CurrencySource.Abbreviation.Equals(avgSource.Abbreviation));

                                context.PartialCurrencyPairs.AddRange(
                                    new PartialCurrencyPair()
                                    {
                                        CurrencyId = usdBfx.Id,
                                        IsMain = false,
                                        CurrencyPairId = currencyPairs[0].Id
                                    },
                                    new PartialCurrencyPair()
                                    {
                                        CurrencyId = ethBfx.Id,
                                        IsMain = true,
                                        CurrencyPairId = currencyPairs[0].Id
                                    },
                                    new PartialCurrencyPair()
                                    {
                                        CurrencyId = usdBfx.Id,
                                        IsMain = false,
                                        CurrencyPairId = currencyPairs[1].Id
                                    },
                                    new PartialCurrencyPair()
                                    {
                                        CurrencyId = kncBfx.Id,
                                        IsMain = true,
                                        CurrencyPairId = currencyPairs[1].Id
                                    },
                                    new PartialCurrencyPair()
                                    {
                                        CurrencyId = kncBna.Id,
                                        IsMain = true,
                                        CurrencyPairId = currencyPairs[2].Id
                                    },
                                    new PartialCurrencyPair()
                                    {
                                        CurrencyId = ethBna.Id,
                                        IsMain = false,
                                        CurrencyPairId = currencyPairs[2].Id
                                    },
                                    new PartialCurrencyPair()
                                    {
                                        CurrencyId = eurECB.Id,
                                        IsMain = true,
                                        CurrencyPairId = currencyPairs[3].Id
                                    },
                                    new PartialCurrencyPair()
                                    {
                                        CurrencyId = usdECB.Id,
                                        IsMain = false,
                                        CurrencyPairId = currencyPairs[3].Id
                                    },
                                    new PartialCurrencyPair()
                                    {
                                        CurrencyId = eurAVG.Id,
                                        IsMain = true,
                                        CurrencyPairId = currencyPairs[4].Id
                                    },
                                    new PartialCurrencyPair()
                                    {
                                        CurrencyId = usdAVG.Id,
                                        IsMain = false,
                                        CurrencyPairId = currencyPairs[4].Id
                                    });

                                context.SaveChanges();
                            }
                        }
                    }
                }
            }
        }
    }
}