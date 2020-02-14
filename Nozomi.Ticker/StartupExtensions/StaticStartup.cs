using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Repo.Data;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class StaticStartup
    {
        public static void ConfigureStatics(this IApplicationBuilder app, IHostingEnvironment env)
        {
//            using (var serviceScope = app.ApplicationServices
//                .GetRequiredService<IServiceScopeFactory>()
//                .CreateScope())
//            {
//                using (var context = serviceScope.ServiceProvider.GetService<NozomiDbContext>())
//                {
//                }
//            }
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            using (var context = serviceScope.ServiceProvider.GetService<NozomiDbContext>())
            {
                // Auto Wipe
                if (env.IsDevelopment())
                {
                    context.Database.EnsureDeleted();
                }

                context.Database.Migrate();

                var sourceTypes = new List<SourceType>()
                {
                    // UNK is always seeded via .HasData.
                    new SourceType
                    {
                        Guid = Guid.Parse("05b6457d-059c-458c-8774-0811e4d59ea8"),
                        Name = "Unknown",
                        Abbreviation = "UNK",
                        IsEnabled = true
                    },
                    new SourceType
                    {
                        Guid = Guid.Parse("dd09c50c-687e-4895-a46c-e54d5570c668"),
                        Abbreviation = "EXC",
                        Name = "Exchange",
                        IsEnabled = true
                    },
                    new SourceType
                    {
                        Guid = Guid.Parse("a682ca6c-5911-4dcd-9e92-7fd865782707"),
                        Abbreviation = "TRH",
                        Name = "Trading House",
                        IsEnabled = true
                    },
                    new SourceType
                    {
                        Guid = Guid.Parse("a7f1eea1-9fe9-4756-a3b2-4209da68c829"),
                        Abbreviation = "REG",
                        Name = "Regulated Body",
                        IsEnabled = true
                    }
                };
                if (context.SourceTypes.Count() < 2)
                {
                    context.AddRange(sourceTypes);
                    context.SaveChanges();
                }

                var sources = new List<Source>()
                {
                    new Source(Guid.Parse("894c8128-953e-4ead-b5f2-73e90dff4a97"), "BFX", "Bitfinex",
                        "https://docs.bitfinex.com/docs/introduction",
                        context.SourceTypes.SingleOrDefault(st => st.Abbreviation.Equals("EXC")).Guid),
                    new Source(Guid.Parse("088746a6-4e99-471b-b989-adc8c799532b"), "HAKO", "Coinhako", "None",
                        context.SourceTypes.SingleOrDefault(st => st.Abbreviation.Equals("EXC")).Guid),
                    new Source(Guid.Parse("df739166-4db1-4ad2-bfbb-d048d9ca345a"), "BNA", "Binance",
                        "https://github.com/binance-exchange/binance-official-api-docs/blob/master/rest-api.md",
                        context.SourceTypes.SingleOrDefault(st => st.Abbreviation.Equals("EXC")).Guid),
                    new Source(Guid.Parse("e436c827-bb15-4e28-9d35-d7624c08fbf3"), "ECB",
                        "European Central Bank",
                        "https://www.ecb.europa.eu/stats/policy_and_exchange_rates/euro_reference_exchange_rates/html/index.en.html",
                        context.SourceTypes.SingleOrDefault(st => st.Abbreviation.Equals("REG")).Guid),
                    new Source(Guid.Parse("c6fa40f9-c6ee-4192-b8a6-c5878173bcc0"), "AVG", "AlphaVantage",
                        "https://www.alphavantage.co/documentation/",
                        context.SourceTypes.SingleOrDefault(st => st.Abbreviation.Equals("TRH")).Guid
                    ),
                    new Source(Guid.Parse("ae0b0acc-5108-4b44-ac69-c1110992b7d4"), "POLO", "Poloniex",
                        "https://docs.poloniex.com/#public-http-api-methods",
                        context.SourceTypes.SingleOrDefault(st => st.Abbreviation.Equals("EXC")).Guid)
                };
                if (!context.Sources.Any())
                {
                    context.AddRange(sources);
                    context.SaveChanges();
                }

                if (!context.CurrencyTypes.Any())
                {
                    context.CurrencyTypes.AddRange(
                        new CurrencyType()
                        {
                            Guid = Guid.Parse("f39f5c10-be6e-4dc3-906e-a9e69fa2c380"),
                            TypeShortForm = "FIAT",
                            Name = "FIAT Cash"
                        },
                        new CurrencyType()
                        {
                            Guid = Guid.Parse("b3044801-5506-4d91-b0c0-13e6e84badc5"),
                            TypeShortForm = "CRYPTO",
                            Name = "Cryptocurrency"
                        });

                    context.SaveChanges();
                }

                if (context.Sources.Any())
                {
                    var bfxSource = sources.SingleOrDefault(s => s.Abbreviation.Equals("BFX"));
                    var bnaSource = sources.SingleOrDefault(s => s.Abbreviation.Equals("BNA"));
                    var ecbSource = sources.SingleOrDefault(s => s.Abbreviation.Equals("ECB"));
                    var hakoSource = sources.SingleOrDefault(s => s.Abbreviation.Equals("HAKO"));
                    var avgSource = sources.SingleOrDefault(s => s.Abbreviation.Equals("AVG"));
                    var poloSource = sources.SingleOrDefault(s => s.Abbreviation.Equals("POLO"));

                    if (!context.Currencies.Any() && context.CurrencyTypes.Any())
                    {
                        var fiatType = context.CurrencyTypes.SingleOrDefault(ct => ct.TypeShortForm.Equals("FIAT"));
                        var cryptoType =
                            context.CurrencyTypes.SingleOrDefault(ct => ct.TypeShortForm.Equals("CRYPTO"));

                        if (fiatType != null && cryptoType != null && bfxSource != null && bnaSource != null
                            && ecbSource != null && avgSource != null)
                            context.Currencies.AddRange(
                                new Currency(fiatType.Id, null, "USD", "usd", 
                                    "United States Dollar", null, 8, null)
                                {
                                    CurrencySources = new List<CurrencySource>()
                                    {
                                        new CurrencySource(avgSource.Id),
                                        new CurrencySource(bfxSource.Id),
                                        new CurrencySource(ecbSource.Id)
                                    }
                                },
                                new Currency(fiatType.Id, null, "EUR", "eur", 
                                    "Euro", null, 8, null)
                                {
                                    CurrencySources = new List<CurrencySource>()
                                    {
                                        new CurrencySource(avgSource.Id),
                                        new CurrencySource(ecbSource.Id),
                                        new CurrencySource(bfxSource.Id)
                                    }
                                },
                                new Currency(cryptoType.Id, "assets/svg/icons/eth.svg", "ETH", 
                                    "eth", "Ethereum", null, 12, 
                                    "Wei")
                                {
                                    CurrencySources = new List<CurrencySource>()
                                    {
                                        new CurrencySource(bfxSource.Id),
                                        new CurrencySource(bnaSource.Id)
                                    },
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
                                            IsDenominated = true,
                                            DeletedAt = null
                                        }
                                    },
                                    Requests = new List<Request>
                                    {
                                        new Request
                                        {
                                            Guid = Guid.NewGuid(),
                                            RequestType = RequestType.HttpGet,
                                            DataPath = "https://api.etherscan.io/api",
                                            Delay = 5000,
                                            RequestComponents = new List<Component>
                                            {
                                                new Component(ComponentType.CirculatingSupply, null,
                                                    "result", false, true,
                                                    false)
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
                                new Currency(cryptoType.Id, "assets/svg/icons/knc-2018.svg", "KNC", 
                                    "knc", "Kyber Network Crystal", null, 12, 
                                    null)
                                {
                                    CurrencySources = new List<CurrencySource>()
                                    {
                                        new CurrencySource(bfxSource.Id)
                                    },
                                    Denominations = 18,
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
                                    Requests = new List<Request>
                                    {
                                        new Request
                                        {
                                            Guid = Guid.NewGuid(),
                                            RequestType = RequestType.HttpGet,
                                            DataPath = "https://api.etherscan.io/api",
                                            Delay = 5000,
                                            RequestComponents = new List<Component>
                                            {
                                                new Component(ComponentType.CirculatingSupply, null,
                                                    "result", true, true,
                                                    false)
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
                                new Currency(cryptoType.Id, "assets/svg/icons/btc.svg", "BTC", 
                                    "btc", "Bitcoin", null, 9, 
                                    "Satoshi")
                                {
                                    CurrencySources = new List<CurrencySource>()
                                    {
                                        new CurrencySource(bfxSource.Id),
                                        new CurrencySource(bnaSource.Id),
                                        new CurrencySource(hakoSource.Id),
                                        new CurrencySource(poloSource.Id)
                                    },
                                    Denominations = 8,
                                    DenominationName = "Sat",
                                    AnalysedComponents = new List<AnalysedComponent>()
                                    {
                                        new AnalysedComponent
                                        {
                                            ComponentType = AnalysedComponentType.MarketCap,
                                            Delay = 500,
                                            CreatedAt = DateTime.UtcNow,
                                            ModifiedAt = DateTime.UtcNow,
                                            DeletedAt = null
                                        }
                                    },
                                    Requests = new List<Request>()
                                    {
                                        new Request
                                        {
                                            Guid = Guid.NewGuid(),
                                            RequestType = RequestType.HttpGet,
                                            DataPath = "https://insight.bitpay.com/api/status?q=getBlockCount",
                                            Delay = 90000,
                                            RequestComponents = new List<Component>
                                            {
                                                new Component(ComponentType.BlockCount, null,
                                                    "info/blocks", false,
                                                    false, false),
                                                new Component(ComponentType.Difficulty, null, "info/difficulty", false,
                                                    false, false)
                                            }
                                        },
                                        new Request()
                                        {
                                            Guid = Guid.NewGuid(),
                                            RequestType = RequestType.HttpGet,
                                            DataPath = "https://api.coinranking.com/v1/public/coin/1?base=USD",
                                            Delay = 90000,
                                            RequestComponents = new List<Component>
                                            {
                                                new Component(ComponentType.CirculatingSupply, null,
                                                    "data/coin/circulatingSupply", false,
                                                    false, false)
                                            }
                                        }
                                    }
                                },
                                new Currency(cryptoType.Id, "assets/svg/icons/bcn.svg", "BCN", 
                                    "bcn", "Bytecoin", null, 9, 
                                    null)
                                {
                                    CurrencyTypeId = cryptoType.Id,
                                    Abbreviation = "BCN",
                                    Name = "Bytecoin",
                                    CurrencySources = new List<CurrencySource>()
                                    {
                                        new CurrencySource(poloSource.Id)
                                    }
                                },
                                new Currency(cryptoType.Id, "assets/svg/icons/bts.svg", "BTS", 
                                    "bts", "Bitshares", null, 9, 
                                    null)
                                {
                                    CurrencySources = new List<CurrencySource>()
                                    {
                                        new CurrencySource(poloSource.Id)
                                    }
                                },
                                new Currency(cryptoType.Id, "assets/svg/icons/usdt.svg", "USDT", 
                                    "usdt", "Tether", null, 9, 
                                    null)
                                {
                                    CurrencySources = new List<CurrencySource>()
                                    {
                                        new CurrencySource(bfxSource.Id),
                                        new CurrencySource(bnaSource.Id),
                                        new CurrencySource(hakoSource.Id),
                                        new CurrencySource(poloSource.Id)
                                    }
                                }
                            );

                        context.SaveChanges();
                    }

                    if (!context.CurrencyPairs.Any())
                    {
                        var currencyPairs = new List<CurrencyPair>()
                        {
                            new CurrencyPair(CurrencyPairType.TRADEABLE, "ETH", 
                                "USD", 
                                "https://api-pub.bitfinex.com/v2/tickers?symbols=ALL", 
                                "0=>tETHUSD", 
                                bfxSource.Id)
                            {
                                AnalysedComponents = new List<AnalysedComponent>()
                                {
                                    new AnalysedComponent(AnalysedComponentType.CurrentAveragePrice, 1000,
                                        "$ 0[.]00", true, false),
                                    new AnalysedComponent(AnalysedComponentType.HourlyPricePctChange, 3600000,
                                        "$ 0[.]00", true, false),
                                    new AnalysedComponent(AnalysedComponentType.DailyVolume, 86400000,
                                        "$ 0[.]00", true, false)
                                },
                                Requests = new List<Request>()
                                {
                                    new Request(RequestType.HttpGet, ResponseType.Json,
                                        "https://api-pub.bitfinex.com/v2/tickers?symbols=ALL", 1000,
                                        5000)
                                    {
                                        RequestComponents = new List<Component>()
                                        {
                                            new Component(ComponentType.Ask, $"0=>tETHUSD", "3",
                                                false, true, false),
                                            new Component(ComponentType.Bid, $"0=>tETHUSD", "1",
                                                false, true, false),
                                            new Component(ComponentType.DailyVolume, $"0=>tETHUSD", "8",
                                                false, true, false)
                                        }
                                    }
                                }
                            },
                            new CurrencyPair(CurrencyPairType.TRADEABLE, "KNC", 
                                "USD", 
                                "https://api-pub.bitfinex.com/v2/tickers?symbols=ALL", 
                                "0=>tKNCUSD", 
                                bfxSource.Id)
                            {
                                AnalysedComponents = new List<AnalysedComponent>()
                                {
                                    new AnalysedComponent(AnalysedComponentType.CurrentAveragePrice, 1000,
                                        "$ 0[.]00", true, false),
                                    new AnalysedComponent(AnalysedComponentType.HourlyPricePctChange, 3600000,
                                        "$ 0[.]00", true, false),
                                    new AnalysedComponent(AnalysedComponentType.DailyVolume, 86400000,
                                        "$ 0[.]00", true, false)
                                },
                                Requests = new List<Request>()
                                {
                                    new Request(RequestType.HttpGet, ResponseType.Json,
                                        "https://api-pub.bitfinex.com/v2/tickers?symbols=ALL", 1000,
                                        5000)
                                    {
                                        RequestComponents = new List<Component>()
                                        {
                                            new Component(ComponentType.Ask, $"0=>tKNCUSD", "3",
                                                false, true, false),
                                            new Component(ComponentType.Bid, $"0=>tKNCUSD", "1",
                                                false, true, false),
                                            new Component(ComponentType.DailyVolume, $"0=>tKNCUSD", "8",
                                                false, true, false)
                                        }
                                    }
                                }
                            },
                            new CurrencyPair(CurrencyPairType.TRADEABLE, "EUR", 
                                "USD", 
                                "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", 
                                "gesmes:Envelope/Cube/Cube/Cube/0=>@rate", 
                                ecbSource.Id)
                            {
                                Requests = new List<Request>()
                                {
                                    new Request(RequestType.HttpGet, ResponseType.Json,
                                        "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", 86400000,
                                        5000)
                                    {
                                        RequestComponents = new List<Component>()
                                        {
                                            new Component(ComponentType.Ask,
                                                "gesmes:Envelope/Cube/Cube/Cube/0=>@rate",
                                                "3", false, true, false)
                                        }
                                    }
                                }
                            },
                            new CurrencyPair(CurrencyPairType.TRADEABLE, "USD", 
                                "EUR", "Realtime Currency Exchange Rate/5. Exchange Rate", 
                                "['Realtime Currency Exchange Rate']/['5. Exchange Rate']", 
                                avgSource.Id)
                            {
                                AnalysedComponents = new List<AnalysedComponent>()
                                {
                                    new AnalysedComponent(AnalysedComponentType.CurrentAveragePrice, 1000,
                                        "$ 0[.]00", true, false),
                                    new AnalysedComponent(AnalysedComponentType.HourlyPricePctChange, 3600000,
                                        "$ 0[.]00", true, false),
                                    new AnalysedComponent(AnalysedComponentType.DailyVolume, 86400000,
                                        "$ 0[.]00", true, false)
                                },
                                Requests = new List<Request>()
                                {
                                    new Request(RequestType.HttpGet, ResponseType.Json,
                                        "https://www.alphavantage.co/query", 1000,
                                        5000)
                                    {
                                        RequestComponents = new List<Component>
                                        {
                                            new Component(ComponentType.Ask, null,
                                                "['Realtime Currency Exchange Rate']/['5. Exchange Rate']",
                                                false, true, false)
                                        },
                                        RequestProperties = new List<RequestProperty>
                                        {
                                            new RequestProperty
                                            {
                                                RequestPropertyType = RequestPropertyType.HttpQuery,
                                                Key = "apikey",
                                                Value = "TV5HJJHNP8094BRO"
                                            }, new RequestProperty
                                            {
                                                RequestPropertyType = RequestPropertyType.HttpQuery,
                                                Key = "function",
                                                Value = "CURRENCY_EXCHANGE_RATE"
                                            },
                                            new RequestProperty
                                            {
                                                RequestPropertyType = RequestPropertyType.HttpQuery,
                                                Key = "from_currency",
                                                Value = "USD"
                                            },
                                            new RequestProperty
                                            {
                                                RequestPropertyType = RequestPropertyType.HttpQuery,
                                                Key = "to_currency",
                                                Value = "EUR"
                                            }
                                        }
                                    }
                                }
                            },
                            new CurrencyPair(CurrencyPairType.EXCHANGEABLE, "BTC", 
                                "BCN",
                                "https://poloniex.com/public?command=returnTicker", 
                                "BTC_BCN/lowestAsk", poloSource.Id)
                            {
                                AnalysedComponents = new List<AnalysedComponent>()
                                {
                                    // Calculates volume ONLY for this exact Currency pair on this exchange.
                                    new AnalysedComponent(AnalysedComponentType.DailyVolume, 1000,
                                        "0[.]000000000 BCN", true, false),
                                    new AnalysedComponent(AnalysedComponentType.CurrentAveragePrice, 500,
                                        "0[.]000000000 BCN", true, false),
                                    new AnalysedComponent(AnalysedComponentType.DailyPricePctChange, 500,
                                        "00[.]00 %", true, false)
                                },
                                Requests = new List<Request>()
                                {
                                    new Request(RequestType.HttpGet, ResponseType.Json,
                                        "https://poloniex.com/public?command=returnTicker", 1000,
                                        5000)
                                    {
                                        RequestComponents = new List<Component>()
                                        {
                                            new Component(ComponentType.DailyVolume, null,
                                                "BTC_BCN/baseVolume", false,
                                                true, false),
                                            new Component(ComponentType.Ask, null,
                                                "BTC_BCN/lowestAsk", false,
                                                true, false),
                                            new Component(ComponentType.Bid, null,
                                                "BTC_BCN/highestBid", false,
                                                true, false)
                                        }
                                    },
                                }
                            },
                            new CurrencyPair()
                            {
                                CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                APIUrl = "https://poloniex.com/public?command=returnTicker",
                                DefaultComponent = "BTC_BTS/lowestAsk",
                                MainTicker = "BTC",
                                CounterTicker = "BTS",
                                SourceId = poloSource.Id,
                                AnalysedComponents = new List<AnalysedComponent>()
                                {
                                    // Calculates volume ONLY for this exact Currency pair on this exchange.
                                    new AnalysedComponent(AnalysedComponentType.DailyVolume, 1000,
                                        "0[.]000000000 BTS", true, false),
                                    new AnalysedComponent(AnalysedComponentType.CurrentAveragePrice, 500,
                                        "0[.]000000000 BTS", true, false),
                                    new AnalysedComponent(AnalysedComponentType.DailyPricePctChange, 500,
                                        "00[.]00 %", true, false)
                                },
                                Requests = new List<Request>()
                                {
                                    new Request(RequestType.HttpGet, ResponseType.Json,
                                        "https://poloniex.com/public?command=returnTicker", 1000,
                                        5000)
                                    {
                                        RequestComponents = new List<Component>()
                                        {
                                            new Component(ComponentType.DailyVolume, null,
                                                "BTC_BTS/baseVolume", false,
                                                true, false),
                                            new Component(ComponentType.Ask, null,
                                                "BTC_BTS/lowestAsk", false,
                                                true, false),
                                            new Component(ComponentType.Bid, null,
                                                "BTC_BTS/highestBid", false,
                                                true, false)
                                        }
                                    }
                                }
                            },
                            new CurrencyPair(CurrencyPairType.TRADEABLE, "ETH", 
                                "EUR",
                                "https://api-pub.bitfinex.com/v2/tickers?symbols=ALL", 
                                "0", bfxSource.Id)
                            {
                                AnalysedComponents = new List<AnalysedComponent>()
                                {
                                    // Calculates volume ONLY for this exact Currency pair on this exchange.
                                    new AnalysedComponent(AnalysedComponentType.DailyVolume, 1000,
                                        "0[.]00", true, false),
                                    new AnalysedComponent(AnalysedComponentType.CurrentAveragePrice, 500,
                                        "0[.]00", true, false),
                                    new AnalysedComponent(AnalysedComponentType.DailyPricePctChange, 500,
                                        "00[.]00 %", true, false)
                                },
                                Requests = new List<Request>()
                                {
                                    new Request(RequestType.HttpGet, ResponseType.Json,
                                        "https://api-pub.bitfinex.com/v2/tickers?symbols=ALL", 1000,
                                        5000)
                                    {
                                        RequestComponents = new List<Component>()
                                        {
                                            new Component(ComponentType.Ask, $"0=>tETHEUR", "3",
                                                false, true, false),
                                            new Component(ComponentType.Bid, $"0=>tETHUSD", "1",
                                                false, true, false),
                                            new Component(ComponentType.DailyVolume, $"0=>tETHUSD",
                                                "8", false, true,
                                                false)
                                        }
                                    },
                                }
                            },
                            new CurrencyPair(CurrencyPairType.EXCHANGEABLE, "USDT", 
                                "BTC",
                                "https://poloniex.com/public?command=returnTicker", 
                                "USDT_BTC/lowestAsk", poloSource.Id)
                            {
                                AnalysedComponents = new List<AnalysedComponent>
                                {
                                    // Calculates volume ONLY for this exact Currency pair on this exchange.
                                    new AnalysedComponent(AnalysedComponentType.DailyVolume, 1000,
                                        "0[.]000000000 BTC", true, false),
                                    new AnalysedComponent(AnalysedComponentType.CurrentAveragePrice, 500,
                                        "0[.]000000000 BTC", true, false),
                                    new AnalysedComponent(AnalysedComponentType.DailyPricePctChange, 500,
                                        "00[.]00 %", true, false)
                                },
                                Requests = new List<Request>()
                                {
                                    new Request(RequestType.HttpGet, ResponseType.Json,
                                        "https://poloniex.com/public?command=returnTicker", 1000,
                                        5000)
                                    {
                                        RequestComponents = new List<Component>()
                                        {
                                            new Component(ComponentType.DailyVolume, null,
                                                "USDT_BTC/baseVolume", false,
                                                true, false),
                                            new Component(ComponentType.Ask, null,
                                                "USDT_BTC/lowestAsk", false,
                                                true, false),
                                            new Component(ComponentType.Bid, null,
                                                "USDT_BTC/highestBid", false,
                                                true, false)
                                        }
                                    }
                                }
                            }
                        };

                        context.CurrencyPairs.AddRange(currencyPairs);

                        context.SaveChanges();

                        if (context.CurrencyPairs.Any() &&
                            !context.Requests.Any(r => r.RequestType.Equals(RequestType.WebSocket)))
                        {
                            // Binance's Websocket-based ticker data stream
                            var binanceETHBTCWSR = new Request()
                            {
                                CurrencyPair = new CurrencyPair
                                {
                                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                    APIUrl = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                    DefaultComponent = "b",
                                    SourceId = bnaSource.Id,
                                    MainTicker = "ETH",
                                    CounterTicker = "BTC"
                                },
                                Guid = Guid.NewGuid(),
                                RequestType = RequestType.WebSocket,
                                ResponseType = ResponseType.Json,
                                DataPath = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                Delay = 0,
                                WebsocketCommands = new List<WebsocketCommand>(),
                                RequestComponents = new List<Component>()
                                {
                                    new Component
                                    {
                                        ComponentType = ComponentType.DailyVolume,
                                        Identifier = "data/s=>ETHBTC",
                                        QueryComponent = "v"
                                    },
                                    new Component
                                    {
                                        ComponentType = ComponentType.Ask,
                                        Identifier = "data/s=>ETHBTC",
                                        QueryComponent = "a"
                                    },
                                    new Component
                                    {
                                        ComponentType = ComponentType.AskSize,
                                        Identifier = "data/s=>ETHBTC",
                                        QueryComponent = "A"
                                    },
                                    new Component
                                    {
                                        ComponentType = ComponentType.Bid,
                                        Identifier = "data/s=>ETHBTC",
                                        QueryComponent = "b"
                                    },
                                    new Component
                                    {
                                        ComponentType = ComponentType.BidSize,
                                        Identifier = "data/s=>ETHBTC",
                                        QueryComponent = "B"
                                    }
                                }
                            };

                            context.Requests.Add(binanceETHBTCWSR);

                            // Binance's Websocket-based ticker data stream
                            var binanceKNCETHWSR = new Request
                            {
                                CurrencyPair = new CurrencyPair
                                {
                                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                    APIUrl = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                    DefaultComponent = "b",
                                    SourceId = bnaSource.Id,
                                    MainTicker = "KNC",
                                    CounterTicker = "ETH"
                                },
                                Guid = Guid.NewGuid(),
                                RequestType = RequestType.WebSocket,
                                ResponseType = ResponseType.Json,
                                DataPath = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                Delay = 0,
                                WebsocketCommands = new List<WebsocketCommand>(),
                                RequestComponents = new List<Component>()
                                {
                                    new Component
                                    {
                                        ComponentType = ComponentType.DailyVolume,
                                        Identifier = "data/s=>KNCETH",
                                        QueryComponent = "v"
                                    },
                                    new Component
                                    {
                                        ComponentType = ComponentType.Ask,
                                        Identifier = "data/s=>KNCETH",
                                        QueryComponent = "a"
                                    },
                                    new Component
                                    {
                                        ComponentType = ComponentType.AskSize,
                                        Identifier = "data/s=>KNCETH",
                                        QueryComponent = "A"
                                    },
                                    new Component
                                    {
                                        ComponentType = ComponentType.Bid,
                                        Identifier = "data/s=>KNCETH",
                                        QueryComponent = "b"
                                    },
                                    new Component
                                    {
                                        ComponentType = ComponentType.BidSize,
                                        Identifier = "data/s=>KNCETH",
                                        QueryComponent = "B"
                                    }
                                }
                            };

                            context.Requests.Add(binanceKNCETHWSR);

                            context.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}