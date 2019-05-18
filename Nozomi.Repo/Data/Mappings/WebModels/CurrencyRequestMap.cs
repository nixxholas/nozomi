using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class CurrencyRequestMap : BaseMap<CurrencyRequest>
    {
        public CurrencyRequestMap(EntityTypeBuilder<CurrencyRequest> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasOne(cr => cr.Currency)
                .WithMany(c => c.CurrencyRequests).HasForeignKey(cr => cr.CurrencyId)
                .HasConstraintName("CurrencyRequest_Currency_Constraint");

            entityTypeBuilder.HasData(
                // ETH BFX Etherscan Request
                new CurrencyRequest
                {
                    Id = 1,
                    CurrencyId = 3,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.etherscan.io/api",
                    Delay = 5000
                },
                // KNC BFX Etherscan Request
                new CurrencyRequest
                {
                    Id = 2,
                    CurrencyId = 4,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.etherscan.io/api",
                    Delay = 5000
                },
                // BTC POLO Bitpay Insight Request
                new CurrencyRequest
                {
                    Id = 3,
                    CurrencyId = 12,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://insight.bitpay.com/api/status?q=getBlockCount",
                    Delay = 90000
                },
                // BTC POLO Coinranking Request
                new CurrencyRequest
                {
                    Id = 4,
                    CurrencyId = 12,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.coinranking.com/v1/public/coin/1?base=USD",
                    Delay = 90000
                }
            );
        }
    }
}