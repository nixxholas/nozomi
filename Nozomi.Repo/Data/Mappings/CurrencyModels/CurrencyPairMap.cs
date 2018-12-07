using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyPairMap : BaseMap<CurrencyPair>
    {
        public CurrencyPairMap(EntityTypeBuilder<CurrencyPair> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(cp => cp.Id).HasName("CurrencyPair_PK_Id");
            entityTypeBuilder.Property(cp => cp.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(cp => cp.APIUrl).IsRequired();
            entityTypeBuilder.Property(cp => cp.DefaultComponent).IsRequired();

            entityTypeBuilder.HasOne(cp => cp.CurrencySource).WithMany(cs => cs.CurrencyPairs)
                .HasForeignKey(cp => cp.CurrencySourceId)
                .HasConstraintName("CurrencyPairs_CurrencySource_Constraint");
            entityTypeBuilder.HasMany(cp => cp.CurrencyPairRequests).WithOne(cpr => cpr.CurrencyPair)
                .HasForeignKey(cpr => cpr.CurrencyPairId)
                .HasConstraintName("CurrencyPair_CurrencyPairRequest_Constraint");
            entityTypeBuilder.HasMany(cp => cp.PartialCurrencyPairs).WithOne(pcp => pcp.CurrencyPair)
                .HasForeignKey(pcp => pcp.CurrencyPairId)
                .HasConstraintName("CurrencyPair_PartialCurrencyPairs_Constraint");

            entityTypeBuilder.HasData(
                new CurrencyPair()
                {
                    Id = 1,
                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                    APIUrl = "https://api.ethfinex.com/v2/ticker/tETHUSD",
                    DefaultComponent = "0",
                    CurrencySourceId = 1
                },
                new CurrencyPair()
                {
                    Id = 2,
                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                    APIUrl = "https://api.ethfinex.com/v2/ticker/tKNCUSD",
                    DefaultComponent = "0",
                    CurrencySourceId = 1
                },
                new CurrencyPair()
                {
                    Id = 3,
                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                    APIUrl = "https://api.binance.com/api/v3/ticker/bookTicker?symbol=KNCETH",
                    DefaultComponent = "askPrice",
                    CurrencySourceId = 3
                },
                new CurrencyPair()
                {
                    Id = 4,
                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                    APIUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
                    DefaultComponent = "Cube",
                    CurrencySourceId = 4
                },
                new CurrencyPair()
                {
                    Id = 5,
                    CurrencyPairType = CurrencyPairType.TRADEABLE,
                    APIUrl = "https://www.alphavantage.co/query",
                    DefaultComponent = "Realtime Currency Exchange Rate/5. Exchange Rate",
                    CurrencySourceId = 5
                }
            );
        }
    }
}
