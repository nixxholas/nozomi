using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.WebModels;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class RequestComponentDatumMap : BaseMap<RequestComponentDatum>
    {
        
        public RequestComponentDatumMap(EntityTypeBuilder<RequestComponentDatum> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(rcd => rcd.Id).HasName("RequestComponenDatum_PK_Id");
            entityTypeBuilder.Property(rc => rc.Id).ValueGeneratedOnAdd();
            
            entityTypeBuilder.Property(rc => rc.Value).HasDefaultValue(string.Empty);

            entityTypeBuilder.HasOne(rcd => rcd.RequestComponent).WithMany(rc => rc.RequestComponentData)
                .HasForeignKey(rcd => rcd.RequestComponentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}