using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyPairMap : BaseMap<CurrencyPair>
    {
        public CurrencyPairMap(EntityTypeBuilder<CurrencyPair> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(cp => cp.Id).HasName("CurrencyPair_PK_Id");
            entityTypeBuilder.Property(cp => cp.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(cp => cp.APIUrl).IsRequired();
            entityTypeBuilder.Property(cp => cp.DefaultComponent).IsRequired();

            entityTypeBuilder.HasOne(cp => cp.CurrencySource).WithMany(cs => cs.CurrencyPairs)
                .HasForeignKey(cp => cp.CurrencySourceId)
                .HasConstraintName("CurrencyPairs_CurrencySource_Constraint");
            entityTypeBuilder.HasMany(cp => cp.CurrencyPairRequests).WithOne(cpr => cpr.CurrencyPair)
                .HasForeignKey(cpr => cpr.CurrencyPairId)
                .HasConstraintName("CurrencyPair_CurrencyPairRequest_Constraint");
            entityTypeBuilder.HasMany(cp => cp.CurrencyPairCurrencies)
                .WithOne(pcp => pcp.CurrencyPair)
                .HasForeignKey(pcp => pcp.CurrencyPairId)
                .HasConstraintName("CurrencyPair_PartialCurrencyPairs_Constraint");

            entityTypeBuilder.HasData(
                new CurrencyPair()
                {
                    Id = 1,
                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                    APIUrl = "https://api.ethfinex.com/v2/ticker/tETHUSD",
                    DefaultComponent = "0",
                    CurrencySourceId = 1,
                    MainCurrency = "ETH",
                    CounterCurrency = "USD",
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
                    Id = 2,
                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                    APIUrl = "https://api.ethfinex.com/v2/ticker/tKNCUSD",
                    DefaultComponent = "0",
                    CurrencySourceId = 1,
                    MainCurrency = "KNC",
                    CounterCurrency = "USD",
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
                    Id = 3,
                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                    APIUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
                    DefaultComponent = "Cube",
                    CurrencySourceId = 4,
                    MainCurrency = "EUR",
                    CounterCurrency = "USD",
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
                    Id = 4,
                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                    APIUrl = "https://www.alphavantage.co/query",
                    DefaultComponent = "Realtime Currency Exchange Rate/5. Exchange Rate",
                    CurrencySourceId = 5,
                    MainCurrency = "EUR",
                    CounterCurrency = "USD",
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
                    Id = 5,
                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                    APIUrl = "https://poloniex.com/public?command=returnTicker",
                    DefaultComponent = "BTC_BCN/lowestAsk",
                    CurrencySourceId = 6,
                    MainCurrency = "BTC",
                    CounterCurrency = "BCN",
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
                    Id = 6,
                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                    APIUrl = "https://poloniex.com/public?command=returnTicker",
                    DefaultComponent = "BTC_BTS/lowestAsk",
                    CurrencySourceId = 6,
                    MainCurrency = "BTC",
                    CounterCurrency = "BTS",
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
                    Id = 7,
                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                    APIUrl = "https://api.bitfinex.com/v1/pubticker/etheur",
                    DefaultComponent = "0",
                    CurrencySourceId = 1,
                    MainCurrency = "ETH",
                    CounterCurrency = "EUR",
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
                    Id = 8,
                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                    APIUrl = "https://poloniex.com/public?command=returnTicker",
                    DefaultComponent = "USDT_BTC/lowestAsk",
                    CurrencySourceId = 6,
                    MainCurrency = "BTC",
                    CounterCurrency = "USDT",
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
            );
        }
    }
}