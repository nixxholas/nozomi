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
                }
            );
        }
    }
}