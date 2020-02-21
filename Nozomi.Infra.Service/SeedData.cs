using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nozomi.Base.BCL.Helpers.Enumerator;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Repo.Data;

namespace Nozomi.Service
{
    public static class SeedData
    {
        public static void UseAutoDbMigration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            
            using (var context = serviceScope.ServiceProvider.GetService<NozomiDbContext>())
            {
                #if DEBUG
                if (env.IsDevelopment())
                {
                    context.Database.EnsureDeleted();
                }
                #endif
                
                context.Database.Migrate();
                
                // Component Types
                var genericComponentTypes = 
                    EnumHelper.GetEnumDescriptionsAndValues<GenericComponentType>();
                foreach (var gct in genericComponentTypes)
                {
                    if (!context.ComponentTypes.Any(c => c.Id.Equals(gct.Key)))
                    {
                        var componentType = new ComponentType
                        {
                            Id = gct.Key,
                            Description = gct.Value,
                            // https://stackoverflow.com/questions/16039037/get-the-name-of-enum-value
                            Name = Enum.GetName(typeof(GenericComponentType), gct.Key),
                            Slug = Enum.GetName(typeof(GenericComponentType), gct.Key)
                        };

                        context.ComponentTypes.Add(componentType);
                        context.SaveChanges();
                    }
                }

                // Source Types
                if (!context.SourceTypes.Any(st => st.Abbreviation.Equals("EXC")))
                {
                    var exchangeSourceType = new SourceType
                    {
                        Abbreviation = "EXC",
                        Name = "Exchange"
                    };

                    context.SourceTypes.Add(exchangeSourceType);
                    context.SaveChanges();
                }

                // Sources
                if (!context.Sources.Any(s => s.Abbreviation.Equals("bn")))
                {
                    var source = context.SourceTypes.SingleOrDefault(st => st.Abbreviation.Equals("EXC"));

                    if (source != null)
                    {
                        var binanceSource = new Source
                        {
                            SourceTypeGuid = source.Guid,
                            Abbreviation = "bn",
                            Name = "Binance",
                            APIDocsURL = "https://github.com/binance-exchange/binance-official-api-docs"
                        };

                        context.Sources.Add(binanceSource);
                        context.SaveChanges();
                    }
                }
                
                // Currency Types
                if (!context.CurrencyTypes.Any(ct => ct.TypeShortForm.Equals("CRYPTO")))
                {
                    var cryptocurrencyType = new CurrencyType
                    {
                        TypeShortForm = "CRYPTO",
                        Name = "Cryptocurrencies"
                    };

                    context.CurrencyTypes.Add(cryptocurrencyType);
                    context.SaveChanges();
                }
                
                // Currencies and its bindings
                if (!context.Currencies.Any(c => c.Slug.Equals("btc")))
                {
                    var type = context.CurrencyTypes
                        .SingleOrDefault(ct => ct.TypeShortForm.Equals("CRYPTO"));

                    if (type != null)
                    {
                        var bitcoin = new Currency
                        {
                            CurrencyTypeId = type.Id,
                            Abbreviation = "BTC",
                            LogoPath = "/assets/svg/icons/btc.svg",
                            Name = "Bitcoin",
                            Description = "Bitcoin (BTC) is a consensus network that enables a new payment system " +
                                          "and a completely digital currency. Powered by its users, it is a peer to " +
                                          "peer payment network that requires no central authority to operate. On " +
                                          "October 31st, 2008, an individual or group of individuals operating under " +
                                          "the pseudonym 'Satoshi Nakamoto' published the Bitcoin Whitepaper and " +
                                          "described it as: 'a purely peer-to-peer version of electronic cash, which " +
                                          "would allow online payments to be sent directly from one party to another " +
                                          "without going through a financial institution.'",
                            Slug = "btc"
                        };

                        context.Currencies.Add(bitcoin);
                        context.SaveChanges();

                        var binanceSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("bn"));
                        if (binanceSource != null && !context.CurrencySources
                            .Any(cs => cs.CurrencyId.Equals(bitcoin.Id) 
                                       && cs.SourceId.Equals(binanceSource.Id)))
                        {
                            var btcBinanceSource = new CurrencySource
                            {
                                CurrencyId = bitcoin.Id,
                                SourceId = binanceSource.Id
                            };

                            context.CurrencySources.Add(btcBinanceSource);
                            context.SaveChanges();
                        }
                    }
                }
                
                if (!context.Currencies.Any(c => c.Slug.Equals("usdt")))
                {
                    var type = context.CurrencyTypes
                        .SingleOrDefault(ct => ct.TypeShortForm.Equals("CRYPTO"));

                    if (type != null)
                    {
                        var tether = new Currency
                        {
                            CurrencyTypeId = type.Id,
                            LogoPath = "/assets/svg/icons/usdt.svg",
                            Abbreviation = "USDT",
                            Slug = "usdt",
                            Name = "Tether"
                        };

                        context.Currencies.Add(tether);
                        context.SaveChanges();

                        var binanceSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("bn"));
                        if (binanceSource != null && !context.CurrencySources
                            .Any(cs => cs.CurrencyId.Equals(tether.Id) 
                                       && cs.SourceId.Equals(binanceSource.Id)))
                        {
                            var tetherBinanceSource = new CurrencySource
                            {
                                CurrencyId = tether.Id,
                                SourceId = binanceSource.Id
                            };

                            context.CurrencySources.Add(tetherBinanceSource);
                            context.SaveChanges();
                        }
                    }
                }
                
