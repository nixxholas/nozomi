using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyTypeMap
    {
        public CurrencyTypeMap(EntityTypeBuilder<CurrencyType> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(ct => ct.Id);
            entityTypeBuilder.Property(ct => ct.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(ct => ct.Name).IsRequired();
            entityTypeBuilder.Property(ct => ct.TypeShortForm).IsRequired();

            entityTypeBuilder.HasMany(ct => ct.Currencies).WithOne(c => c.CurrencyType)
                .HasForeignKey(c => c.CurrencyTypeId);
        }
    }
}