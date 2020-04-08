using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencySourceMap : BaseMap<CurrencySource>
    {
        public CurrencySourceMap(EntityTypeBuilder<CurrencySource> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("ItemSources");
        
            entityTypeBuilder.HasKey(cs => cs.Id).HasName("ItemSource_PK_Id");
            entityTypeBuilder.Property(cs => cs.Id).ValueGeneratedOnAdd();
            
            entityTypeBuilder.HasIndex(cs => new { cs.CurrencyId, cs.SourceId }).IsUnique()
                .HasName("ItemSource_CK_ItemId_SourceId");

            entityTypeBuilder.HasOne(cs => cs.Currency)
                .WithMany(c => c.CurrencySources).HasForeignKey(cs => cs.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("ItemSource_Item_Constraint");
            entityTypeBuilder.HasOne(cs => cs.Source)
                .WithMany(s => s.SourceCurrencies).HasForeignKey(cs => cs.SourceId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("ItemSource_Source_Constraint");
        }
    }
}