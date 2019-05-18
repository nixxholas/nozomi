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
                    Delay = 5000
                },
                // BFX KNCUSD
                new CurrencyPairRequest()
                {
                    Id = 6,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.ethfinex.com/v2/ticker/tKNCUSD",
                    CurrencyPairId = 2,
                    Delay = 5000
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
                    Delay = 86400000
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
                    Delay = 5000
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
                    Delay = 5000
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
                    Delay = 2000
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
                    Delay = 5000
                }
            );
        }
    }
}