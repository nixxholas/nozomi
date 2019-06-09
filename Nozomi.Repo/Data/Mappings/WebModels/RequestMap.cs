using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class RequestMap : BaseMap<Request>
    {
        public RequestMap(EntityTypeBuilder<Request> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(r => r.Id).HasName("Request_PK_Id");
            entityTypeBuilder.Property(r => r.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasAlternateKey(r => r.Guid).HasName("Request_AK_Guid");
            entityTypeBuilder.Property(r => r.Guid).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(r => r.Delay).HasDefaultValue(0).IsRequired();
            entityTypeBuilder.Property(r => r.FailureDelay).HasDefaultValue(3600000).IsRequired();
            
            // We need this to determine the type of request to execute with
            entityTypeBuilder.Property(r => r.RequestType).IsRequired();

            entityTypeBuilder.Property(r => r.ResponseType).IsRequired().HasDefaultValue(ResponseType.Json);
            
            // Sometimes, some APIs don't really have a deep declaration requirement
            entityTypeBuilder.Property(r => r.DataPath).IsRequired(false);

            entityTypeBuilder.HasMany(r => r.RequestComponents).WithOne(rc => rc.Request)
                .HasForeignKey(rc => rc.RequestId).OnDelete(DeleteBehavior.Cascade);
            entityTypeBuilder.HasMany(r => r.RequestProperties).WithOne(rp => rp.Request)
                .HasForeignKey(rp => rp.RequestId).OnDelete(DeleteBehavior.Cascade);
            entityTypeBuilder.HasMany(r => r.WebsocketCommands).WithOne(wsc => wsc.Request)
                .HasForeignKey(wsc => wsc.RequestId).OnDelete(DeleteBehavior.Cascade);

            entityTypeBuilder.HasData(
                // ETH Etherscan Request
                new Request
                {
                    Id = 1,
                    CurrencyId = 3,
                    Guid = Guid.Parse("d13fc276-8077-49d2-ba38-998c58895df9"),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.etherscan.io/api",
                    Delay = 5000
                },
                // KNC Etherscan Request
                new Request
                {
                    Id = 2,
                    CurrencyId = 4,
                    Guid = Guid.Parse("b7b9642e-357a-451c-9741-bf5a7fcb0ad1"),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.etherscan.io/api",
                    Delay = 5000
                },
                // BTC Bitpay Insight Request
                new Request
                {
                    Id = 3,
                    CurrencyId = 5,
                    Guid = Guid.Parse("31ceeb18-1d89-43d2-b215-0488d9417c67"),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://insight.bitpay.com/api/status?q=getBlockCount",
                    Delay = 90000
                },
                // BTC Coinranking Request
                new Request
                {
                    Id = 4,
                    CurrencyId = 5,
                    Guid = Guid.Parse("7f10715f-b5cc-4e52-9fa8-011311a5a2ca"),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.coinranking.com/v1/public/coin/1?base=USD",
                    Delay = 90000
                },
                // BFX ETHUSD
                new Request()
                {
                    Id = 5,
                    Guid = Guid.Parse("096e9def-1c0f-4d1c-aa7b-273499f2cbda"),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.ethfinex.com/v2/ticker/tETHUSD",
                    CurrencyPairId = 1,
                    Delay = 5000
                },
                // BFX KNCUSD
                new Request()
                {
                    Id = 6,
                    Guid = Guid.Parse("534ccff8-b6ff-4cce-961b-8458ef0ca5af"),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.ethfinex.com/v2/ticker/tKNCUSD",
                    CurrencyPairId = 2,
                    Delay = 5000
                },
                // ECB EURUSD
                new Request()
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
                new Request()
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
                new Request()
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
                new Request()
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
                new Request()
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
                new Request()
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
                new Request()
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
                new Request()
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
                new Request()
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
                new Request()
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
                new Request()
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
                new Request()
                {
                    Id = 20,
                    Guid = Guid.Parse("92121fbb-8f01-45de-bfab-fe17aeac7174"),
                    RequestType = RequestType.HttpGet,
                    ResponseType = ResponseType.Json,
                    DataPath = "https://www.coinhako.com/api/v1/price/currency/LTCUSD",
                    CurrencyPairId = 16,
                    Delay = 10000
                },
                // Binance's Websocket-based ticker data stream
                new Request
                {
                    Id = 13,
                    CurrencyPairId = 9,
                    Guid = Guid.Parse("6f9d8fe7-71f4-42b8-ac31-526f559549a3"),
                    RequestType = RequestType.WebSocket,
                    ResponseType = ResponseType.Json,
                    DataPath = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                    Delay = 0
                },
                new Request
                {
                    Id = 14,
                    CurrencyPairId = 10,
                    Guid = Guid.Parse("dc33dc82-26e5-4eef-af44-78e1efce2d1f"),
                    RequestType = RequestType.WebSocket,
                    ResponseType = ResponseType.Json,
                    DataPath = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                    Delay = 0
                });
        }
    }
}