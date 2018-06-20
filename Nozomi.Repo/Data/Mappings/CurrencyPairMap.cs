﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Repo.Data.Mappings
{
    public class CurrencyPairMap
    {
        public CurrencyPairMap(EntityTypeBuilder<CurrencyPair> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(cp => cp.Id);
            entityTypeBuilder.Property(cp => cp.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(cp => cp.APIUrl).IsRequired();
            entityTypeBuilder.Property(cp => cp.DefaultComponent).IsRequired();

            entityTypeBuilder.HasOne(cp => cp.CurrencySource).WithMany(cs => cs.CurrencyPairs)
                .HasForeignKey(cp => cp.CurrencySourceId);
            entityTypeBuilder.HasMany(cp => cp.CurrencyPairComponents).WithOne(cpc => cpc.CurrencyPair)
                .HasForeignKey(cpc => cpc.CurrencyPairId);
            entityTypeBuilder.HasMany(cp => cp.PartialCurrencyPairs).WithOne(pcp => pcp.CurrencyPair)
                .HasForeignKey(pcp => pcp.CurrencyPairId);
        }
    }
}
