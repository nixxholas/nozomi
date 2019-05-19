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
            
            entityTypeBuilder.HasIndex(cs => new {cs.CurrencyId, cs.SourceId}).IsUnique()
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
                    CurrencyId = 1,
                    SourceId = 1
                },
                // Bitfinex EUR
                new CurrencySource
                {
                    Id = 2,
                    CurrencyId = 2,
                    SourceId = 1
                },
                // Bitfinex ETH
                new CurrencySource
                {
                    Id = 3,
                    CurrencyId = 3,
                    SourceId = 1
                },
                // Bitfinex KNC
                new CurrencySource
                {
                    Id = 4,
                    CurrencyId = 4,
                    SourceId = 1
                },
                // Binance KNC
                new CurrencySource
                {
                    Id = 5,
                    CurrencyId = 5,
                    SourceId = 3
                },
                // Binance ETH
                new CurrencySource
                {
                    Id = 6,
                    CurrencyId = 6,
                    SourceId = 3
                },
                // Binance BTC
                new CurrencySource
                {
                    Id = 7,
                    CurrencyId = 7,
                    SourceId = 3
                },
                // European Central Bank EUR
                new CurrencySource
                {
                    Id = 8,
                    CurrencyId = 2,
                    SourceId = 4
                },
                // European Central Bank USD
                new CurrencySource
                {
                    Id = 9,
                    CurrencyId = 1,
                    SourceId = 4
                },
                // AlphaVantage EUR
                new CurrencySource
                {
                    Id = 10,
                    CurrencyId = 2,
                    SourceId = 5
                },
                // AlphaVantage USD
                new CurrencySource
                {
                    Id = 11,
                    CurrencyId = 1,
                    SourceId = 5
                },
                // Poloniex BTC
                new CurrencySource
                {
                    Id = 12,
                    CurrencyId = 7,
                    SourceId = 6
                },
                // Poloniex BCN
                new CurrencySource
                {
                    Id = 13,
                    CurrencyId = 8,
                    SourceId = 6
                },
                // Poloniex BTS
                new CurrencySource
                {
                    Id = 14,
                    CurrencyId = 9,
                    SourceId = 6
                },
                // Poloniex USDT
                new CurrencySource
                {
                    Id = 15,
                    CurrencyId = 10,
                    SourceId = 6
                },
                // Coinhako BTC
                new CurrencySource
                {
                    Id = 16,
                    CurrencyId = 7,
                    SourceId = 2
                },
                // Coinhako SGD
                new CurrencySource
                {
                    Id = 17,
                    CurrencyId = 11,
                    SourceId = 2
                },
                // Coinhako ETH
                new CurrencySource
                {
                    Id = 18,
                    CurrencyId = 6,
                    SourceId = 2
                },
                // Coinhako USD
                new CurrencySource
                {
                    Id = 19,
                    CurrencyId = 1,
                    SourceId = 2
                },
                // Coinhako LTC
                new CurrencySource
                {
                    Id = 20,
                    CurrencyId = 12,
                    SourceId = 2
                }
            );
        }
    }
}