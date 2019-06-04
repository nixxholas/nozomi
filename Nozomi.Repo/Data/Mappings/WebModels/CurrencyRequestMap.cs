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

            entityTypeBuilder.HasMany(cr => cr.RequestComponents)
                .WithOne(rc => rc.Request as CurrencyRequest).HasForeignKey(rc => rc.RequestId)
                .OnDelete(DeleteBehavior.Restrict);
            entityTypeBuilder.HasMany(cr => cr.RequestLogs)
                .WithOne(rc => rc.Request as CurrencyRequest).HasForeignKey(rc => rc.RequestId)
                .OnDelete(DeleteBehavior.Restrict);
            entityTypeBuilder.HasMany(cr => cr.RequestProperties)
                .WithOne(rc => rc.Request as CurrencyRequest).HasForeignKey(rc => rc.RequestId)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasData(
                // ETH Etherscan Request
                new CurrencyRequest
                {
                    Id = 1,
                    CurrencyId = 3,
                    Guid = Guid.Parse("d13fc276-8077-49d2-ba38-998c58895df9"),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.etherscan.io/api",
                    Delay = 5000
                },
                // KNC Etherscan Request
                new CurrencyRequest
                {
                    Id = 2,
                    CurrencyId = 4,
                    Guid = Guid.Parse("b7b9642e-357a-451c-9741-bf5a7fcb0ad1"),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.etherscan.io/api",
                    Delay = 5000
                },
                // BTC Bitpay Insight Request
                new CurrencyRequest
                {
                    Id = 3,
                    CurrencyId = 5,
                    Guid = Guid.Parse("31ceeb18-1d89-43d2-b215-0488d9417c67"),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://insight.bitpay.com/api/status?q=getBlockCount",
                    Delay = 90000
                },
                // BTC Coinranking Request
                new CurrencyRequest
                {
                    Id = 4,
                    CurrencyId = 5,
                    Guid = Guid.Parse("7f10715f-b5cc-4e52-9fa8-011311a5a2ca"),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.coinranking.com/v1/public/coin/1?base=USD",
                    Delay = 90000
                }
            );
        }
    }
}