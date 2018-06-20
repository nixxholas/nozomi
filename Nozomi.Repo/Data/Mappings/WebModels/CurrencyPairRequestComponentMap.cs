﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.WebModels;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class CurrencyPairRequestComponentMap
    {
        public CurrencyPairRequestComponentMap(EntityTypeBuilder<CurrencyPairRequestComponent> entityTypeBuilder)
        {
            entityTypeBuilder.Property(cprc => cprc.ComponentType).IsRequired();

            entityTypeBuilder.HasOne(cprc => cprc.Request).WithMany(r => r.RequestComponents)
                .HasForeignKey(cprc => cprc.RequestId);
        }
    }
}