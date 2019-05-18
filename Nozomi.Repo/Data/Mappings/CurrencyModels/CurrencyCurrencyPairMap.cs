using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyCurrencyPairMap : BaseMap<CurrencyCurrencyPair>
    {
        public CurrencyCurrencyPairMap(EntityTypeBuilder<CurrencyCurrencyPair> entityTypeBuilder) : base(
            entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(ccp => new {ccp.CurrencyPairId, ccp.CurrencyId})
                .HasName("CurrencyCurrencyPair_CK_CurrencyPairId_CurrencyId");

            entityTypeBuilder.HasOne(pcp => pcp.Currency)
                .WithMany(c => c.CurrencyCurrencyPairs)
                .HasForeignKey(pcp => pcp.CurrencyId)
                .HasConstraintName("CurrencyCurrencyPairs_Currency_Constraint");
            entityTypeBuilder.HasOne(pcp => pcp.CurrencyPair)
                .WithMany(cp => cp.CurrencyPairCurrencies)
                .HasForeignKey(pcp => pcp.CurrencyPairId)
                .HasConstraintName("PartialCurrencyPairs_CurrencyPair_Constraint");

            entityTypeBuilder.HasData(
                // BFX ETHUSD
                new CurrencyCurrencyPair
                {
                    CurrencyPairId = 1,
                    CurrencyId = 3
                },
                new CurrencyCurrencyPair
                {
                    CurrencyPairId = 1,
                    CurrencyId = 1
                },
                // BFX KNCUSD
                new CurrencyCurrencyPair
                {
                    CurrencyPairId = 2,
                    CurrencyId = 4
                },
                new CurrencyCurrencyPair
                {
                    CurrencyPairId = 2,
                    CurrencyId = 1
                },
                // ECB EURUSD
                new CurrencyCurrencyPair
                {
                    CurrencyPairId = 3,
                    CurrencyId = 8
                },
                new CurrencyCurrencyPair
                {
                    CurrencyPairId = 3,
                    CurrencyId = 9
                },
                // AlphaVantage EURUSD
                new CurrencyCurrencyPair
                {
                    CurrencyPairId = 4,
                    CurrencyId = 10
                },
                new CurrencyCurrencyPair
                {
                    CurrencyPairId = 4,
                    CurrencyId = 11
                },
                // POLO BTCBCN
                new CurrencyCurrencyPair
                {
                    CurrencyPairId = 5,
                    CurrencyId = 12
                },
                new CurrencyCurrencyPair
                {
                    CurrencyPairId = 5,
                    CurrencyId = 13
                },
                // POLO BTCBTS
                new CurrencyCurrencyPair
                {
                    CurrencyPairId = 6,
                    CurrencyId = 12
                },
                new CurrencyCurrencyPair
                {
                    CurrencyPairId = 6,
                    CurrencyId = 14
                },
                // BFX ETHEUR
                new CurrencyCurrencyPair
                {
                    CurrencyPairId = 7,
                    CurrencyId = 3
                },
                new CurrencyCurrencyPair
                {
                    CurrencyPairId = 7,
                    CurrencyId = 2
                },
                new CurrencyCurrencyPair
                {
                    CurrencyPairId = 8,
                    CurrencyId = 12
                },
                new CurrencyCurrencyPair
                {
                    CurrencyPairId = 8,
                    CurrencyId = 15
                }
            );
        }
    }
}