                if (!context.Currencies.Any(c => c.Slug.Equals("eth")))
                {
                    var type = context.CurrencyTypes
                        .SingleOrDefault(ct => ct.TypeShortForm.Equals("CRYPTO"));

                    if (type != null)
                    {
                        var ethereum = new Currency
                        {
                            CurrencyTypeId = type.Id,
                            LogoPath = "/assets/svg/icons/eth.svg",
                            Abbreviation = "ETH",
                            Slug = "eth",
                            Name = "Ethereum"
                        };

                        context.Currencies.Add(ethereum);
                        context.SaveChanges();

                        var binanceSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("bn"));
                        if (binanceSource != null && !context.CurrencySources
                            .Any(cs => cs.CurrencyId.Equals(ethereum.Id) 
                                       && cs.SourceId.Equals(binanceSource.Id)))
                        {
                            var ethereumBinanceSource = new CurrencySource
                            {
                                CurrencyId = ethereum.Id,
                                SourceId = binanceSource.Id
                            };

                            context.CurrencySources.Add(ethereumBinanceSource);
                            context.SaveChanges();
                        }
                    }
                }
                
                // Currency Pairs
                if (!context.CurrencyPairs // For Binance's BTCUSDT
                    .Include(cp => cp.Source)
                    .Any(cp => cp.MainTicker.Equals("BTC")
                                                     && cp.CounterTicker.Equals("USDT")
                                                     && cp.Source.Abbreviation.Equals("bn")))
                {
                    var binanceSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("bn"));

                    if (binanceSource != null)
                    {
                        var btcusdtBinance = new CurrencyPair
                        {
                            CurrencyPairType = CurrencyPairType.TRADEABLE,
                            DefaultComponent = "b",
                            SourceId = binanceSource.Id,
                            MainTicker = "BTC",
                            CounterTicker = "USDT",
                            AnalysedComponents = new List<AnalysedComponent>()
                            {
                                new AnalysedComponent
                                {
                                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                    IsDenominated = true,
                                    Delay = 0,
                                    UIFormatting = "$ 0[.]000"
                                }
                            },
                            Requests = new List<Request>
                            {
                                new Request
                                {
                                    RequestType = RequestType.WebSocket,
                                    ResponseType = ResponseType.Json,
                                    DataPath = "wss://stream.binance.com:9443/ws/!ticker@arr",
                                    Delay = 0,
                                    FailureDelay = 10000,
                                    RequestComponents = new List<Component>()
                                    {
                                        new Component
                                        {
                                            ComponentTypeId = (long)GenericComponentType.Bid,
                                            Identifier = "s=>BTCUSDT",
                                            QueryComponent = "b",
                                            IsDenominated = true
                                        },
                                        new Component
                                        {
                                            ComponentTypeId = (long)GenericComponentType.Ask,
                                            Identifier = "s=>BTCUSDT",
                                            QueryComponent = "a",
                                            IsDenominated = true
                                        }
                                    }
                                }
                            }
                        };

                        context.CurrencyPairs.Add(btcusdtBinance);
                        context.SaveChanges();
                    }
                }
                
                // Currency Pairs
                if (!context.CurrencyPairs // For Binance's BTCUSDT
                    .Include(cp => cp.Source)
                    .Any(cp => cp.MainTicker.Equals("ETH")
                                                     && cp.CounterTicker.Equals("USDT")
                                                     && cp.Source.Abbreviation.Equals("bn")))
                {
                    var binanceSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("bn"));

                    if (binanceSource != null)
                    {
                        var ethusdtBinance = new CurrencyPair
                        {
                            CurrencyPairType = CurrencyPairType.TRADEABLE,
                            DefaultComponent = "b",
                            SourceId = binanceSource.Id,
                            MainTicker = "ETH",
                            CounterTicker = "USDT",
                            AnalysedComponents = new List<AnalysedComponent>()
                            {
                                new AnalysedComponent
                                {
                                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                                    IsDenominated = true,
                                    Delay = 0,
                                    UIFormatting = "$ 0[.]000"
                                }
                            },
                            Requests = new List<Request>
                            {
                                new Request
                                {
                                    RequestType = RequestType.WebSocket,
                                    ResponseType = ResponseType.Json,
                                    DataPath = "wss://stream.binance.com:9443/ws/ethusdt@bookTicker",
                                    Delay = 0,
                                    FailureDelay = 10000,
                                    RequestComponents = new List<Component>()
                                    {
                                        new Component
                                        {
                                            ComponentTypeId = (long)GenericComponentType.Bid,
                                            // Identifier = "s=>ETHUSDT",
                                            QueryComponent = "b",
                                            IsDenominated = true
                                        },
                                        new Component
                                        {
                                            ComponentTypeId = (long)GenericComponentType.Ask,
                                            // Identifier = "s=>ETHUSDT",
                                            QueryComponent = "a",
                                            IsDenominated = true
                                        }
                                    }
                                }
                            }
                        };

                        context.CurrencyPairs.Add(ethusdtBinance);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}