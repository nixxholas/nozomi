using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyCurrencyPairMap : BaseMap<CurrencyCurrencyPair>
        {
            public CurrencyCurrencyPairMap(EntityTypeBuilder<CurrencyCurrencyPair> entityTypeBuilder) : base(entityTypeBuilder)
            {
                entityTypeBuilder.HasKey(ccp => new { ccp.CurrencyPairId, ccp.CurrencyId })
                    .HasName("CurrencyCurrencyPair_CK_CurrencyPairId_CurrencyId");

                entityTypeBuilder.HasOne(pcp => pcp.Currency)
                    .WithMany(c => c.CurrencyCurrencyPairs)
                    .HasForeignKey(pcp => pcp.CurrencyId)
                    .HasConstraintName("CurrencyCurrencyPairs_Currency_Constraint");
                entityTypeBuilder.HasOne(pcp => pcp.CurrencyPair)
                    .WithMany(cp => cp.CurrencyPairCurrencies)
                    .HasForeignKey(pcp => pcp.CurrencyPairId)
                    .HasConstraintName("PartialCurrencyPairs_CurrencyPair_Constraint");
            }
        }
}