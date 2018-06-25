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
            entityTypeBuilder.HasKey(cp => cp.Id);
            entityTypeBuilder.Property(cp => cp.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(cp => cp.APIUrl).IsRequired();
            entityTypeBuilder.Property(cp => cp.DefaultComponent).IsRequired();

            entityTypeBuilder.HasOne(cp => cp.CurrencySource).WithMany(cs => cs.CurrencyPairs)
                .HasForeignKey(cp => cp.CurrencySourceId);
            entityTypeBuilder.HasMany(cp => cp.CurrencyPairComponents).WithOne(cpc => cpc.CurrencyPair)
                .HasForeignKey(cpc => cpc.CurrencyPairId);
            entityTypeBuilder.HasMany(cp => cp.CurrencyPairRequests).WithOne(cpr => cpr.CurrencyPair)
                .HasForeignKey(cpr => cpr.CurrencyPairId);
            entityTypeBuilder.HasMany(cp => cp.PartialCurrencyPairs).WithOne(pcp => pcp.CurrencyPair)
                .HasForeignKey(pcp => pcp.CurrencyPairId);

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
                }
            );
        }
    }
}
