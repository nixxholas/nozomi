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
                            },
                            new Source
                            {
                                Abbreviation = "POLO",
                                Name = "Poloniex",
                                APIDocsURL = "https://docs.poloniex.com/#public-http-api-methods"
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
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
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
                                        Name = "Kyber Network Coin",
                                        CurrencySourceId = bfxSource.Id,
                                        WalletTypeId = 4 // As per CNWallet
                                    },
                                    new Currency
                                    {
                                        CurrencyTypeId = cryptoType.Id,
                                        Abbrv = "KNC",
                                        Name = "Kyber Network Coin",
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
                                        DenominationName = "Sat"
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
                                },
                                new CurrencyPair()
                                {
                                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                    APIUrl = "https://poloniex.com/public?command=returnTicker",
                                    DefaultComponent = "BTC_BCN/lowestAsk",
                                    CurrencySourceId = poloSource.Id
                                },
                                new CurrencyPair()
                                {
                                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                    APIUrl = "https://poloniex.com/public?command=returnTicker",
                                    DefaultComponent = "BTC_BTS/lowestAsk",
                                    CurrencySourceId = poloSource.Id
                                },
                                new CurrencyPair
                                {
                                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                                    APIUrl = "https://api.bitfinex.com/v1/pubticker/etheur",
                                    DefaultComponent = "0",
                                    CurrencySourceId = bfxSource.Id
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
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 500,
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 500,
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
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 500,
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 500,
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
                                        DataPath = "https://api.binance.com/api/v3/ticker/bookTicker?symbol=KNCETH",
                                        CurrencyPairId = currencyPairs[2].Id,
                                        Delay = 5000,
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Calculates volume ONLY for this exact Currency pair on this exchange.
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.DailyVolume,
                                                Delay = 1000,
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 500,
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 500,
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            }
                                        },
                                        RequestComponents = new List<RequestComponent>()
                                        {
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Ask,
                                                QueryComponent = "askPrice",
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Bid,
                                                QueryComponent = "bidPrice",
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
                                        CurrencyPairId = currencyPairs[3].Id,
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
                                        CurrencyPairId = currencyPairs[4].Id,
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
                                        CurrencyPairId = currencyPairs[5].Id,
                                        Delay = 5000,
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Calculates volume ONLY for this exact Currency pair on this exchange.
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.DailyVolume,
                                                Delay = 1000,
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 500,
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 500,
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            }
                                        },
                                        RequestComponents = new List<RequestComponent>()
                                        {
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
                                        CurrencyPairId = currencyPairs[6].Id,
                                        Delay = 5000,
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Calculates volume ONLY for this exact Currency pair on this exchange.
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.DailyVolume,
                                                Delay = 1000,
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 500,
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 500,
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            }
                                        },
                                        RequestComponents = new List<RequestComponent>()
                                        {
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
                                        CurrencyPairId = currencyPairs[7].Id,
                                        Delay = 2000,
                                        AnalysedComponents = new List<AnalysedComponent>()
                                        {
                                            // Calculates volume ONLY for this exact Currency pair on this exchange.
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.DailyVolume,
                                                Delay = 1000,
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent
                                            {
                                                ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                                Delay = 500,
                                                CreatedAt = DateTime.UtcNow,
                                                ModifiedAt = DateTime.UtcNow,
                                                DeletedAt = null
                                            },
                                            new AnalysedComponent()
                                            {
                                                ComponentType = AnalysedComponentType.DailyPricePctChange,
                                                Delay = 500,
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
                                    },
                                    new PartialCurrencyPair()
                                    {
                                        CurrencyId = btcPOLO.Id,
                                        IsMain = true,
                                        CurrencyPairId = currencyPairs[5].Id
                                    },
                                    new PartialCurrencyPair()
                                    {
                                        CurrencyId = bcnPOLO.Id,
                                        IsMain = false,
                                        CurrencyPairId = currencyPairs[5].Id
                                    },
                                    new PartialCurrencyPair()
                                    {
                                        CurrencyId = btcPOLO.Id,
                                        IsMain = true,
                                        CurrencyPairId = currencyPairs[6].Id
                                    },
                                    new PartialCurrencyPair()
                                    {
                                        CurrencyId = btsPOLO.Id,
                                        IsMain = false,
                                        CurrencyPairId = currencyPairs[6].Id
                                    },
                                    new PartialCurrencyPair()
                                    {
                                        CurrencyId = ethBfx.Id,
                                        IsMain = true,
                                        CurrencyPairId = currencyPairs[7].Id
                                    },
                                    new PartialCurrencyPair()
                                    {
                                        CurrencyId = eurBfx.Id,
                                        IsMain = false,
                                        CurrencyPairId = currencyPairs[7].Id
                                    });

                                context.SaveChanges();

                                if (!context.CurrencyRequests.Any())
                                {
                                    var currencyRequests = new List<CurrencyRequest>()
                                    {
                                        new CurrencyRequest
                                        {
                                            Guid = Guid.NewGuid(),
                                            RequestType = RequestType.HttpGet,
                                            DataPath = "https://api.etherscan.io/api",
                                            CurrencyId = ethBfx.Id,
                                            Delay = 10000,
                                            RequestProperties = new List<RequestProperty>()
                                            {
                                                new RequestProperty
                                                {
                                                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                                                    Key = "module",
                                                    Value = "stats"
                                                },
                                                new RequestProperty
                                                {
                                                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                                                    Key = "action",
                                                    Value = "ethsupply"
                                                },
                                                new RequestProperty
                                                {
                                                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                                                    Key = "apikey",
                                                    Value = "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224"
                                                },
                                            },
                                            RequestComponents = new List<RequestComponent>()
                                            {
                                                new RequestComponent
                                                {
                                                    ComponentType = ComponentType.Circulating_Supply,
                                                    QueryComponent = "result",
                                                    CreatedAt = DateTime.UtcNow,
                                                    ModifiedAt = DateTime.UtcNow,
                                                    DeletedAt = null
                                                }
                                            }
                                        }
                                    };

                                    context.CurrencyRequests.AddRange(currencyRequests);
                                    context.SaveChanges();
                                }

                                if (!context.WebsocketRequests.Any())
                                {
                                    // Binance's Websocket-based ticker data stream
                                    var binanceWSR = new WebsocketRequest
                                    {
                                        CurrencyPair = new CurrencyPair
                                        {
                                            CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                            APIUrl = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                            DefaultComponent = "b",
                                            CurrencySourceId = bnaSource.Id,
                                            PartialCurrencyPairs = new List<PartialCurrencyPair>()
                                            {
                                                new PartialCurrencyPair()
                                                {
                                                    CurrencyId = ethBna.Id,
                                                    IsMain = true
                                                },
                                                new PartialCurrencyPair()
                                                {
                                                    CurrencyId = btcBna.Id,
                                                    IsMain = false,
                                                }
                                            }
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

                                    context.WebsocketRequests.Add(binanceWSR);

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