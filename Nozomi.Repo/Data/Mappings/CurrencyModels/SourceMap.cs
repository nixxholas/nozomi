using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class SourceMap : BaseMap<Source>
    {
        public SourceMap(EntityTypeBuilder<Source> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(s => s.Id);
            entityTypeBuilder.Property(s => s.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasIndex(s => s.Abbreviation);
            entityTypeBuilder.Property(s => s.Abbreviation).IsRequired();

            entityTypeBuilder.Property(s => s.Name).IsRequired();

            entityTypeBuilder.HasMany(s => s.Currencies).WithOne(c => c.CurrencySource)
                .HasForeignKey(c=> c.CurrencySourceId);
            entityTypeBuilder.HasMany(s => s.CurrencyPairs).WithOne(cp => cp.CurrencySource)
                .HasForeignKey(cp => cp.CurrencySourceId);

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
                }
            );
        }
    }
}