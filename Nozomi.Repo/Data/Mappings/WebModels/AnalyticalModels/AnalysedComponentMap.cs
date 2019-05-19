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
                new AnalysedComponent
                {
                    Id = 46,
                    ComponentType = AnalysedComponentType.MarketCapChange,
                    Delay = 1000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 3
                },
                new AnalysedComponent
                {
                    Id = 47,
                    ComponentType = AnalysedComponentType.MarketCapPctChange,
                    Delay = 1000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 3
                },
                new AnalysedComponent
                {
                    Id = 48,
                    ComponentType = AnalysedComponentType.MarketCapHourlyPctChange,
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
                    UIFormatting = "$ 0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 4
                },
                new AnalysedComponent
                {
                    Id = 49,
                    ComponentType = AnalysedComponentType.MarketCapChange,
                    Delay = 1000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 4
                },
                new AnalysedComponent
                {
                    Id = 50,
                    ComponentType = AnalysedComponentType.MarketCapPctChange,
                    Delay = 1000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 4
                },
                new AnalysedComponent
                {
                    Id = 51,
                    ComponentType = AnalysedComponentType.MarketCapHourlyPctChange,
                    Delay = 1000,
                    UIFormatting = "$ 0 a",
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
                },
                new AnalysedComponent
                {
                    Id = 52,
                    ComponentType = AnalysedComponentType.MarketCapChange,
                    Delay = 1000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 12
                },
                new AnalysedComponent
                {
                    Id = 53,
                    ComponentType = AnalysedComponentType.MarketCapPctChange,
                    Delay = 1000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 12
                },
                new AnalysedComponent
                {
                    Id = 54,
                    ComponentType = AnalysedComponentType.MarketCapHourlyPctChange,
                    Delay = 1000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDenominated = true,
                    DeletedAt = null,
                    CurrencyId = 12
                },
                // Calculates volume ONLY for this exact Currency pair on this exchange.
                new AnalysedComponent
                {
                    Id = 4,
                    ComponentType = AnalysedComponentType.DailyVolume,
                    Delay = 1000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 5
                },
                new AnalysedComponent
                {
                    Id = 5,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 500,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 5
                },
                new AnalysedComponent
                {
                    Id = 6,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 5
                },
                new AnalysedComponent()
                {
                    Id = 7,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 500,
                    UIFormatting = "0[.]0",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 5
                },
                // Calculates volume ONLY for this exact Currency pair on this exchange.
                new AnalysedComponent
                {
                    Id = 8,
                    ComponentType = AnalysedComponentType.DailyVolume,
                    Delay = 1000,
                    UIFormatting = "0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 6
                },
                new AnalysedComponent
                {
                    Id = 9,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 500,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 6
                },
                new AnalysedComponent
                {
                    Id = 10,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 6
                },
                new AnalysedComponent()
                {
                    Id = 11,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 500,
                    UIFormatting = "0[.]0",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 6
                },
                // Calculates volume ONLY for this exact Currency pair on this exchange.
                new AnalysedComponent
                {
                    Id = 12,
                    ComponentType = AnalysedComponentType.DailyVolume,
                    Delay = 1000,
                    UIFormatting = "0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 9
                },
                new AnalysedComponent
                {
                    Id = 13,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 500,
                    UIFormatting = "0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 9
                },
                new AnalysedComponent
                {
                    Id = 14,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 9
                },
                new AnalysedComponent()
                {
                    Id = 15,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 500,
                    UIFormatting = "0[.]0",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 9
                },
                // Calculates volume ONLY for this exact Currency pair on this exchange.
                new AnalysedComponent
                {
                    Id = 16,
                    ComponentType = AnalysedComponentType.DailyVolume,
                    Delay = 1000,
                    UIFormatting = "0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 10
                },
                new AnalysedComponent
                {
                    Id = 17,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 500,
                    UIFormatting = "0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 10
                },
                new AnalysedComponent
                {
                    Id = 18,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 10
                },
                new AnalysedComponent()
                {
                    Id = 19,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 500,
                    UIFormatting = "0[.]0",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 10
                },
                // Calculates volume ONLY for this exact Currency pair on this exchange.
                new AnalysedComponent
                {
                    Id = 20,
                    ComponentType = AnalysedComponentType.DailyVolume,
                    Delay = 1000,
                    UIFormatting = "0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 11
                },
                new AnalysedComponent
                {
                    Id = 21,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 500,
                    UIFormatting = "0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 11
                },
                new AnalysedComponent
                {
                    Id = 22,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 11
                },
                new AnalysedComponent()
                {
                    Id = 23,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 500,
                    UIFormatting = "0[.]0",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 11
                },
                // Calculates volume ONLY for this exact Currency pair on this exchange.
                new AnalysedComponent
                {
                    Id = 24,
                    ComponentType = AnalysedComponentType.DailyVolume,
                    Delay = 1000,
                    UIFormatting = "0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 12
                },
                new AnalysedComponent
                {
                    Id = 25,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 500,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 12
                },
                new AnalysedComponent
                {
                    Id = 26,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 12
                },
                new AnalysedComponent()
                {
                    Id = 27,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 500,
                    UIFormatting = "0[.]0",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 12
                },
                // Coinhako BTCSGD
                new AnalysedComponent
                {
                    Id = 28,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 15
                },
                // Coinhako BTCSGD
                new AnalysedComponent
                {
                    Id = 29,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 15
                },
                // Coinhako BTCSGD
                new AnalysedComponent()
                {
                    Id = 30,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 10000,
                    UIFormatting = "0[.]0",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 15
                },
                // Coinhako BTCUSD
                new AnalysedComponent
                {
                    Id = 31,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 16
                },
                // Coinhako BTCUSD
                new AnalysedComponent
                {
                    Id = 32,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 16
                },
                // Coinhako BTCUSD
                new AnalysedComponent()
                {
                    Id = 33,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 10000,
                    UIFormatting = "0[.]0",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 16
                },
                // Coinhako ETHSGD
                new AnalysedComponent
                {
                    Id = 34,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 17
                },
                // Coinhako ETHSGD
                new AnalysedComponent
                {
                    Id = 35,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 17
                },
                // Coinhako ETHSGD
                new AnalysedComponent()
                {
                    Id = 36,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 10000,
                    UIFormatting = "0[.]0",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 17
                },
                // Coinhako ETHUSD
                new AnalysedComponent
                {
                    Id = 37,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 18
                },
                // Coinhako ETHUSD
                new AnalysedComponent
                {
                    Id = 38,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 18
                },
                // Coinhako ETHUSD
                new AnalysedComponent()
                {
                    Id = 39,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 10000,
                    UIFormatting = "0[.]0",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 18
                },
                // Coinhako LTCSGD
                new AnalysedComponent
                {
                    Id = 40,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 19
                },
                // Coinhako LTCSGD
                new AnalysedComponent
                {
                    Id = 41,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 19
                },
                // Coinhako LTCSGD
                new AnalysedComponent()
                {
                    Id = 42,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 10000,
                    UIFormatting = "0[.]0",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 19
                },
                // Coinhako LTCUSD
                new AnalysedComponent
                {
                    Id = 43,
                    ComponentType = AnalysedComponentType.CurrentAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 20
                },
                // Coinhako LTCUSD
                new AnalysedComponent
                {
                    Id = 44,
                    ComponentType = AnalysedComponentType.HourlyAveragePrice,
                    Delay = 10000,
                    UIFormatting = "$ 0[.]00",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 20
                },
                // Coinhako LTCUSD
                new AnalysedComponent()
                {
                    Id = 45,
                    ComponentType = AnalysedComponentType.DailyPricePctChange,
                    Delay = 10000,
                    UIFormatting = "0[.]0",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    RequestId = 20
                },
                // Crypto Market Cap
                new AnalysedComponent
                {
                    Id = 55,
                    ComponentType = AnalysedComponentType.MarketCap,
                    Delay = 3000,
                    UIFormatting = "$ 0 a",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyTypeId = 2
                }
            );
        }
    }
}