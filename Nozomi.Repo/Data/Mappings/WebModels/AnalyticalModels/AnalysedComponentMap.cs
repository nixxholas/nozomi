using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.Models.Analytical;
using Nozomi.Data.Models.Currency;
using Nozomi.Repo.BCL;

namespace Nozomi.Repo.Data.Mappings.WebModels.AnalyticalModels
{
    public class AnalysedComponentMap : BaseMap<AnalysedComponent>
    {
        public AnalysedComponentMap(EntityTypeBuilder<AnalysedComponent> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(ac => ac.Id).HasName("AnalysedComponent_PK_Id");
            entityTypeBuilder.Property(ac => ac.Id)
                .ValueGeneratedOnAdd();

            entityTypeBuilder.HasAlternateKey(r => r.Guid);
            entityTypeBuilder.Property(r => r.Guid).HasDefaultValueSql("uuid_generate_v4()");

            entityTypeBuilder.HasIndex(ac => new { ac.CurrencyId, ac.ComponentType })
                .HasName("AnalysedComponent_Index_CurrencyId_ComponentType").IsUnique();
            entityTypeBuilder.HasIndex(ac => new { ac.CurrencyPairId, ac.ComponentType })
                .HasName("AnalysedComponent_Index_CurrencyPairId_ComponentType").IsUnique();
            entityTypeBuilder.HasIndex(ac => new { ac.CurrencyTypeId, ac.ComponentType })
                .HasName("AnalysedComponent_Index_CurrencyTypeId_ComponentType").IsUnique();

            entityTypeBuilder.Property(ac => ac.IsFailing).HasDefaultValue(false);
            entityTypeBuilder.Property(ac => ac.StoreHistoricals).HasDefaultValue(false);
            entityTypeBuilder.Property(ac => ac.UIFormatting).IsRequired(false);
            entityTypeBuilder.Property(ac => ac.Value).IsRequired(false);
            entityTypeBuilder.Property(ac => ac.Delay).HasDefaultValue(86400000); // 24 hours
            entityTypeBuilder.Property(ac => ac.ComponentType).HasDefaultValue(AnalysedComponentType.Unknown);

            entityTypeBuilder.HasOne(ac => ac.Currency)
                .WithMany(c => c.AnalysedComponents).HasForeignKey(ac => ac.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
            entityTypeBuilder.HasOne(ac => ac.CurrencyType)
                .WithMany(ct => ct.AnalysedComponents).HasForeignKey(ac => ac.CurrencyTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
            entityTypeBuilder.HasOne(ac => ac.CurrencyPair)
                .WithMany(r => r.AnalysedComponents).HasForeignKey(ac => ac.CurrencyPairId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
            entityTypeBuilder.HasMany(ac => ac.AnalysedHistoricItems)
                .WithOne(ahi => ahi.AnalysedComponent).HasForeignKey(ac => ac.AnalysedComponentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}