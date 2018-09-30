using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.WebModels;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class RequestComponentMap : BaseMap<RequestComponent>
    {
        public RequestComponentMap(EntityTypeBuilder<RequestComponent> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(rc => rc.Id).HasName("RequestComponent_PK_Id");
            entityTypeBuilder.Property(rc => rc.Id).ValueGeneratedOnAdd();
            
            entityTypeBuilder.Property(rc => rc.QueryComponent).IsRequired(false);
            
            entityTypeBuilder.HasOne(rc => rc.Request).WithMany(r => r.RequestComponents)
                .HasForeignKey(rc => rc.RequestId);
            entityTypeBuilder.HasMany(rc => rc.RequestComponentData).WithOne(rcd => rcd.RequestComponent)
                .HasForeignKey(rcd => rcd.RequestComponentId);
        }
    }
}