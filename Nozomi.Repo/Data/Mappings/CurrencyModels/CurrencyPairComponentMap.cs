using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyPairComponentMap : BaseMap<CurrencyPairComponent>
    {
        public CurrencyPairComponentMap(EntityTypeBuilder<CurrencyPairComponent> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.Property(cpc => cpc.QueryComponent).IsRequired();
            entityTypeBuilder.Property(cpc => cpc.Value);

            entityTypeBuilder.HasOne(cpc => cpc.CurrencyPair).WithMany(cp => cp.CurrencyPairComponents)
                .HasForeignKey(cpc => cpc.CurrencyPairId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}