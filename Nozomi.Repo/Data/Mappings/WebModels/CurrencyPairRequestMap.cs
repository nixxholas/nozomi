using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class CurrencyPairRequestMap : BaseMap<CurrencyPairRequest>
    {
        public CurrencyPairRequestMap(EntityTypeBuilder<CurrencyPairRequest> entityTypeBuilder) : base(
            entityTypeBuilder)
        {
            entityTypeBuilder.HasOne(cpr => cpr.CurrencyPair).WithMany(cp => cp.CurrencyPairRequests)
                .HasForeignKey(cpr => cpr.CurrencyPairId);

            entityTypeBuilder.HasData(
                // BFX ETHUSD
                new CurrencyPairRequest()
                {
                    Id = 5,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.ethfinex.com/v2/ticker/tETHUSD",
                    CurrencyPairId = 1,
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
                // BFX KNCUSD
                new CurrencyPairRequest()
                {
                    Id = 6,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.ethfinex.com/v2/ticker/tKNCUSD",
                    CurrencyPairId = 2,
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
                // ECB EURUSD
                new CurrencyPairRequest()
                {
                    Id = 7,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    ResponseType = ResponseType.XML,
                    DataPath = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
                    CurrencyPairId = 3,
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
                // AlphaVantage EURUSD
                new CurrencyPairRequest()
                {
                    Id = 8,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    ResponseType = ResponseType.Json,
                    DataPath = "https://www.alphavantage.co/query",
                    CurrencyPairId = 4,
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
                            Value = "EUR"
                        },
                        new RequestProperty()
                        {
                            RequestPropertyType = RequestPropertyType.HttpQuery,
                            Key = "to_currency",
                            Value = "USD"
                        }
                    }
                },
                // POLO BTCBCN
                new CurrencyPairRequest()
                {
                    Id = 9,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    ResponseType = ResponseType.Json,
                    DataPath = "https://poloniex.com/public?command=returnTicker",
                    CurrencyPairId = 5,
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
                // POLO BTCBTS
                new CurrencyPairRequest()
                {
                    Id = 10,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    ResponseType = ResponseType.Json,
                    DataPath = "https://poloniex.com/public?command=returnTicker",
                    CurrencyPairId = 6,
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
                // BFX ETHEUR
                new CurrencyPairRequest()
                {
                    Id = 11,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    ResponseType = ResponseType.Json,
                    DataPath = "https://api.bitfinex.com/v1/pubticker/etheur",
                    CurrencyPairId = 7,
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
                // POLO BTCUSDT
                new CurrencyPairRequest()
                {
                    Id = 12,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    ResponseType = ResponseType.Json,
                    DataPath = "https://poloniex.com/public?command=returnTicker",
                    CurrencyPairId = 8,
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
            );
        }
    }
}