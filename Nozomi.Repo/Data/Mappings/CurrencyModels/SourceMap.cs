﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class SourceMap : BaseMap<Source>
    {
        public SourceMap(EntityTypeBuilder<Source> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(s => s.Id).HasName("Source_PK_Id");
            entityTypeBuilder.Property(s => s.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasAlternateKey(e => e.Guid);
            entityTypeBuilder.Property(e => e.Guid).ValueGeneratedOnAdd();

            entityTypeBuilder.HasIndex(s => s.Abbreviation).HasName("Source_Index_Abbreviation");
            entityTypeBuilder.Property(s => s.Abbreviation).IsRequired();

            entityTypeBuilder.Property(s => s.Name).IsRequired();

            entityTypeBuilder.HasOne(s => s.SourceType)
                .WithMany(st => st.Sources)
                .HasForeignKey(s => s.SourceTypeGuid)
                .IsRequired();
            // entityTypeBuilder.HasMany(s => s.SourceCurrencies).WithOne(c => c.Source)
            //     .HasForeignKey(c=> c.SourceId).OnDelete(DeleteBehavior.Restrict)
            //     .HasConstraintName("Source_SourceCurrencies_Constraint");
            // entityTypeBuilder.HasMany(s => s.CurrencyPairs).WithOne(cp => cp.Source)
            //     .HasForeignKey(cp => cp.SourceId)
            //     .HasConstraintName("Source_CurrencyPairs_Constraint");
        }
    }
}