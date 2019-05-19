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
                new CurrencySource
                {
                    CurrencyId = 1,
                    SourceId = 1
                },
                new CurrencySource
                {
                    CurrencyId = 2,
                    SourceId = 1
                },
                new CurrencySource
                {
                    CurrencyId = 3,
                    SourceId = 1
                },
                new CurrencySource
                {
                    CurrencyId = 4,
                    SourceId = 1
                },
                new CurrencySource
                {
                    CurrencyId = 5,
                    SourceId = 3
                },
                new CurrencySource
                {
                    CurrencyId = 6,
                    SourceId = 3
                },
                new CurrencySource
                {
                    CurrencyId = 7,
                    SourceId = 3
                },
                new CurrencySource
                {
                    CurrencyId = 8,
                    SourceId = 4
                },
                new CurrencySource
                {
                    CurrencyId = 9,
                    SourceId = 4
                },
                new CurrencySource
                {
                    CurrencyId = 10,
                    SourceId = 5
                },
                new CurrencySource
                {
                    CurrencyId = 11,
                    SourceId = 5
                },
                new CurrencySource
                {
                    CurrencyId = 12,
                    SourceId = 6
                },
                new CurrencySource
                {
                    CurrencyId = 13,
                    SourceId = 6
                },
                new CurrencySource
                {
                    CurrencyId = 14,
                    SourceId = 6
                },
                new CurrencySource
                {
                    CurrencyId = 15,
                    SourceId = 6
                },
                new CurrencySource
                {
                    CurrencyId = 16,
                    SourceId = 2
                },
                new CurrencySource
                {
                    CurrencyId = 17,
                    SourceId = 2
                },
                new CurrencySource
                {
                    CurrencyId = 18,
                    SourceId = 2
                },
                new CurrencySource
                {
                    CurrencyId = 19,
                    SourceId = 2
                },
                new CurrencySource
                {
                    CurrencyId = 20,
                    SourceId = 2
                }
            );
        }
    }
}