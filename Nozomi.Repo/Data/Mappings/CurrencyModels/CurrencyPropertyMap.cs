using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyPropertyMap : BaseMap<CurrencyProperty>
    {
        public CurrencyPropertyMap(EntityTypeBuilder<CurrencyProperty> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(cp => cp.Id).HasName("CurrencyProperty_PK_Id");
            entityTypeBuilder.Property(cp => cp.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(e => e.Guid).ValueGeneratedOnAdd();
            entityTypeBuilder.HasIndex(e => e.Guid).IsUnique();

            entityTypeBuilder.Property(c => c.Type).IsRequired().HasDefaultValue(CurrencyPropertyType.Generic);
            entityTypeBuilder.Property(c => c.Value).IsRequired(false);

            entityTypeBuilder.HasOne(cp => cp.Currency).WithMany(c => c.CurrencyProperties)
                .HasForeignKey(c => c.CurrencyId).OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("CurrencyProperty_Currency_Constraint");
        }
    }
}