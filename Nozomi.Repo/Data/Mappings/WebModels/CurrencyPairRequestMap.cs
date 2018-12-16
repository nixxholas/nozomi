using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.WebModels;
using System;
using Nozomi.Core.Helpers.Mapping;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class CurrencyPairRequestMap : BaseMap<CurrencyPairRequest>
    {
        public CurrencyPairRequestMap(EntityTypeBuilder<CurrencyPairRequest> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasOne(cpr => cpr.CurrencyPair).WithMany(cp => cp.CurrencyPairRequests)
                .HasForeignKey(cpr => cpr.CurrencyPairId);
        }
    }
}