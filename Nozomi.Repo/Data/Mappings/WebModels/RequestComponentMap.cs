using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Core.Helpers.Mapping;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.WebModels;

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
            
            entityTypeBuilder.Property(rc => rc.QueryComponent).IsRequired(false);
            
            entityTypeBuilder.HasOne(rc => rc.Request).WithMany(r => r.RequestComponents)
                .HasForeignKey(rc => rc.RequestId);
            entityTypeBuilder.HasOne(rc => rc.RequestComponentDatum).WithOne(rcd => rcd.RequestComponent)
                .HasForeignKey<RequestComponentDatum>(rcd => rcd.RequestComponentId);
        }
    }
}