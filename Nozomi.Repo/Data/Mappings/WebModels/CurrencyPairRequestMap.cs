using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.WebModels;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class CurrencyPairRequestMap
    {
        public CurrencyPairRequestMap(EntityTypeBuilder<CurrencyPairRequest> entityTypeBuilder)
        {
            entityTypeBuilder.HasMany(cpr => cpr.RequestComponents).WithOne(rc => rc.Request)
                .HasForeignKey(r => r.RequestId);
        }
    }
}