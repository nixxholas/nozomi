﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class SourceMap : BaseMap<Source>
    {
        public SourceMap(EntityTypeBuilder<Source> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(s => s.Id).HasName("Source_PK_Id");
            entityTypeBuilder.Property(s => s.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasIndex(s => s.Abbreviation).HasName("Source_Index_Abbreviation");
            entityTypeBuilder.Property(s => s.Abbreviation).IsRequired();

            entityTypeBuilder.Property(s => s.Name).IsRequired();

            entityTypeBuilder.HasMany(s => s.Currencies).WithOne(c => c.CurrencySource)
                .HasForeignKey(c=> c.CurrencySourceId)
                .HasConstraintName("Source_Currencies_Constraint");
            entityTypeBuilder.HasMany(s => s.CurrencyPairs).WithOne(cp => cp.CurrencySource)
                .HasForeignKey(cp => cp.CurrencySourceId)
                .HasConstraintName("Source_CurrencyPairs_Constraint");
        }
    }
}