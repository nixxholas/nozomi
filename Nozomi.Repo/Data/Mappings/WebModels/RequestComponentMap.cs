using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class RequestComponentMap : BaseMap<Component>
    {
        public RequestComponentMap(EntityTypeBuilder<Component> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(rc => rc.Id).HasName("RequestComponent_PK_Id");
            entityTypeBuilder.Property(rc => rc.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(e => e.Guid).ValueGeneratedOnAdd();
            entityTypeBuilder.HasIndex(e => e.Guid).IsUnique();

            entityTypeBuilder.HasIndex(rc => new {rc.RequestId, rc.ComponentType})
                .HasName("RequestComponent_AK_RequestId_ComponentType").IsUnique();
            
            entityTypeBuilder.HasIndex(rc => rc.Guid).IsUnique();
            entityTypeBuilder.Property(rc => rc.Guid);

            entityTypeBuilder.Property(rc => rc.Identifier).IsRequired(false);
            entityTypeBuilder.Property(rc => rc.QueryComponent).IsRequired(false);
            entityTypeBuilder.Property(rc => rc.IsDenominated).HasDefaultValue(false).IsRequired();
            entityTypeBuilder.Property(rc => rc.AnomalyIgnorance).HasDefaultValue(false).IsRequired();
            entityTypeBuilder.Property(rc => rc.StoreHistoricals).HasDefaultValue(false).IsRequired();
            
            entityTypeBuilder.HasOne(rc => rc.Request).WithMany(r => r.RequestComponents)
                .HasForeignKey(rc => rc.RequestId).OnDelete(DeleteBehavior.Restrict);
            entityTypeBuilder.HasMany(rc => rc.RcdHistoricItems).WithOne(rcd => rcd.Component)
                .HasForeignKey(rcd => rcd.RequestComponentId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}