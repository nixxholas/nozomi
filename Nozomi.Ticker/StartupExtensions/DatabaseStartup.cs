using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Data.CurrencyModels;
using Nozomi.Repo.Data;

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
                using (var context = serviceScope.ServiceProvider.GetService<NozomiDbContext>())
                {
                    // Auto Wipe
                    if (env.IsDevelopment())
                    {
                        context.Database.EnsureDeleted();
                    }

                    context.Database.EnsureCreated();

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

                        if (!context.CurrencyPairs.Any())
                        {
                            context.CurrencyPairs.AddRange(
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
                                });

                            context.SaveChanges();
                        }

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
                    }
                }
            }
        }
    }
}