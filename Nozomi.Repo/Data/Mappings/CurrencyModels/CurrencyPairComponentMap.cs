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

            entityTypeBuilder.HasOne(cpc => cpc.CurrencyPair).WithMany(cp => cp.CurrencyPairComponents)
                .HasForeignKey(cpc => cpc.CurrencyPairId).OnDelete(DeleteBehavior.Cascade);

            entityTypeBuilder.HasData(
                new CurrencyPairComponent()
                {
                    Id = 1,
                    CurrencyPairId = 1,
                    ComponentType = ComponentType.Ask,
                    QueryComponent = "0",
                    RequestId = 1
                },
                new CurrencyPairComponent()
                {
                    Id = 2,
                    CurrencyPairId = 2,
                    ComponentType = ComponentType.Ask,
                    QueryComponent = "0",
                    RequestId = 2
                }
            );
        }
    }
}