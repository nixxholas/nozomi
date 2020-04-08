﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyTypeMap : BaseMap<CurrencyType>
    {
        public CurrencyTypeMap(EntityTypeBuilder<CurrencyType> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("ItemTypes");
        
            entityTypeBuilder.HasKey(ct => ct.Id).HasName("ItemType_PK_Id");
            entityTypeBuilder.Property(ct => ct.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasAlternateKey(e => e.Guid);
            entityTypeBuilder.Property(e => e.Guid).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(ct => ct.Name).IsRequired();
            entityTypeBuilder.Property(ct => ct.TypeShortForm).IsRequired();

            entityTypeBuilder.HasMany(ct => ct.AnalysedComponents)
                .WithOne(ac => ac.CurrencyType)
                .HasForeignKey(ac => ac.CurrencyTypeId)
                .HasConstraintName("ItemType_AnalysedComponents_Constraint")
                .IsRequired(false);
            entityTypeBuilder.HasMany(ct => ct.Currencies)
                .WithOne(c => c.CurrencyType)
                .HasForeignKey(c => c.CurrencyTypeId).HasConstraintName("ItemType_Items_Constraint");
            entityTypeBuilder.HasMany(ct => ct.Requests)
                .WithOne(r => r.CurrencyType).HasForeignKey(r => r.CurrencyTypeId)
                .HasConstraintName("ItemType_Requests_Constraint")
                .IsRequired(false);
        }
    }
}