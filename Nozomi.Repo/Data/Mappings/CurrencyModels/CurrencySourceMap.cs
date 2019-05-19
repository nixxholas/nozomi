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
                    CurrencyId = 1,
                    SourceId = 1
                },
                // Bitfinex EUR
                new CurrencySource
                {
                    CurrencyId = 2,
                    SourceId = 1
                },
                // Bitfinex ETH
                new CurrencySource
                {
                    CurrencyId = 3,
                    SourceId = 1
                },
                // Bitfinex KNC
                new CurrencySource
                {
                    CurrencyId = 4,
                    SourceId = 1
                },
                // Binance KNC
                new CurrencySource
                {
                    CurrencyId = 5,
                    SourceId = 3
                },
                // Binance ETH
                new CurrencySource
                {
                    CurrencyId = 6,
                    SourceId = 3
                },
                // Binance BTC
                new CurrencySource
                {
                    CurrencyId = 7,
                    SourceId = 3
                },
                // European Central Bank EUR
                new CurrencySource
                {
                    CurrencyId = 2,
                    SourceId = 4
                },
                // European Central Bank USD
                new CurrencySource
                {
                    CurrencyId = 1,
                    SourceId = 4
                },
                // AlphaVantage EUR
                new CurrencySource
                {
                    CurrencyId = 2,
                    SourceId = 5
                },
                // AlphaVantage USD
                new CurrencySource
                {
                    CurrencyId = 1,
                    SourceId = 5
                },
                // Poloniex BTC
                new CurrencySource
                {
                    CurrencyId = 7,
                    SourceId = 6
                },
                // Poloniex BCN
                new CurrencySource
                {
                    CurrencyId = 8,
                    SourceId = 6
                },
                // Poloniex BTS
                new CurrencySource
                {
                    CurrencyId = 9,
                    SourceId = 6
                },
                // Poloniex USDT
                new CurrencySource
                {
                    CurrencyId = 10,
                    SourceId = 6
                },
                // Coinhako BTC
                new CurrencySource
                {
                    CurrencyId = 7,
                    SourceId = 2
                },
                // Coinhako SGD
                new CurrencySource
                {
                    CurrencyId = 11,
                    SourceId = 2
                },
                // Coinhako ETH
                new CurrencySource
                {
                    CurrencyId = 6,
                    SourceId = 2
                },
                // Coinhako USD
                new CurrencySource
                {
                    CurrencyId = 1,
                    SourceId = 2
                },
                // Coinhako LTC
                new CurrencySource
                {
                    CurrencyId = 12,
                    SourceId = 2
                }
            );
        }
    }
}