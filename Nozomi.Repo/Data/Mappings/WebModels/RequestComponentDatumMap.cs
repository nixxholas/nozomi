using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class RequestComponentDatumMap : BaseMap<RequestComponentDatum>
    {
        
        public RequestComponentDatumMap(EntityTypeBuilder<RequestComponentDatum> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(rcd => rcd.Id).HasName("RequestComponentDatum_PK_Id");
            entityTypeBuilder.Property(rc => rc.Id).ValueGeneratedOnAdd();
            
            entityTypeBuilder.Property(rc => rc.Value).HasDefaultValue(string.Empty);

            entityTypeBuilder.HasOne(rcd => rcd.RequestComponent)
                .WithOne(rc => rc.RequestComponentDatum)
                .HasForeignKey<RequestComponentDatum>(rcd => rcd.RequestComponentId).OnDelete(DeleteBehavior.Cascade);
            entityTypeBuilder.HasMany(rcd => rcd.RcdHistoricItems)
                .WithOne(rcdhi => rcdhi.RequestComponentDatum)
                .HasForeignKey(rcdhi => rcdhi.RequestComponentDatumId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}