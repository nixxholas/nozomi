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
                    Guid = Guid.Parse("096e9def-1c0f-4d1c-aa7b-273499f2cbda"),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.ethfinex.com/v2/ticker/tETHUSD",
                    CurrencyPairId = 1,
                    Delay = 5000
                },
                // BFX KNCUSD
                new CurrencyPairRequest()
                {
                    Id = 6,
                    Guid = Guid.Parse("534ccff8-b6ff-4cce-961b-8458ef0ca5af"),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.ethfinex.com/v2/ticker/tKNCUSD",
                    CurrencyPairId = 2,
                    Delay = 5000
                },
                // ECB EURUSD
                new CurrencyPairRequest()
                {
                    Id = 7,
                    Guid = Guid.Parse("1d8ba5ea-9d3a-4b02-b2d8-84ccd0851e69"),
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
                    Guid = Guid.Parse("48ad7cb2-b2b7-41be-8540-64136b72883c"),
                    RequestType = RequestType.HttpGet,
                    ResponseType = ResponseType.Json,
                    DataPath = "https://www.alphavantage.co/query",
                    CurrencyPairId = 4,
                    Delay = 5000
                },
                // POLO BTCBCN
                new CurrencyPairRequest()
                {
                    Id = 9,
                    Guid = Guid.Parse("419db9ee-0510-47d1-8b14-620e2c86dcb4"),
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
                    Guid = Guid.Parse("b729acf9-a83c-4e76-8af8-a2ac7efc28c2"),
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
                    Guid = Guid.Parse("ee593665-c6c5-454a-8831-b7e28265a1c8"),
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
                    Guid = Guid.Parse("e47e6062-e727-41ed-a0c1-750b1a792dd7"),
                    RequestType = RequestType.HttpGet,
                    ResponseType = ResponseType.Json,
                    DataPath = "https://poloniex.com/public?command=returnTicker",
                    CurrencyPairId = 8,
                    Delay = 5000
                },
                // Coinhako BTCSGD
                new CurrencyPairRequest()
                {
                    Id = 15,
                    Guid = Guid.Parse("c162e683-cceb-4a03-aa24-f095b4d9db1f"),
                    RequestType = RequestType.HttpGet,
                    ResponseType = ResponseType.Json,
                    DataPath = "https://www.coinhako.com/api/v1/price/currency/BTCSGD",
                    CurrencyPairId = 11,
                    Delay = 10000
                },
                // Coinhako BTCUSD
                new CurrencyPairRequest()
                {
                    Id = 16,
                    Guid = Guid.Parse("fd199860-f699-4414-ba14-fdae9e856b5e"),
                    RequestType = RequestType.HttpGet,
                    ResponseType = ResponseType.Json,
                    DataPath = "https://www.coinhako.com/api/v1/price/currency/BTCUSD",
                    CurrencyPairId = 12,
                    Delay = 10000
                },
                // Coinhako ETHSGD
                new CurrencyPairRequest()
                {
                    Id = 17,
                    Guid = Guid.Parse("49be3d33-d7b8-47aa-abf0-ee8765100b21"),
                    RequestType = RequestType.HttpGet,
                    ResponseType = ResponseType.Json,
                    DataPath = "https://www.coinhako.com/api/v1/price/currency/ETHSGD",
                    CurrencyPairId = 13,
                    Delay = 10000
                },
                // Coinhako ETHUSD
                new CurrencyPairRequest()
                {
                    Id = 18,
                    Guid = Guid.Parse("ceb4e033-ebbb-45d9-9312-951f09228c30"),
                    RequestType = RequestType.HttpGet,
                    ResponseType = ResponseType.Json,
                    DataPath = "https://www.coinhako.com/api/v1/price/currency/ETHUSD",
                    CurrencyPairId = 14,
                    Delay = 10000
                },
                // Coinhako LTCSGD
                new CurrencyPairRequest()
                {
                    Id = 19,
                    Guid = Guid.Parse("58bf3728-1887-4460-bf61-6b898be360f3"),
                    RequestType = RequestType.HttpGet,
                    ResponseType = ResponseType.Json,
                    DataPath = "https://www.coinhako.com/api/v1/price/currency/LTCSGD",
                    CurrencyPairId = 15,
                    Delay = 10000
                },
                // Coinhako LTCUSD
                new CurrencyPairRequest()
                {
                    Id = 20,
                    Guid = Guid.Parse("92121fbb-8f01-45de-bfab-fe17aeac7174"),
                    RequestType = RequestType.HttpGet,
                    ResponseType = ResponseType.Json,
                    DataPath = "https://www.coinhako.com/api/v1/price/currency/LTCUSD",
                    CurrencyPairId = 16,
                    Delay = 10000
                }
            );
        }
    }
}