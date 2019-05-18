using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Repo.Data.Mappings.WebModels.AnalyticalModels
{
    public class AnalysedComponentMap : BaseMap<AnalysedComponent>
    {
        public AnalysedComponentMap(EntityTypeBuilder<AnalysedComponent> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(ac => ac.Id).HasName("AnalysedComponent_PK_Id");
            entityTypeBuilder.Property(ac => ac.Id).ValueGeneratedOnAdd();

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
            entityTypeBuilder.HasOne(ac => ac.Request)
                .WithMany(r => r.AnalysedComponents).HasForeignKey(ac => ac.RequestId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
            entityTypeBuilder.HasMany(ac => ac.AnalysedHistoricItems)
                .WithOne(ahi => ahi.AnalysedComponent).HasForeignKey(ac => ac.AnalysedComponentId)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasData(
                // ETH Bitfinex Market Cap
                new AnalysedComponent
                {
                    Id = 1,
                    ComponentType = AnalysedComponentType.MarketCap,
                    Delay = 1000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 3
                },
                // KNC Bitfinex Market Cap
                new AnalysedComponent
                {
                    Id = 2,
                    ComponentType = AnalysedComponentType.MarketCap,
                    Delay = 1000,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 4
                },
                // BTC POLO Market Cap
                new AnalysedComponent
                {
                    Id = 3,
                    ComponentType = AnalysedComponentType.MarketCap,
                    Delay = 500,
                    UIFormatting = "$ 0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 12
                }
            );
        }
    }
}