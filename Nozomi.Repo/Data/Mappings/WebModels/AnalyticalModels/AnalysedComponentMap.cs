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

            entityTypeBuilder.HasIndex(ac => new { ac.CurrencyId, ac.ComponentType })
                .HasName("AnalysedComponent_Index_CurrencyId_ComponentType").IsUnique();
            entityTypeBuilder.HasIndex(ac => new { ac.CurrencyPairId, ac.ComponentType })
                .HasName("AnalysedComponent_Index_CurrencyPairId_ComponentType").IsUnique();
            entityTypeBuilder.HasIndex(ac => new { ac.CurrencyTypeId, ac.ComponentType })
                .HasName("AnalysedComponent_Index_CurrencyTypeId_ComponentType").IsUnique();

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

            entityTypeBuilder.HasData(
                // ============================ START OF ETHEREUM ============================ //
                new AnalysedComponent
                {
                    Id = 1,
                    ComponentType = AnalysedComponentType.MarketCap,
                    Delay = 3000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 3
                },
                new AnalysedComponent
                {
                    Id = 46,
                    ComponentType = AnalysedComponentType.MarketCapChange,
                    Delay = 3000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 3
                },
                new AnalysedComponent
                {
                    Id = 47,
                    ComponentType = AnalysedComponentType.MarketCapPctChange,
                    Delay = 3000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 3
                },
                new AnalysedComponent
                {
                    Id = 48,
                    ComponentType = AnalysedComponentType.MarketCapHourlyPctChange,
                    Delay = 3000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 3
                },
                // ETH Current Average Price
                new AnalysedComponent
                {
                    Id = 56,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 3000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 3
                },
                // ETH Hourly Average Price
                new AnalysedComponent
                {
                    Id = 57,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 3000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 3
                },
                // ============================ END OF ETHEREUM ============================ //
                
                // ============================ START OF KYBER ============================ //
                new AnalysedComponent
                {
                    Id = 2,
                    ComponentType = AnalysedComponentType.MarketCap,
                    Delay = 3000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 4
                },
                new AnalysedComponent
                {
                    Id = 49,
                    ComponentType = AnalysedComponentType.MarketCapChange,
                    Delay = 3000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 4
                },
                new AnalysedComponent
                {
                    Id = 50,
                    ComponentType = AnalysedComponentType.MarketCapPctChange,
                    Delay = 3000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 4
                },
                new AnalysedComponent
                {
                    Id = 51,
                    ComponentType = AnalysedComponentType.MarketCapHourlyPctChange,
                    Delay = 3000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 4
                },
                // KNC Current Average Price
                new AnalysedComponent
                {
                    Id = 58,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 3000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 4
                },
                // KNC Hourly Average Price
                new AnalysedComponent
                {
                    Id = 59,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 3000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 4
                },
                // ============================ END OF KYBER ============================ //
                
                // ============================ START OF BITCOIN ============================ //
                // BTC Market Cap
                new AnalysedComponent
                {
                    Id = 3,
                    ComponentType = AnalysedComponentType.MarketCap,
                    Delay = 3000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyId = 5
                },
                new AnalysedComponent
                {
                    Id = 52,
                    ComponentType = AnalysedComponentType.MarketCapChange,
                    Delay = 3000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 5
                },
                new AnalysedComponent
                {
                    Id = 53,
                    ComponentType = AnalysedComponentType.MarketCapPctChange,
                    Delay = 3000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 5
                },
                new AnalysedComponent
                {
                    Id = 54,
                    ComponentType = AnalysedComponentType.MarketCapHourlyPctChange,
                    Delay = 3000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 5
                },
                // KNC Current Average Price
                new AnalysedComponent
                {
                    Id = 60,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 3000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 5
                },
                // KNC Hourly Average Price
                new AnalysedComponent
                {
                    Id = 61,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 3000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 5
                },
                // ============================ END OF BITCOIN ============================ //
                
                // Calculates volume ONLY for this exact Currency pair on this exchange.
                new AnalysedComponent
                {
                    Id = 4,
                    ComponentType = AnalysedComponentType.DailyVolume,
                    Delay = 1000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 1
                },
                new AnalysedComponent
                {
                    Id = 5,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 500,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 1
                },
                new AnalysedComponent
                {
                    Id = 6,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 1
                },
                new AnalysedComponent()
                {
                    Id = 7,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 500,
                    UIFormatting = "0[.]0",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 1
                },
                // Calculates volume ONLY for this exact Currency pair on this exchange.
                new AnalysedComponent
                {
                    Id = 8,
                    ComponentType = AnalysedComponentType.DailyVolume,
                    Delay = 1000,
                    UIFormatting = "0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 2
                },
                new AnalysedComponent
                {
                    Id = 9,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 500,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 2
                },
                new AnalysedComponent
                {
                    Id = 10,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 2
                },
                new AnalysedComponent()
                {
                    Id = 11,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 500,
                    UIFormatting = "0[.]0",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 2
                },
                // Calculates volume ONLY for this exact Currency pair on this exchange.
                new AnalysedComponent
                {
                    Id = 12,
                    ComponentType = AnalysedComponentType.DailyVolume,
                    Delay = 1000,
                    UIFormatting = "0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 5
                },
                new AnalysedComponent
                {
                    Id = 13,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 500,
                    UIFormatting = "0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 5
                },
                new AnalysedComponent
                {
                    Id = 14,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 5
                },
                new AnalysedComponent()
                {
                    Id = 15,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 500,
                    UIFormatting = "0[.]0",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 5
                },
                // Calculates volume ONLY for this exact Currency pair on this exchange.
                new AnalysedComponent
                {
                    Id = 16,
                    ComponentType = AnalysedComponentType.DailyVolume,
                    Delay = 1000,
                    UIFormatting = "0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 6
                },
                new AnalysedComponent
                {
                    Id = 17,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 500,
                    UIFormatting = "0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 6
                },
                new AnalysedComponent
                {
                    Id = 18,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 6
                },
                new AnalysedComponent()
                {
                    Id = 19,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 500,
                    UIFormatting = "0[.]0",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 6
                },
                // Calculates volume ONLY for this exact Currency pair on this exchange.
                new AnalysedComponent
                {
                    Id = 20,
                    ComponentType = AnalysedComponentType.DailyVolume,
                    Delay = 1000,
                    UIFormatting = "0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 7
                },
                new AnalysedComponent
                {
                    Id = 21,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 500,
                    UIFormatting = "0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 7
                },
                new AnalysedComponent
                {
                    Id = 22,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 7
                },
                new AnalysedComponent()
                {
                    Id = 23,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 500,
                    UIFormatting = "0[.]0",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 7
                },
                // Calculates volume ONLY for this exact Currency pair on this exchange.
                new AnalysedComponent
                {
                    Id = 24,
                    ComponentType = AnalysedComponentType.DailyVolume,
                    Delay = 1000,
                    UIFormatting = "0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 8
                },
                new AnalysedComponent
                {
                    Id = 25,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 500,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 8
                },
                new AnalysedComponent
                {
                    Id = 26,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 8
                },
                new AnalysedComponent()
                {
                    Id = 27,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 500,
                    UIFormatting = "0[.]0",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 8
                },
                // Coinhako BTCSGD
                new AnalysedComponent
                {
                    Id = 28,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 11
                },
                // Coinhako BTCSGD
                new AnalysedComponent
                {
                    Id = 29,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 11
                },
                // Coinhako BTCSGD
                new AnalysedComponent()
                {
                    Id = 30,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 10000,
                    UIFormatting = "0[.]0",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 11
                },
                // Coinhako BTCUSD
                new AnalysedComponent
                {
                    Id = 31,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 12
                },
                // Coinhako BTCUSD
                new AnalysedComponent
                {
                    Id = 32,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 12
                },
                // Coinhako BTCUSD
                new AnalysedComponent()
                {
                    Id = 33,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 10000,
                    UIFormatting = "0[.]0",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 12
                },
                // Coinhako ETHSGD
                new AnalysedComponent
                {
                    Id = 34,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 13
                },
                // Coinhako ETHSGD
                new AnalysedComponent
                {
                    Id = 35,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 13
                },
                // Coinhako ETHSGD
                new AnalysedComponent()
                {
                    Id = 36,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 10000,
                    UIFormatting = "0[.]0",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 13
                },
                // Coinhako ETHUSD
                new AnalysedComponent
                {
                    Id = 37,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 14
                },
                // Coinhako ETHUSD
                new AnalysedComponent
                {
                    Id = 38,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 14
                },
                // Coinhako ETHUSD
                new AnalysedComponent()
                {
                    Id = 39,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 10000,
                    UIFormatting = "0[.]0",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 14
                },
                // Coinhako LTCSGD
                new AnalysedComponent
                {
                    Id = 40,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 15
                },
                // Coinhako LTCSGD
                new AnalysedComponent
                {
                    Id = 41,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 15
                },
                // Coinhako LTCSGD
                new AnalysedComponent()
                {
                    Id = 42,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 10000,
                    UIFormatting = "0[.]0",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 15
                },
                // Coinhako LTCUSD
                new AnalysedComponent
                {
                    Id = 43,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 16
                },
                // Coinhako LTCUSD
                new AnalysedComponent
                {
                    Id = 44,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 16
                },
                // Coinhako LTCUSD
                new AnalysedComponent()
                {
                    Id = 45,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 10000,
                    UIFormatting = "0[.]0",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyPairId = 16
                },
                // Crypto Market Cap
                new AnalysedComponent
                {
                    Id = 55,
                    ComponentType = AnalysedComponentType.MarketCap,
                    Delay = 3000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    ModifiedAt = new DateTime(2019, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc),
                    DeletedAt = null,
                    CurrencyTypeId = 2
                }
            );
        }
    }
}