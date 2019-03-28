using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
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
        }
    }
}