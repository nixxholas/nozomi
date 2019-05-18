using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class RequestComponentMap : BaseMap<RequestComponent>
    {
        public RequestComponentMap(EntityTypeBuilder<RequestComponent> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(rc => rc.Id).HasName("RequestComponent_PK_Id");
            entityTypeBuilder.Property(rc => rc.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasIndex(rc => new {rc.RequestId, rc.ComponentType})
                .HasName("RequestComponent_AK_RequestId_ComponentType").IsUnique();

            entityTypeBuilder.Property(rc => rc.Identifier).IsRequired(false);
            entityTypeBuilder.Property(rc => rc.QueryComponent).IsRequired(false);
            entityTypeBuilder.Property(rc => rc.IsDenominated).HasDefaultValue(false).IsRequired();
            entityTypeBuilder.Property(rc => rc.AnomalyIgnorance).HasDefaultValue(false).IsRequired();
            
            entityTypeBuilder.HasOne(rc => rc.Request).WithMany(r => r.RequestComponents)
                .HasForeignKey(rc => rc.RequestId).IsRequired();
            entityTypeBuilder.HasMany(rc => rc.RcdHistoricItems).WithOne(rcd => rcd.RequestComponent)
                .HasForeignKey(rcd => rcd.RequestComponentId);

            entityTypeBuilder.HasData(
                // ETH BFX Etherscan Request for Circulating Supply
                new RequestComponent
                {
                    Id = 1,
                    ComponentType = ComponentType.Circulating_Supply,
                    Identifier = null,
                    IsDenominated = true,
                    QueryComponent = "result",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 1
                },
                // KNC BFX Etherscan Request for Circulating Supply
                new RequestComponent
                {
                    Id = 2,
                    ComponentType = ComponentType.Circulating_Supply,
                    Identifier = null,
                    IsDenominated = true,
                    QueryComponent = "result",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 2
                },
                // POLO BTC Bitpay Insight for Blockcount
                new RequestComponent
                {
                    Id = 3,
                    ComponentType = ComponentType.BlockCount,
                    Identifier = null,
                    QueryComponent = "info/blocks",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 3
                },
                // POLO BTC Bitpay Insight for network difficulty
                new RequestComponent
                {
                    Id = 4,
                    ComponentType = ComponentType.Difficulty,
                    Identifier = null,
                    QueryComponent = "info/difficulty",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 3
                },
                // POLO BTC Coinranking for circulating supply
                new RequestComponent
                {
                    Id = 5,
                    ComponentType = ComponentType.Circulating_Supply,
                    Identifier = null,
                    QueryComponent = "data/coin/circulatingSupply",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 4
                },
                new RequestComponent
                {
                    Id = 6,
                    ComponentType = ComponentType.VOLUME,
                    Identifier = null,
                    QueryComponent = "7",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 5
                },
                new RequestComponent
                {
                    Id = 7,
                    ComponentType = ComponentType.Ask,
                    Identifier = null,
                    QueryComponent = "2",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 5
                },
                new RequestComponent
                {
                    Id = 8,
                    ComponentType = ComponentType.Ask_Size,
                    Identifier = null,
                    QueryComponent = "3",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 5
                },
                new RequestComponent()
                {
                    Id = 9,
                    ComponentType = ComponentType.Bid,
                    Identifier = null,
                    QueryComponent = "0",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 5
                },
                new RequestComponent()
                {
                    Id = 10,
                    ComponentType = ComponentType.Bid_Size,
                    Identifier = null,
                    QueryComponent = "1",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 5
                },
                new RequestComponent
                {
                    Id = 11,
                    ComponentType = ComponentType.VOLUME,
                    Identifier = null,
                    QueryComponent = "7",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 6
                },
                new RequestComponent
                {
                    Id = 12,
                    ComponentType = ComponentType.Ask,
                    Identifier = null,
                    QueryComponent = "2",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 6
                },
                new RequestComponent
                {
                    Id = 13,
                    ComponentType = ComponentType.Ask_Size,
                    Identifier = null,
                    QueryComponent = "3",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 6
                },
                new RequestComponent()
                {
                    Id = 14,
                    ComponentType = ComponentType.Bid,
                    Identifier = null,
                    QueryComponent = "0",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 6
                },
                new RequestComponent()
                {
                    Id = 15,
                    ComponentType = ComponentType.Bid_Size,
                    Identifier = null,
                    QueryComponent = "1",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 6
                },
                new RequestComponent()
                {
                    Id = 16,
                    ComponentType = ComponentType.Ask,
                    Identifier = null,
                    QueryComponent = "gesmes:Envelope/Cube/Cube/Cube/0=>@rate",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 7
                },
                new RequestComponent()
                {
                    Id = 17,
                    ComponentType = ComponentType.Ask,
                    Identifier = null,
                    QueryComponent = "['Realtime Currency Exchange Rate']/['5. Exchange Rate']",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 8
                },
                new RequestComponent
                {
                    Id = 18,
                    ComponentType = ComponentType.VOLUME,
                    Identifier = null,
                    QueryComponent = "BTC_BCN/baseVolume",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 9
                },
                new RequestComponent
                {
                    Id = 19,
                    ComponentType = ComponentType.Ask,
                    Identifier = null,
                    QueryComponent = "BTC_BCN/lowestAsk",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 9
                },
                new RequestComponent
                {
                    Id = 20,
                    ComponentType = ComponentType.Bid,
                    Identifier = null,
                    QueryComponent = "BTC_BCN/highestBid",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 9
                },
                new RequestComponent
                {
                    Id = 21,
                    ComponentType = ComponentType.VOLUME,
                    Identifier = null,
                    QueryComponent = "BTC_BTS/baseVolume",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 10
                },
                new RequestComponent
                {
                    Id = 22,
                    ComponentType = ComponentType.Ask,
                    Identifier = null,
                    QueryComponent = "BTC_BTS/lowestAsk",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 10
                },
                new RequestComponent
                {
                    Id = 23,
                    ComponentType = ComponentType.Bid,
                    Identifier = null,
                    QueryComponent = "BTC_BTS/highestBid",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 10
                },
                new RequestComponent
                {
                    Id = 24,
                    ComponentType = ComponentType.VOLUME,
                    Identifier = null,
                    QueryComponent = "volume",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 11
                },
                new RequestComponent
                {
                    Id = 25,
                    ComponentType = ComponentType.Ask,
                    Identifier = null,
                    QueryComponent = "ask",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 11
                },
                new RequestComponent()
                {
                    Id = 26,
                    ComponentType = ComponentType.Bid,
                    Identifier = null,
                    QueryComponent = "bid",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 11
                },
                new RequestComponent
                {
                    Id = 27,
                    ComponentType = ComponentType.Ask,
                    Identifier = null,
                    QueryComponent = "USDT_BTC/lowestAsk",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 12
                },
                new RequestComponent
                {
                    Id = 28,
                    ComponentType = ComponentType.Bid,
                    Identifier = null,
                    QueryComponent = "USDT_BTC/highestBid",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 12
                },
                new RequestComponent
                {
                    Id = 29,
                    ComponentType = ComponentType.VOLUME,
                    Identifier = "data/s=>ETHBTC",
                    QueryComponent = "v",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 13
                },
                new RequestComponent
                {
                    Id = 30,
                    ComponentType = ComponentType.Ask,
                    Identifier = "data/s=>ETHBTC",
                    QueryComponent = "a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 13
                },
                new RequestComponent
                {
                    Id = 31,
                    ComponentType = ComponentType.Ask_Size,
                    Identifier = "data/s=>ETHBTC",
                    QueryComponent = "A",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 13
                },
                new RequestComponent
                {
                    Id = 32,
                    ComponentType = ComponentType.Bid,
                    Identifier = "data/s=>ETHBTC",
                    QueryComponent = "b",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 13
                },
                new RequestComponent
                {
                    Id = 33,
                    ComponentType = ComponentType.Bid_Size,
                    Identifier = "data/s=>ETHBTC",
                    QueryComponent = "B",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 13
                },
                new RequestComponent
                {
                    Id = 34,
                    ComponentType = ComponentType.VOLUME,
                    Identifier = "data/s=>KNCETH",
                    QueryComponent = "v",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 14
                },
                new RequestComponent
                {
                    Id = 35,
                    ComponentType = ComponentType.Ask,
                    Identifier = "data/s=>KNCETH",
                    QueryComponent = "a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 14
                },
                new RequestComponent
                {
                    Id = 36,
                    ComponentType = ComponentType.Ask_Size,
                    Identifier = "data/s=>KNCETH",
                    QueryComponent = "A",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 14
                },
                new RequestComponent
                {
                    Id = 37,
                    ComponentType = ComponentType.Bid,
                    Identifier = "data/s=>KNCETH",
                    QueryComponent = "b",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 14
                },
                new RequestComponent
                {
                    Id = 38,
                    ComponentType = ComponentType.Bid_Size,
                    Identifier = "data/s=>KNCETH",
                    QueryComponent = "B",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 14
                },
                // Coinhako BTCSGD
                new RequestComponent
                {
                    Id = 39,
                    ComponentType = ComponentType.Bid,
                    Identifier = null,
                    QueryComponent = "data/buy_price",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 15
                },
                new RequestComponent
                {
                    Id = 40,
                    ComponentType = ComponentType.Ask,
                    Identifier = null,
                    QueryComponent = "data/sell_price",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 15
                },
                // Coinhako BTCUSD
                new RequestComponent
                {
                    Id = 41,
                    ComponentType = ComponentType.Bid,
                    Identifier = null,
                    QueryComponent = "data/buy_price",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 16
                },
                new RequestComponent
                {
                    Id = 42,
                    ComponentType = ComponentType.Ask,
                    Identifier = null,
                    QueryComponent = "data/sell_price",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 16
                },
                // Coinhako ETHSGD
                new RequestComponent
                {
                    Id = 43,
                    ComponentType = ComponentType.Bid,
                    Identifier = null,
                    QueryComponent = "data/buy_price",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 17
                },
                new RequestComponent
                {
                    Id = 44,
                    ComponentType = ComponentType.Ask,
                    Identifier = null,
                    QueryComponent = "data/sell_price",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 17
                },
                // Coinhako ETHUSD
                new RequestComponent
                {
                    Id = 45,
                    ComponentType = ComponentType.Bid,
                    Identifier = null,
                    QueryComponent = "data/buy_price",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 18
                },
                new RequestComponent
                {
                    Id = 46,
                    ComponentType = ComponentType.Ask,
                    Identifier = null,
                    QueryComponent = "data/sell_price",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 18
                },
                // Coinhako LTCSGD
                new RequestComponent
                {
                    Id = 47,
                    ComponentType = ComponentType.Bid,
                    Identifier = null,
                    QueryComponent = "data/buy_price",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 19
                },
                new RequestComponent
                {
                    Id = 48,
                    ComponentType = ComponentType.Ask,
                    Identifier = null,
                    QueryComponent = "data/sell_price",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 19
                },
                // Coinhako LTCUSD
                new RequestComponent
                {
                    Id = 49,
                    ComponentType = ComponentType.Bid,
                    Identifier = null,
                    QueryComponent = "data/buy_price",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 20
                },
                new RequestComponent
                {
                    Id = 50,
                    ComponentType = ComponentType.Ask,
                    Identifier = null,
                    QueryComponent = "data/sell_price",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 20
                }
            );
        }
    }
}