using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.WebModels;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyPairComponentMap : BaseMap<CurrencyPairComponent>
    {
        public CurrencyPairComponentMap(EntityTypeBuilder<CurrencyPairComponent> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.Property(cpc => cpc.QueryComponent).IsRequired();

            entityTypeBuilder.HasOne(cpc => cpc.CurrencyPair).WithMany(cp => cp.CurrencyPairComponents)
                .HasForeignKey(cpc => cpc.CurrencyPairId).OnDelete(DeleteBehavior.Cascade);

            entityTypeBuilder.Property(cpc => cpc.ComponentType).IsRequired();

            entityTypeBuilder.HasOne(cpc => cpc.Request).WithMany(r => r.RequestComponents)
                .HasForeignKey(cpc => cpc.RequestId);
            
            entityTypeBuilder.HasData(
                new CurrencyPairComponent()
                {
                    Id = 1,
                    CurrencyPairId = 1,
                    ComponentType = ComponentType.Ask,
                    QueryComponent = "0",
                    RequestId = 1,
                    RequestComponentData = new List<RequestComponentDatum>(),
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    DeletedAt = null
                },
                new CurrencyPairComponent()
                {
                    Id = 2,
                    CurrencyPairId = 2,
                    ComponentType = ComponentType.Ask,
                    QueryComponent = "0",
                    RequestId = 2,
                    RequestComponentData = new List<RequestComponentDatum>(),
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    DeletedAt = null
                }
            );
        }
    }
}