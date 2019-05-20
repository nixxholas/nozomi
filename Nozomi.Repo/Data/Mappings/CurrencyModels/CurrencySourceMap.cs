using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencySourceMap : BaseMap<CurrencySource>
    {
        public CurrencySourceMap(EntityTypeBuilder<CurrencySource> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(cs => cs.Id).HasName("CurrencySource_PK_Id");
            entityTypeBuilder.Property(cs => cs.Id).ValueGeneratedOnAdd();
            
            entityTypeBuilder.HasIndex(cs => new { cs.CurrencyId, cs.SourceId }).IsUnique()
                .HasName("CurrencySource_CK_CurrencyId_SourceId");

            entityTypeBuilder.HasOne(cs => cs.Currency)
                .WithMany(c => c.CurrencySources).HasForeignKey(cs => cs.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("CurrencySource_Currency_Constraint");
            entityTypeBuilder.HasOne(cs => cs.Source)
                .WithMany(s => s.SourceCurrencies).HasForeignKey(cs => cs.SourceId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("CurrencySource_Source_Constraint");

            entityTypeBuilder.HasData(
                // Bitfinex USD
                new CurrencySource
                {
                    Id = 1,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 1,
                    SourceId = 1
                },
                // Bitfinex EUR
                new CurrencySource
                {
                    Id = 2,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 2,
                    SourceId = 1
                },
                // Bitfinex ETH
                new CurrencySource
                {
                    Id = 3,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 3,
                    SourceId = 1
                },
                // Bitfinex KNC
                new CurrencySource
                {
                    Id = 4,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 4,
                    SourceId = 1
                },
                // Binance KNC
                new CurrencySource
                {
                    Id = 5,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 4,
                    SourceId = 3
                },
                // Binance ETH
                new CurrencySource
                {
                    Id = 6,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 3,
                    SourceId = 3
                },
                // Binance BTC
                new CurrencySource
                {
                    Id = 7,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 5,
                    SourceId = 3
                },
                // European Central Bank EUR
                new CurrencySource
                {
                    Id = 8,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 2,
                    SourceId = 4
                },
                // European Central Bank USD
                new CurrencySource
                {
                    Id = 9,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 1,
                    SourceId = 4
                },
                // AlphaVantage EUR
                new CurrencySource
                {
                    Id = 10,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 2,
                    SourceId = 5
                },
                // AlphaVantage USD
                new CurrencySource
                {
                    Id = 11,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 1,
                    SourceId = 5
                },
                // Poloniex BTC
                new CurrencySource
                {
                    Id = 12,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 5,
                    SourceId = 6
                },
                // Poloniex BCN
                new CurrencySource
                {
                    Id = 13,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 6,
                    SourceId = 6
                },
                // Poloniex BTS
                new CurrencySource
                {
                    Id = 14,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 7,
                    SourceId = 6
                },
                // Poloniex USDT
                new CurrencySource
                {
                    Id = 15,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 8,
                    SourceId = 6
                },
                // Coinhako BTC
                new CurrencySource
                {
                    Id = 16,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 5,
                    SourceId = 2
                },
                // Coinhako SGD
                new CurrencySource
                {
                    Id = 17,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 9,
                    SourceId = 2
                },
                // Coinhako ETH
                new CurrencySource
                {
                    Id = 18,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 6,
                    SourceId = 2
                },
                // Coinhako USD
                new CurrencySource
                {
                    Id = 19,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 1,
                    SourceId = 2
                },
                // Coinhako LTC
                new CurrencySource
                {
                    Id = 20,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    CurrencyId = 10,
                    SourceId = 2
                }
            );
        }
    }
}