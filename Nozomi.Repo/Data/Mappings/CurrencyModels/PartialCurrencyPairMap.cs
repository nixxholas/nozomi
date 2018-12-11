using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class PartialCurrencyPairMap : BaseMap<PartialCurrencyPair>
    {
        public PartialCurrencyPairMap(EntityTypeBuilder<PartialCurrencyPair> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(pcp => new { pcp.CurrencyPairId, pcp.IsMain })
                .HasName("PartialCurrencyPair_CK_CurrencyPairId_IsMain");

            entityTypeBuilder.HasOne(pcp => pcp.Currency).WithMany(c => c.PartialCurrencyPairs)
                .HasForeignKey(pcp => pcp.CurrencyId)
                .HasConstraintName("PartialCurrencyPairs_Currency_Constraint");
            entityTypeBuilder.HasOne(pcp => pcp.CurrencyPair).WithMany(cp => cp.PartialCurrencyPairs)
                .HasForeignKey(pcp => pcp.CurrencyPairId)
                .HasConstraintName("PartialCurrencyPairs_CurrencyPair_Constraint");
        }
    }
}