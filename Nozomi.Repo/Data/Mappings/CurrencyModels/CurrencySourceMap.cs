using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Categorisation;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencySourceMap : BaseMap<ItemSource>
    {
        public CurrencySourceMap(EntityTypeBuilder<ItemSource> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(cs => cs.Id).HasName("CurrencySource_PK_Id");
            entityTypeBuilder.Property(cs => cs.Id).ValueGeneratedOnAdd();
            
            entityTypeBuilder.HasIndex(cs => new { CurrencyId = cs.ItemId, cs.SourceId }).IsUnique()
                .HasName("CurrencySource_CK_CurrencyId_SourceId");

            // entityTypeBuilder.HasOne(cs => cs.Currency)
            //     .WithMany(c => c.CurrencySources).HasForeignKey(cs => cs.CurrencyId)
            //     .OnDelete(DeleteBehavior.Restrict)
            //     .HasConstraintName("CurrencySource_Currency_Constraint");
            // entityTypeBuilder.HasOne(cs => cs.Source)
            //     .WithMany(s => s.SourceCurrencies).HasForeignKey(cs => cs.SourceId)
            //     .OnDelete(DeleteBehavior.Restrict)
            //     .HasConstraintName("CurrencySource_Source_Constraint");
        }
    }
}