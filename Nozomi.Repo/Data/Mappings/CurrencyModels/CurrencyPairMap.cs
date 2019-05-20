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

            entityTypeBuilder.HasOne(cp => cp.Source).WithMany(cs => cs.CurrencyPairs)
                .HasForeignKey(cp => cp.SourceId)
                .HasConstraintName("CurrencyPairs_CurrencySource_Constraint");
            entityTypeBuilder.HasMany(cp => cp.AnalysedComponents).WithOne(ac => ac.CurrencyPair)
                .HasForeignKey(ac => ac.CurrencyPairId).OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
            entityTypeBuilder.HasMany(cp => cp.CurrencyPairRequests).WithOne(cpr => cpr.CurrencyPair)
                .HasForeignKey(cpr => cpr.CurrencyPairId)
                .HasConstraintName("CurrencyPair_CurrencyPairRequest_Constraint");

            entityTypeBuilder.HasData(
                new CurrencyPair()
                {
                    Id = 1,
                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                    APIUrl = "https://api.ethfinex.com/v2/ticker/tETHUSD",
                    DefaultComponent = "0",
                    SourceId = 1,
                    MainCurrencyAbbrv = "ETH",
                    CounterCurrencyAbbrv = "USD"
                },
                new CurrencyPair
                {
                    Id = 2,
                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                    APIUrl = "https://api.ethfinex.com/v2/ticker/tKNCUSD",
                    DefaultComponent = "0",
                    SourceId = 1,
                    MainCurrencyAbbrv = "KNC",
                    CounterCurrencyAbbrv = "USD"
                },
                new CurrencyPair()
                {
                    Id = 3,
                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                    APIUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
                    DefaultComponent = "Cube",
                    SourceId = 4,
                    MainCurrencyAbbrv = "EUR",
                    CounterCurrencyAbbrv = "USD"
                },
                new CurrencyPair
                {
                    Id = 4,
                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                    APIUrl = "https://www.alphavantage.co/query",
                    DefaultComponent = "Realtime Currency Exchange Rate/5. Exchange Rate",
                    SourceId = 5,
                    MainCurrencyAbbrv = "EUR",
                    CounterCurrencyAbbrv = "USD"
                },
                new CurrencyPair
                {
                    Id = 5,
                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                    APIUrl = "https://poloniex.com/public?command=returnTicker",
                    DefaultComponent = "BTC_BCN/lowestAsk",
                    SourceId = 6,
                    MainCurrencyAbbrv = "BTC",
                    CounterCurrencyAbbrv = "BCN"
                },
                new CurrencyPair
                {
                    Id = 6,
                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                    APIUrl = "https://poloniex.com/public?command=returnTicker",
                    DefaultComponent = "BTC_BTS/lowestAsk",
                    SourceId = 6,
                    MainCurrencyAbbrv = "BTC",
                    CounterCurrencyAbbrv = "BTS"
                },
                new CurrencyPair
                {
                    Id = 7,
                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                    APIUrl = "https://api.bitfinex.com/v1/pubticker/etheur",
                    DefaultComponent = "0",
                    SourceId = 1,
                    MainCurrencyAbbrv = "ETH",
                    CounterCurrencyAbbrv = "EUR"
                },
                new CurrencyPair()
                {
                    Id = 8,
                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                    APIUrl = "https://poloniex.com/public?command=returnTicker",
                    DefaultComponent = "USDT_BTC/lowestAsk",
                    SourceId = 6,
                    MainCurrencyAbbrv = "BTC",
                    CounterCurrencyAbbrv = "USDT"
                },
                new CurrencyPair
                {
                    Id = 9,
                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                    APIUrl = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                    DefaultComponent = "b",
                    SourceId = 3,
                    MainCurrencyAbbrv = "ETH",
                    CounterCurrencyAbbrv = "BTC"
                },
                new CurrencyPair
                {
                    Id = 10,
                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                    APIUrl = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                    DefaultComponent = "b",
                    SourceId = 3,
                    MainCurrencyAbbrv = "KNC",
                    CounterCurrencyAbbrv = "ETH"
                },
                new CurrencyPair
                {
                    Id = 11,
                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                    APIUrl = "https://www.coinhako.com/api/v1/price/currency/BTCSGD",
                    DefaultComponent = "data/buy_price",
                    SourceId = 3,
                    MainCurrencyAbbrv = "BTC",
                    CounterCurrencyAbbrv = "SGD"
                },
                new CurrencyPair
                {
                    Id = 12,
                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                    APIUrl = "https://www.coinhako.com/api/v1/price/currency/BTCUSD",
                    DefaultComponent = "data/buy_price",
                    SourceId = 3,
                    MainCurrencyAbbrv = "BTC",
                    CounterCurrencyAbbrv = "USD"
                },
                new CurrencyPair
                {
                    Id = 13,
                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                    APIUrl = "https://www.coinhako.com/api/v1/price/currency/ETHSGD",
                    DefaultComponent = "data/buy_price",
                    SourceId = 3,
                    MainCurrencyAbbrv = "ETH",
                    CounterCurrencyAbbrv = "SGD"
                },
                new CurrencyPair
                {
                    Id = 14,
                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                    APIUrl = "https://www.coinhako.com/api/v1/price/currency/ETHUSD",
                    DefaultComponent = "data/buy_price",
                    SourceId = 3,
                    MainCurrencyAbbrv = "ETH",
                    CounterCurrencyAbbrv = "USD"
                },
                new CurrencyPair
                {
                    Id = 15,
                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                    APIUrl = "https://www.coinhako.com/api/v1/price/currency/LTCSGD",
                    DefaultComponent = "data/buy_price",
                    SourceId = 3,
                    MainCurrencyAbbrv = "LTC",
                    CounterCurrencyAbbrv = "SGD"
                },
                new CurrencyPair
                {
                    Id = 16,
                    CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                    APIUrl = "https://www.coinhako.com/api/v1/price/currency/LTCUSD",
                    DefaultComponent = "data/buy_price",
                    SourceId = 3,
                    MainCurrencyAbbrv = "LTC",
                    CounterCurrencyAbbrv = "USD"
                }
            );
        }
    }
}