using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Core.Helpers.Mapping;
using Nozomi.Data.WebModels;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class RequestComponentDatumMap : BaseMap<RequestComponentDatum>
    {
        
        public RequestComponentDatumMap(EntityTypeBuilder<RequestComponentDatum> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(rcd => rcd.Id).HasName("RequestComponentDatum_PK_Id");
            entityTypeBuilder.Property(rc => rc.Id).ValueGeneratedOnAdd();
            
            entityTypeBuilder.Property(rc => rc.Value).HasDefaultValue(string.Empty);

            entityTypeBuilder.HasOne(rcd => rcd.RequestComponent).WithOne(rc => rc.RequestComponentDatum)
                .HasForeignKey<RequestComponentDatum>(rcd => rcd.RequestComponentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}