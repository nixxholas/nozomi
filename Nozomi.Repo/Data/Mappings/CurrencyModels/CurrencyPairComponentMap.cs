using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyPairComponentMap
    {
        public CurrencyPairComponentMap(EntityTypeBuilder<CurrencyPairComponent> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(cpc => cpc.Id);
            entityTypeBuilder.Property(cpc => cpc.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(cpc => cpc.QueryComponent).IsRequired();
            entityTypeBuilder.Property(cpc => cpc.Value);

            entityTypeBuilder.HasOne(cpc => cpc.CurrencyPair).WithMany(cp => cp.CurrencyPairComponents)
                .HasForeignKey(cpc => cpc.CurrencyPairId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}