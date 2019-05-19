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
                    CurrencySourceId = 3
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 1,
                    CurrencySourceId = 1
                },
                // BFX KNCUSD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 2,
                    CurrencySourceId = 4
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 2,
                    CurrencySourceId = 1
                },
                // ECB EURUSD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 3,
                    CurrencySourceId = 8
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 3,
                    CurrencySourceId = 9
                },
                // AlphaVantage EURUSD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 4,
                    CurrencySourceId = 10
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 4,
                    CurrencySourceId = 11
                },
                // POLO BTCBCN
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 5,
                    CurrencySourceId = 12
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 5,
                    CurrencySourceId = 13
                },
                // POLO BTCBTS
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 6,
                    CurrencySourceId = 12
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 6,
                    CurrencySourceId = 14
                },
                // BFX ETHEUR
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 7,
                    CurrencySourceId = 3
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 7,
                    CurrencySourceId = 2
                },
                // POLO BTCUSDT
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 8,
                    CurrencySourceId = 12
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 8,
                    CurrencySourceId = 15
                },
                // Binance ETHBTC
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 9,
                    CurrencySourceId = 6
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 9,
                    CurrencySourceId = 7
                },
                // Binance KNCETH
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 10,
                    CurrencySourceId = 5
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 10,
                    CurrencySourceId = 6
                },
                // Coinhako BTCSGD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 11,
                    CurrencySourceId = 16
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 11,
                    CurrencySourceId = 17
                },
                // Coinhako BTCUSD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 12,
                    CurrencySourceId = 16
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 12,
                    CurrencySourceId = 19
                },
                // Coinhako ETHSGD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 13,
                    CurrencySourceId = 18
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 13,
                    CurrencySourceId = 17
                },
                // Coinhako ETHUSD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 14,
                    CurrencySourceId = 18
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 14,
                    CurrencySourceId = 19
                },
                // Coinhako LTCSGD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 15,
                    CurrencySourceId = 20
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 15,
                    CurrencySourceId = 17
                },
                // Coinhako LTCUSD
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 16,
                    CurrencySourceId = 20
                },
                new CurrencyPairSourceCurrency
                {
                    CurrencyPairId = 16,
                    CurrencySourceId = 19
                }
            );
        }
    }
}