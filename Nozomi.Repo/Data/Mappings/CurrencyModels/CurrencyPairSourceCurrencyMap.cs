using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyPairSourceCurrencyMap : BaseMap<CurrencyPairSourceCurrency>
    {
        public CurrencyPairSourceCurrencyMap(EntityTypeBuilder<CurrencyPairSourceCurrency> entityTypeBuilder) : base(
            entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(cpsc => new { cpsc.CurrencySourceId, cpsc.CurrencyPairId })
                .HasName("CurrencyPairSourceCurrency_CK_CurrencySourceId_CurrencyPairId");

            entityTypeBuilder.HasOne(cpsc => cpsc.CurrencySource)
                .WithMany(cs => cs.CurrencyPairSourceCurrencies)
                .HasForeignKey(cpsc => cpsc.CurrencySourceId)
                .HasConstraintName("CurrencyPairSourceCurrency_CurrencySource_Constraint");
            entityTypeBuilder.HasOne(cpsc => cpsc.CurrencyPair)
                .WithMany(cp => cp.CurrencyPairSourceCurrencies)
                .HasForeignKey(pcp => pcp.CurrencyPairId)
                .HasConstraintName("CurrencyPairSourceCurrency_CurrencyPair_Constraint");

            entityTypeBuilder.HasData(
                // BFX ETHUSD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 1,
                    CurrencyId = 3
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 1,
                    CurrencyId = 1
                },
                // BFX KNCUSD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 2,
                    CurrencyId = 4
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 2,
                    CurrencyId = 1
                },
                // ECB EURUSD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 3,
                    CurrencyId = 8
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 3,
                    CurrencyId = 9
                },
                // AlphaVantage EURUSD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 4,
                    CurrencyId = 10
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 4,
                    CurrencyId = 11
                },
                // POLO BTCBCN
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 5,
                    CurrencyId = 12
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 5,
                    CurrencyId = 13
                },
                // POLO BTCBTS
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 6,
                    CurrencyId = 12
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 6,
                    CurrencyId = 14
                },
                // BFX ETHEUR
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 7,
                    CurrencyId = 3
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 7,
                    CurrencyId = 2
                },
                // POLO BTCUSDT
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 8,
                    CurrencyId = 12
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 8,
                    CurrencyId = 15
                },
                // Binance ETHBTC
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 9,
                    CurrencyId = 6
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 9,
                    CurrencyId = 7
                },
                // Binance KNCETH
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 10,
                    CurrencyId = 5
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 10,
                    CurrencyId = 6
                },
                // Coinhako BTCSGD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 11,
                    CurrencyId = 16
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 11,
                    CurrencyId = 17
                },
                // Coinhako BTCUSD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 12,
                    CurrencyId = 16
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 12,
                    CurrencyId = 19
                },
                // Coinhako ETHSGD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 13,
                    CurrencyId = 18
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 13,
                    CurrencyId = 17
                },
                // Coinhako ETHUSD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 14,
                    CurrencyId = 18
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 14,
                    CurrencyId = 19
                },
                // Coinhako LTCSGD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 15,
                    CurrencyId = 20
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 15,
                    CurrencyId = 17
                },
                // Coinhako LTCUSD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 16,
                    CurrencyId = 20
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 16,
                    CurrencyId = 19
                }
            );
        }
    }
}