﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
            
            entityTypeBuilder.Property(rc => rc.QueryComponent).IsRequired(false);
            
            entityTypeBuilder.HasOne(rc => rc.Request).WithMany(r => r.RequestComponents)
                .HasForeignKey(rc => rc.RequestId);
            entityTypeBuilder.HasMany(rc => rc.RequestComponentData).WithOne(rcd => rcd.RequestComponent)
                .HasForeignKey(rcd => rcd.RequestComponentId);

            entityTypeBuilder.HasData(
                new RequestComponent
                {
                    Id = 1,
                    ComponentType = ComponentType.Ask,
                    QueryComponent = "1",
                    RequestId = 1,
                    RequestComponentData = new List<RequestComponentDatum>(),
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    DeletedAt = null
                },
                new RequestComponent()
                {
                    Id = 2,
                    ComponentType = ComponentType.Bid,
                    QueryComponent = "0",
                    RequestId = 1,
                    RequestComponentData = new List<RequestComponentDatum>(),
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    DeletedAt = null
                },
                new RequestComponent
                {
                    Id = 3,
                    ComponentType = ComponentType.Ask,
                    QueryComponent = "0",
                    RequestId = 2,
                    RequestComponentData = new List<RequestComponentDatum>(),
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    DeletedAt = null
                });
        }
    }
}