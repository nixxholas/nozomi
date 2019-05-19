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
            entityTypeBuilder.HasKey(cs => new {cs.CurrencyId, cs.SourceId})
                .HasName("CurrencySource_CK_CurrencyId_SourceId");

            entityTypeBuilder.HasOne(cs => cs.Currency)
                .WithMany(c => c.CurrencySources).HasForeignKey(cs => cs.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("CurrencySource_Currency_Constraint");
            entityTypeBuilder.HasOne(cs => cs.Source)
                .WithMany(s => s.SourceCurrencies).HasForeignKey(cs => cs.SourceId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("CurrencySource_Source_Constraint");
        }
    }
}