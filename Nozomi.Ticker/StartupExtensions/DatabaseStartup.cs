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
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.Models.Web.Websocket;
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

                        if (userManager.FindByEmailAsync("nicholas@counter.network").Result == null)
                        {
                            var boss = new User
                            {
                                UserName = "nicholas",
                                NormalizedUserName = "NICHOLAS",
                                NormalizedEmail = "NICHOLAS@COUNTER.NETWORK",
                                Email = "nicholas@counter.network",
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

                    if (context.Sources.Any())
                    {
                        var bfxSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("BFX"));
                        var bnaSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("BNA"));
                        var ecbSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("ECB"));
                        var avgSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("AVG"));
                        var poloSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("POLO"));

                        if (!context.Currencies.Any() && context.CurrencyTypes.Any())
                        {
                            var fiatType = context.CurrencyTypes.SingleOrDefault(ct => ct.TypeShortForm.Equals("FIAT"));
                            var cryptoType =
                                context.CurrencyTypes.SingleOrDefault(ct => ct.TypeShortForm.Equals("CRYPTO"));

                            if (fiatType != null && cryptoType != null && bfxSource != null && bnaSource != null
                                && ecbSource != null && avgSource != null)
                                context.Currencies.AddRange(
                                    new Currency
                                    {
                                        CurrencyTypeId = fiatType.Id,
                                        Abbrv = "USD",
                                        Name = "United States Dollar",
                                        CurrencySourceId = bfxSource.Id,
                                        WalletTypeId = 0
                                    },
                                    new Currency
                                    {
                                        CurrencyTypeId = fiatType.Id,
                                        Abbrv = "EUR",
                                        Name = "Euro",
                                        CurrencySourceId = bfxSource.Id,
                                        WalletTypeId = 0
                                    },
                                    new Currency
                                    {
                                        CurrencyTypeId = cryptoType.Id,
                                        Abbrv = "ETH",
                                        Name = "Ethereum",
                                        CurrencySourceId = bfxSource.Id,
                                        WalletTypeId = 1, // As per CNWallet
                                        Denominations = 18,
                                        DenominationName = "Wei",
                                        // Calculates mCap ONLY for this exact Currency pair on this exchange.
                                        AnalysedComponents = new List<AnalysedComponent>
                                        {
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.MarketCap,
                                                Delay = 1000,
                                                UIFormatting = "$ 0 a",
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                IsDenominated = true,
                                                DeletedAt = null
                                            }
                                        },
                                        CurrencyRequests = new List<CurrencyRequest>
                                        {
                                            new CurrencyRequest
                                            {
                                                Guid = Guid.NewGuid(),
                                                RequestType = RequestType.HttpGet,
                                                DataPath = "https://api.etherscan.io/api",
                                                Delay = 5000,
                                                RequestComponents = new List<RequestComponent>
                                                {
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Circulating_Supply,
                                                        IsDenominated = true,
                                                        QueryComponent = "result",
                                                        CreatedAt = DateTime.UtcNow,
                                                        ModifiedAt = DateTime.UtcNow,
                                                        DeletedAt = null
                                                    }
                                                },
                                                RequestProperties = new List<RequestProperty>
                                                {
                                                    new RequestProperty
                                                    {
                                                        RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                                                        Key = "module",
                                                        Value = "stats",
                                                    },
                                                    new RequestProperty
                                                    {
                                                        RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                                                        Key = "action",
                                                        Value = "ethsupply",
                                                    },
                                                    new RequestProperty
                                                    {
                                                        RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                                                        Key = "apikey",
                                                        Value = "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224",
                                                    }
                                                }
                                            },
                                        }
                                    },
                                    new Currency
                                    {
                                        CurrencyTypeId = cryptoType.Id,
                                        Abbrv = "KNC",
                                        Name = "Kyber Network Crystal",
                                        CurrencySourceId = bfxSource.Id,
                                        Denominations = 18,
                                        WalletTypeId = 4, // As per CNWallet
                                        // Calculates mCap ONLY for this exact Currency pair on this exchange.
                                        AnalysedComponents = new List<AnalysedComponent>
                                        {
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.MarketCap,
                                                Delay = 1000,
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                IsDenominated = true,
                                                DeletedAt = null
                                            }
                                        },
                                        CurrencyRequests = new List<CurrencyRequest>
                                        {
                                            new CurrencyRequest
                                            {
                                                Guid = Guid.NewGuid(),
                                                RequestType = RequestType.HttpGet,
                                                DataPath = "https://api.etherscan.io/api",
                                                Delay = 5000,
                                                RequestComponents = new List<RequestComponent>
                                                {
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Circulating_Supply,
                                                        IsDenominated = true,
                                                        QueryComponent = "result",
                                                        CreatedAt = DateTime.UtcNow,
                                                        ModifiedAt = DateTime.UtcNow,
                                                        DeletedAt = null
                                                    }
                                                },
                                                RequestProperties = new List<RequestProperty>
                                                {
                                                    new RequestProperty
                                                    {
                                                        RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                                                        Key = "module",
                                                        Value = "stats",
                                                    },
                                                    new RequestProperty
                                                    {
                                                        RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                                                        Key = "action",
                                                        Value = "tokensupply",
                                                    },
                                                    new RequestProperty
                                                    {
                                                        RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                                                        Key = "apikey",
                                                        Value = "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224",
                                                    }
                                                }
                                            },
                                        }
                                    },
                                    new Currency
                                    {
                                        CurrencyTypeId = cryptoType.Id,
                                        Abbrv = "KNC",
                                        Name = "Kyber Network Crystal",
                                        CurrencySourceId = bnaSource.Id,
                                        WalletTypeId = 4 // As per CNWallet
                                    },
                                    new Currency
                                    {
                                        CurrencyTypeId = cryptoType.Id,
                                        Abbrv = "ETH",
                                        Name = "Ethereum",
                                        CurrencySourceId = bnaSource.Id,
                                        WalletTypeId = 1, // As per CNWallet
                                        Denominations = 18,
                                        DenominationName = "Wei",
                                    },
                                    new Currency
                                    {
                                        CurrencyTypeId = cryptoType.Id,
                                        Abbrv = "BTC",
                                        Name = "Bitcoin",
                                        CurrencySourceId = bnaSource.Id,
                                        WalletTypeId = 0, // As per CNWallet
                                        Denominations = 8,
                                        DenominationName = "Sat"
                                    },
                                    new Currency
                                    {
                                        CurrencyTypeId = fiatType.Id,
                                        Abbrv = "EUR",
                                        Name = "Euro",
                                        CurrencySourceId = ecbSource.Id,
                                        WalletTypeId = 0
                                    },
                                    new Currency
                                    {
                                        CurrencyTypeId = fiatType.Id,
                                        Abbrv = "USD",
                                        Name = "United States Dollar",
                                        CurrencySourceId = ecbSource.Id,
                                        WalletTypeId = 0
                                    },
                                    new Currency
                                    {
                                        CurrencyTypeId = fiatType.Id,
                                        Abbrv = "EUR",
                                        Name = "Euro",
                                        CurrencySourceId = avgSource.Id,
                                        WalletTypeId = 0
                                    },
                                    new Currency
                                    {
                                        CurrencyTypeId = fiatType.Id,
                                        Abbrv = "USD",
                                        Name = "United States Dollar",
                                        CurrencySourceId = avgSource.Id,
                                        WalletTypeId = 0
                                    },
                                    new Currency
                                    {
                                        CurrencyTypeId = cryptoType.Id,
                                        Abbrv = "BTC",
                                        Name = "Bitcoin",
                                        CurrencySourceId = poloSource.Id,
                                        WalletTypeId = 0,
                                        Denominations = 8,
                                        DenominationName = "Sat",
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.MarketCap,
                                                Delay = 500,
                                                UIFormatting = "$ 0 a",
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            }
                                        },
                                        CurrencyRequests = new List<CurrencyRequest>()
                                        {
                                            new CurrencyRequest
                                            {
                                                Guid = Guid.NewGuid(),
                                                RequestType = RequestType.HttpGet,
                                                DataPath = "https://insight.bitpay.com/api/status?q=getBlockCount",
                                                Delay = 90000,
                                                RequestComponents = new List<RequestComponent>
                                                {
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.BlockCount,
                                                        QueryComponent = "info/blocks",
                                                        CreatedAt = DateTime.UtcNow,
                                                        ModifiedAt = DateTime.UtcNow,
                                                        DeletedAt = null
                                                    },
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Difficulty,
                                                        QueryComponent = "info/difficulty",
                                                        CreatedAt = DateTime.UtcNow,
                                                        ModifiedAt = DateTime.UtcNow,
                                                        DeletedAt = null
                                                    }
                                                }
                                            },
                                            new CurrencyRequest
                                            {
                                                Guid = Guid.NewGuid(),
                                                RequestType = RequestType.HttpGet,
                                                DataPath = "https://api.coinranking.com/v1/public/coin/1?base=USD",
                                                Delay = 90000,
                                                RequestComponents = new List<RequestComponent>
                                                {
                                                    new RequestComponent
                                                    {
                                                        ComponentType = ComponentType.Circulating_Supply,
                                                        QueryComponent = "data/coin/circulatingSupply",
                                                        CreatedAt = DateTime.UtcNow,
                                                        ModifiedAt = DateTime.UtcNow,
                                                        DeletedAt = null
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Currency
                                    {
                                        CurrencyTypeId = cryptoType.Id,
                                        Abbrv = "BCN",
                                        Name = "Bytecoin",
                                        CurrencySourceId = poloSource.Id,
                                        WalletTypeId = 0
                                    },
                                    new Currency
                                    {
                                        CurrencyTypeId = cryptoType.Id,
                                        Abbrv = "BTS",
                                        Name = "BitShares",
                                        CurrencySourceId = poloSource.Id,
                                        WalletTypeId = 0
                                    },
                                    new Currency
                                    {
                                        CurrencyTypeId = fiatType.Id,
                                        Abbrv = "USDT",
                                        Name = "Tether USD",
                                        CurrencySourceId = poloSource.Id,
                                        WalletTypeId = 0
                                    }
                                );

                            context.SaveChanges();
                        }

                        if (context.Currencies.Any())
                        {
                            var usdBfx = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("USD") &&
                                    c.CurrencySource.Abbreviation.Equals(bfxSource.Abbreviation));
                            var eurBfx = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("EUR") &&
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
                            var btcBna = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("BTC") &&
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
                            var btcPOLO = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("BTC") &&
                                    c.CurrencySource.Abbreviation.Equals(poloSource.Abbreviation));
                            var bcnPOLO = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("BCN") &&
                                    c.CurrencySource.Abbreviation.Equals(poloSource.Abbreviation));
                            var btsPOLO = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("BTS") &&
                                    c.CurrencySource.Abbreviation.Equals(poloSource.Abbreviation));
                            var usdtPOLO = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("USDT") &&
                                    c.CurrencySource.Abbreviation.Equals(poloSource.Abbreviation));

                            if (!context.CurrencyPairs.Any())
                            {
                                var currencyPairs = new List<CurrencyPair>()
                                {
                                    new CurrencyPair()
                                    {
                                        CurrencyPairType = CurrencyPairType.TRADEABLE,
                                        APIUrl = "https://api.ethfinex.com/v2/ticker/tETHUSD",
                                        DefaultComponent = "0",
                                        CurrencySourceId = bfxSource.Id,
                                        MainCurrency = ethBfx.Abbrv,
                                        CounterCurrency = usdBfx.Abbrv,
                                        CurrencyPairCurrencies = new List<CurrencyCurrencyPair>
                                        {
                                            new CurrencyCurrencyPair
                                            {
                                                CurrencyId = ethBfx.Id
                                            },
                                            new CurrencyCurrencyPair
                                            {
                                                CurrencyId = usdBfx.Id
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.TRADEABLE,
                                        APIUrl = "https://api.ethfinex.com/v2/ticker/tKNCUSD",
                                        DefaultComponent = "0",
                                        CurrencySourceId = bfxSource.Id,
                                        MainCurrency = kncBfx.Abbrv,
                                        CounterCurrency = usdBfx.Abbrv,
                                        CurrencyPairCurrencies = new List<CurrencyCurrencyPair>
                                        {
                                            new CurrencyCurrencyPair
                                            {
                                                CurrencyId = kncBfx.Id
                                            },
                                            new CurrencyCurrencyPair
                                            {
                                                CurrencyId = usdBfx.Id
                                            }
                                        }
                                    },
                                    new CurrencyPair()
                                    {
                                        CurrencyPairType = CurrencyPairType.TRADEABLE,
                                        APIUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
                                        DefaultComponent = "Cube",
                                        CurrencySourceId = ecbSource.Id,
                                        MainCurrency = eurECB.Abbrv,
                                        CounterCurrency = usdECB.Abbrv,
                                        CurrencyPairCurrencies = new List<CurrencyCurrencyPair>
                                        {
                                            new CurrencyCurrencyPair
                                            {
                                                CurrencyId = eurECB.Id
                                            },
                                            new CurrencyCurrencyPair
                                            {
                                                CurrencyId = usdECB.Id
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.TRADEABLE,
                                        APIUrl = "https://www.alphavantage.co/query",
                                        DefaultComponent = "Realtime Currency Exchange Rate/5. Exchange Rate",
                                        CurrencySourceId = avgSource.Id,
                                        MainCurrency = eurAVG.Abbrv,
                                        CounterCurrency = usdAVG.Abbrv,
                                        CurrencyPairCurrencies = new List<CurrencyCurrencyPair>
                                        {
                                            new CurrencyCurrencyPair
                                            {
                                                CurrencyId = eurAVG.Id
                                            },
                                            new CurrencyCurrencyPair
                                            {
                                                CurrencyId = usdAVG.Id
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                        APIUrl = "https://poloniex.com/public?command=returnTicker",
                                        DefaultComponent = "BTC_BCN/lowestAsk",
                                        CurrencySourceId = poloSource.Id,
                                        MainCurrency = btcPOLO.Abbrv,
                                        CounterCurrency = bcnPOLO.Abbrv,
                                        CurrencyPairCurrencies = new List<CurrencyCurrencyPair>
                                        {
                                            new CurrencyCurrencyPair
                                            {
                                                CurrencyId = btcPOLO.Id
                                            },
                                            new CurrencyCurrencyPair
                                            {
                                                CurrencyId = bcnPOLO.Id
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                        APIUrl = "https://poloniex.com/public?command=returnTicker",
                                        DefaultComponent = "BTC_BTS/lowestAsk",
                                        CurrencySourceId = poloSource.Id,
                                        MainCurrency = btcPOLO.Abbrv,
                                        CounterCurrency = btsPOLO.Abbrv,
                                        CurrencyPairCurrencies = new List<CurrencyCurrencyPair>
                                        {
                                            new CurrencyCurrencyPair
                                            {
                                                CurrencyId = btcPOLO.Id
                                            },
                                            new CurrencyCurrencyPair
                                            {
                                                CurrencyId = btsPOLO.Id
                                            }
                                        }
                                    },
                                    new CurrencyPair
                                    {
                                        CurrencyPairType = CurrencyPairType.TRADEABLE,
                                        APIUrl = "https://api.bitfinex.com/v1/pubticker/etheur",
                                        DefaultComponent = "0",
                                        CurrencySourceId = bfxSource.Id,
                                        MainCurrency = ethBfx.Abbrv,
                                        CounterCurrency = eurBfx.Abbrv,
                                        CurrencyPairCurrencies = new List<CurrencyCurrencyPair>
                                        {
                                            new CurrencyCurrencyPair
                                            {
                                                CurrencyId = ethBfx.Id
                                            },
                                            new CurrencyCurrencyPair
                                            {
                                                CurrencyId = eurBfx.Id
                                            }
                                        }
                                    },
                                    new CurrencyPair()
                                    {
                                        CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                        APIUrl = "https://poloniex.com/public?command=returnTicker",
                                        DefaultComponent = "USDT_BTC/lowestAsk",
                                        CurrencySourceId = poloSource.Id,
                                        MainCurrency = btcPOLO.Abbrv,
                                        CounterCurrency = usdtPOLO.Abbrv,
                                        CurrencyPairCurrencies = new List<CurrencyCurrencyPair>
                                        {
                                            new CurrencyCurrencyPair
                                            {
                                                CurrencyId = btcPOLO.Id
                                            },
                                            new CurrencyCurrencyPair
                                            {
                                                CurrencyId = usdtPOLO.Id
                                            }
                                        }
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
                                            AnalysedComponents = new List<AnalysedComponent>()
                                            {
                                                // Calculates volume ONLY for this exact Currency pair on this exchange.
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.DailyVolume,
                                                    Delay = 1000,
                                                    UIFormatting = "$ 0 a",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                    Delay = 500,
                                                    UIFormatting = "$ 0[.]00",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                    Delay = 10000,
                                                    UIFormatting = "$ 0[.]00",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent()
                                                {
                                                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                    Delay = 500,
                                                    UIFormatting = "0[.]0",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                }
                                            },
                                            RequestComponents = new List<RequestComponent>()
                                            {
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.VOLUME,
                                                    QueryComponent = "7",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.Ask,
                                                    QueryComponent = "2",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.Ask_Size,
                                                    QueryComponent = "3",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new RequestComponent()
                                                {
                                                    ComponentType = ComponentType.Bid,
                                                    QueryComponent = "0",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new RequestComponent()
                                                {
                                                    ComponentType = ComponentType.Bid_Size,
                                                    QueryComponent = "1",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
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
                                            AnalysedComponents = new List<AnalysedComponent>()
                                            {
                                                // Calculates volume ONLY for this exact Currency pair on this exchange.
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.DailyVolume,
                                                    Delay = 1000,
                                                    UIFormatting = "0 a",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                    Delay = 500,
                                                    UIFormatting = "$ 0[.]00",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                    Delay = 10000,
                                                    UIFormatting = "$ 0[.]00",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent()
                                                {
                                                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                    Delay = 500,
                                                    UIFormatting = "0[.]0",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                }
                                            },
                                            RequestComponents = new List<RequestComponent>()
                                            {
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.VOLUME,
                                                    QueryComponent = "7",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.Ask,
                                                    QueryComponent = "2",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.Ask_Size,
                                                    QueryComponent = "3",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new RequestComponent()
                                                {
                                                    ComponentType = ComponentType.Bid,
                                                    QueryComponent = "0",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new RequestComponent()
                                                {
                                                    ComponentType = ComponentType.Bid_Size,
                                                    QueryComponent = "1",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
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
                                            CurrencyPairId = currencyPairs[2].Id,
                                            Delay = 86400000,
                                            RequestComponents = new List<RequestComponent>()
                                            {
                                                new RequestComponent()
                                                {
                                                    ComponentType = ComponentType.Ask,
                                                    QueryComponent = "gesmes:Envelope/Cube/Cube/Cube/0=>@rate",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
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
                                            CurrencyPairId = currencyPairs[3].Id,
                                            Delay = 5000,
                                            RequestComponents = new List<RequestComponent>()
                                            {
                                                new RequestComponent()
                                                {
                                                    ComponentType = ComponentType.Ask,
                                                    QueryComponent =
                                                        "['Realtime Currency Exchange Rate']/['5. Exchange Rate']",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
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
                                                    Value = "EUR"
                                                }
                                            }
                                        },
                                        new CurrencyPairRequest()
                                        {
                                            Guid = Guid.NewGuid(),
                                            RequestType = RequestType.HttpGet,
                                            ResponseType = ResponseType.Json,
                                            DataPath = "https://poloniex.com/public?command=returnTicker",
                                            CurrencyPairId = currencyPairs[4].Id,
                                            Delay = 5000,
                                            AnalysedComponents = new List<AnalysedComponent>()
                                            {
                                                // Calculates volume ONLY for this exact Currency pair on this exchange.
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.DailyVolume,
                                                    Delay = 1000,
                                                    UIFormatting = "0 a",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                    Delay = 500,
                                                    UIFormatting = "0[.]00",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                    Delay = 10000,
                                                    UIFormatting = "0[.]00",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent()
                                                {
                                                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                    Delay = 500,
                                                    UIFormatting = "0[.]0",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                }
                                            },
                                            RequestComponents = new List<RequestComponent>()
                                            {
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.VOLUME,
                                                    QueryComponent = "BTC_BCN/baseVolume",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.Ask,
                                                    QueryComponent = "BTC_BCN/lowestAsk",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.Bid,
                                                    QueryComponent = "BTC_BCN/highestBid",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                }
                                            }
                                        },
                                        new CurrencyPairRequest()
                                        {
                                            Guid = Guid.NewGuid(),
                                            RequestType = RequestType.HttpGet,
                                            ResponseType = ResponseType.Json,
                                            DataPath = "https://poloniex.com/public?command=returnTicker",
                                            CurrencyPairId = currencyPairs[5].Id,
                                            Delay = 5000,
                                            AnalysedComponents = new List<AnalysedComponent>()
                                            {
                                                // Calculates volume ONLY for this exact Currency pair on this exchange.
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.DailyVolume,
                                                    Delay = 1000,
                                                    UIFormatting = "0 a",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                    Delay = 500,
                                                    UIFormatting = "0[.]00",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                    Delay = 10000,
                                                    UIFormatting = "0[.]00",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent()
                                                {
                                                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                    Delay = 500,
                                                    UIFormatting = "0[.]0",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                }
                                            },
                                            RequestComponents = new List<RequestComponent>()
                                            {
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.VOLUME,
                                                    QueryComponent = "BTC_BTS/baseVolume",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.Ask,
                                                    QueryComponent = "BTC_BTS/lowestAsk",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.Bid,
                                                    QueryComponent = "BTC_BTS/highestBid",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                }
                                            }
                                        },
                                        new CurrencyPairRequest()
                                        {
                                            Guid = Guid.NewGuid(),
                                            RequestType = RequestType.HttpGet,
                                            ResponseType = ResponseType.Json,
                                            DataPath = "https://api.bitfinex.com/v1/pubticker/etheur",
                                            CurrencyPairId = currencyPairs[6].Id,
                                            Delay = 2000,
                                            AnalysedComponents = new List<AnalysedComponent>()
                                            {
                                                // Calculates volume ONLY for this exact Currency pair on this exchange.
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.DailyVolume,
                                                    Delay = 1000,
                                                    UIFormatting = "0 a",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                    Delay = 500,
                                                    UIFormatting = "0[.]00",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                    Delay = 10000,
                                                    UIFormatting = "0[.]00",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent()
                                                {
                                                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                    Delay = 500,
                                                    UIFormatting = "0[.]0",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                }
                                            },
                                            RequestComponents = new List<RequestComponent>()
                                            {
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.VOLUME,
                                                    QueryComponent = "volume",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.Ask,
                                                    QueryComponent = "ask",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new RequestComponent()
                                                {
                                                    ComponentType = ComponentType.Bid,
                                                    QueryComponent = "bid",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                }
                                            }
                                        },
                                        new CurrencyPairRequest()
                                        {
                                            Guid = Guid.NewGuid(),
                                            RequestType = RequestType.HttpGet,
                                            ResponseType = ResponseType.Json,
                                            DataPath = "https://poloniex.com/public?command=returnTicker",
                                            CurrencyPairId = currencyPairs[7].Id,
                                            Delay = 5000,
                                            AnalysedComponents = new List<AnalysedComponent>
                                            {
                                                // Calculates volume ONLY for this exact Currency pair on this exchange.
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.DailyVolume,
                                                    Delay = 1000,
                                                    UIFormatting = "0 a",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                    Delay = 500,
                                                    UIFormatting = "$ 0[.]00",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent
                                                {
                                                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                                                    Delay = 10000,
                                                    UIFormatting = "$ 0[.]00",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new AnalysedComponent()
                                                {
                                                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                    Delay = 500,
                                                    UIFormatting = "0[.]0",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                }
                                            },
                                            RequestComponents = new List<RequestComponent>
                                            {
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.Ask,
                                                    QueryComponent = "USDT_BTC/lowestAsk",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                },
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.Bid,
                                                    QueryComponent = "USDT_BTC/highestBid",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                }
                                            }
                                        }
                                    };

                                    context.CurrencyPairRequests.AddRange(currencyPairRequests);

                                    context.SaveChanges();
                                }

                                if (!context.WebsocketRequests.Any())
                                {
                                    // Binance's Websocket-based ticker data stream
                                    var binanceETHBTCWSR = new WebsocketRequest
                                    {
                                        CurrencyPair = new CurrencyPair
                                        {
                                            CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                            APIUrl = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                            DefaultComponent = "b",
                                            CurrencySourceId = bnaSource.Id,
                                            CurrencyPairCurrencies = new List<CurrencyCurrencyPair>
                                            {
                                                new CurrencyCurrencyPair
                                                {
                                                    CurrencyId = ethBna.Id
                                                },
                                                new CurrencyCurrencyPair
                                                {
                                                    CurrencyId = btcBna.Id
                                                }
                                            },
                                            MainCurrency = ethBna.Abbrv,
                                            CounterCurrency = btcBna.Abbrv
                                        },
                                        Guid = Guid.NewGuid(),
                                        RequestType = RequestType.WebSocket,
                                        ResponseType = ResponseType.Json,
                                        DataPath = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                        Delay = 0,
                                        WebsocketCommands = new List<WebsocketCommand>(),
                                        RequestComponents = new List<RequestComponent>()
                                        {
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.VOLUME,
                                                Identifier = "data/s=>ETHBTC",
                                                QueryComponent = "v"
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Ask,
                                                Identifier = "data/s=>ETHBTC",
                                                QueryComponent = "a"
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Ask_Size,
                                                Identifier = "data/s=>ETHBTC",
                                                QueryComponent = "A"
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Bid,
                                                Identifier = "data/s=>ETHBTC",
                                                QueryComponent = "b"
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Bid_Size,
                                                Identifier = "data/s=>ETHBTC",
                                                QueryComponent = "B"
                                            }
                                        }
                                    };

                                    context.WebsocketRequests.Add(binanceETHBTCWSR);

                                    // Binance's Websocket-based ticker data stream
                                    var binanceKNCETHWSR = new WebsocketRequest
                                    {
                                        CurrencyPair = new CurrencyPair
                                        {
                                            CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                            APIUrl = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                            DefaultComponent = "b",
                                            CurrencySourceId = bnaSource.Id,
                                            CurrencyPairCurrencies = new List<CurrencyCurrencyPair>
                                            {
                                                new CurrencyCurrencyPair
                                                {
                                                    CurrencyId = kncBna.Id
                                                },
                                                new CurrencyCurrencyPair
                                                {
                                                    CurrencyId = ethBna.Id
                                                }
                                            },
                                            MainCurrency = kncBna.Abbrv,
                                            CounterCurrency = ethBna.Abbrv
                                        },
                                        Guid = Guid.NewGuid(),
                                        RequestType = RequestType.WebSocket,
                                        ResponseType = ResponseType.Json,
                                        DataPath = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                        Delay = 0,
                                        WebsocketCommands = new List<WebsocketCommand>(),
                                        RequestComponents = new List<RequestComponent>()
                                        {
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.VOLUME,
                                                Identifier = "data/s=>KNCETH",
                                                QueryComponent = "v"
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Ask,
                                                Identifier = "data/s=>KNCETH",
                                                QueryComponent = "a"
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Ask_Size,
                                                Identifier = "data/s=>KNCETH",
                                                QueryComponent = "A"
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Bid,
                                                Identifier = "data/s=>KNCETH",
                                                QueryComponent = "b"
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Bid_Size,
                                                Identifier = "data/s=>KNCETH",
                                                QueryComponent = "B"
                                            }
                                        }
                                    };

                                    context.WebsocketRequests.Add(binanceKNCETHWSR);

                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}