using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.CurrencyModels;

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

            entityTypeBuilder.HasData(
                new Source()
                {
                    Id = 1,
                    Abbreviation = "BFX",
                    Name = "Bitfinex",
                    APIDocsURL = "https://docs.bitfinex.com/docs/introduction"
                },
                new Source()
                {
                    Id = 2,
                    Abbreviation = "HAKO",
                    Name = "Coinhako",
                    APIDocsURL = "None"
                },
                new Source()
                {
                    Id = 3,
                    Abbreviation = "BNA",
                    Name = "Binance",
                    APIDocsURL = "https://github.com/binance-exchange/binance-official-api-docs/blob/master/rest-api.md"
                },
                new Source()
                {
                    Id = 4,
                    Abbreviation = "ECB",
                    Name = "European Central Bank",
                    APIDocsURL = "https://www.ecb.europa.eu/stats/policy_and_exchange_rates/euro_reference_exchange_rates/html/index.en.html"
                },
                new Source()
                {
                    Id = 5,
                    Abbreviation = "AVG",
                    Name = "AlphaVantage",
                    APIDocsURL = "https://www.alphavantage.co/documentation/"
                }
            );
        }
    }
}