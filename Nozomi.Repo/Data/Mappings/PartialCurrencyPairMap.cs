using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Repo.Data.Mappings
{
    public class PartialCurrencyPairMap
    {
        public PartialCurrencyPairMap(EntityTypeBuilder<PartialCurrencyPair> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(pcp => new { pcp.CurrencyPairId, pcp.IsMain });

            entityTypeBuilder.HasOne(pcp => pcp.Currency).WithMany(c => c.PartialCurrencyPairs)
                .HasForeignKey(pcp => pcp.CurrencyId);
            entityTypeBuilder.HasOne(pcp => pcp.CurrencyPair).WithMany(cp => cp.PartialCurrencyPairs)
                .HasForeignKey(pcp => pcp.CurrencyPair);
        }
    }
}