using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class SubComputeMap : BaseMap<SubCompute>
    {
        public SubComputeMap(EntityTypeBuilder<SubCompute> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(sc => new { sc.ChildComputeGuid, sc.ParentComputeGuid })
                .HasName("SubCompute_CK_ChildComputeGuid_ParentComputeGuid");
            
            entityTypeBuilder.HasOne(sc => sc.ParentCompute)
                .WithMany(pc => pc.ChildComputes)
                .HasForeignKey(sc => sc.ParentComputeGuid)
                .OnDelete(DeleteBehavior.Cascade);

            entityTypeBuilder.HasOne(sc => sc.ChildCompute)
                .WithMany(cc => cc.ParentComputes)
                .HasForeignKey(sc => sc.ChildComputeGuid)
                .OnDelete(DeleteBehavior.Cascade);
            // entityTypeBuilder.HasMany(sc => sc.ChildComputes)
            //     .WithOne(cc => cc.ParentCompute)
            //     .HasForeignKey(cc => cc.ParentComputeGuid)
            //     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}