using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyTypeMap : BaseMap<CurrencyType>
    {
        public CurrencyTypeMap(EntityTypeBuilder<CurrencyType> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(ct => ct.Id).HasName("CurrencyType_PK_Id");
            entityTypeBuilder.Property(ct => ct.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(ct => ct.Name).IsRequired();
            entityTypeBuilder.Property(ct => ct.TypeShortForm).IsRequired();

            entityTypeBuilder.HasMany(ct => ct.Currencies).WithOne(c => c.CurrencyType)
                .HasForeignKey(c => c.CurrencyTypeId).HasConstraintName("CurrencyType_Currencies_Constraint");
        }
    }
}