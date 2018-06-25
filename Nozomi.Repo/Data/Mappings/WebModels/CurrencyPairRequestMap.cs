using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.WebModels;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class CurrencyPairRequestMap : BaseMap<CurrencyPairRequest>
    {
        public CurrencyPairRequestMap(EntityTypeBuilder<CurrencyPairRequest> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasOne(cpr => cpr.CurrencyPair).WithMany(cp => cp.CurrencyPairRequests)
                .HasForeignKey(cpr => cpr.CurrencyPairId);
            entityTypeBuilder.HasMany(cpr => cpr.RequestComponents).WithOne(rc => rc.Request)
                .HasForeignKey(r => r.RequestId);

            entityTypeBuilder.HasData(
                new CurrencyPairRequest()
                {
                    CurrencyPairId = 1
                },
                new CurrencyPairRequest()
                {
                    CurrencyPairId = 2
                }
            );
        }
    }
}