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
                .HasForeignKey(rc => rc.RequestId);
            entityTypeBuilder.HasMany(rc => rc.RcdHistoricItems).WithOne(rcd => rcd.RequestComponent)
                .HasForeignKey(rcd => rcd.RequestComponentId);

            entityTypeBuilder.HasData(
                // ETH BFX Etherscan Request for Circulating Supply
                new RequestComponent
                {
                    Id = 1,
                    ComponentType = ComponentType.Circulating_Supply,
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
                    QueryComponent =
                        "['Realtime Currency Exchange Rate']/['5. Exchange Rate']",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 8
                },
                new RequestComponent
                {
                    Id = 18,
                    ComponentType = ComponentType.VOLUME,
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
                    QueryComponent = "USDT_BTC/highestBid",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 12
                }
            );
        }
    }
}