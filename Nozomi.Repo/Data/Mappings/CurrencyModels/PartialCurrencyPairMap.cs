using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class PartialCurrencyPairMap : BaseMap<PartialCurrencyPair>
    {
        public PartialCurrencyPairMap(EntityTypeBuilder<PartialCurrencyPair> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(pcp => new { pcp.CurrencyPairId, pcp.IsMain });

            entityTypeBuilder.HasOne(pcp => pcp.Currency).WithMany(c => c.PartialCurrencyPairs)
                .HasForeignKey(pcp => pcp.CurrencyId);
            entityTypeBuilder.HasOne(pcp => pcp.CurrencyPair).WithMany(cp => cp.PartialCurrencyPairs)
                .HasForeignKey(pcp => pcp.CurrencyPairId);

            entityTypeBuilder.HasData(
                new PartialCurrencyPair()
                {
                    CurrencyId = 1,
                    IsMain = false,
                    CurrencyPairId = 1
                },
                new PartialCurrencyPair()
                {
                    CurrencyId = 2,
                    IsMain = true,
                    CurrencyPairId = 1
                },
                new PartialCurrencyPair()
                {
                    CurrencyId = 1,
                    IsMain = false,
                    CurrencyPairId = 2
                },
                new PartialCurrencyPair()
                {
                    CurrencyId = 3,
                    IsMain = true,
                    CurrencyPairId = 2
                },
                new PartialCurrencyPair()
                {
                    CurrencyId = 4,
                    IsMain = true,
                    CurrencyPairId = 3
                },
                new PartialCurrencyPair()
                {
                    CurrencyId = 5,
                    IsMain = false,
                    CurrencyPairId = 3
                },
                new PartialCurrencyPair()
                {
                    CurrencyId = 6,
                    IsMain = true,
                    CurrencyPairId = 4
                },
                new PartialCurrencyPair()
                {
                    CurrencyId = 7,
                    IsMain = false,
                    CurrencyPairId = 4
                }
            );
        }
    }
}