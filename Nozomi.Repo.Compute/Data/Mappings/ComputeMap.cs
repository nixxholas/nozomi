using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;

namespace Nozomi.Repo.Compute.Data.Mappings
{
    public class ComputeMap : BaseMap<Nozomi.Data.Models.Web.Compute>
    {
        public ComputeMap(EntityTypeBuilder<Nozomi.Data.Models.Web.Compute> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(e => e.Guid).HasName("Compute_PK_Guid");
            entityTypeBuilder.Property(e => e.Guid).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(e => e.Key).IsRequired(false);
            entityTypeBuilder.Property(e => e.Formula).IsRequired();
            entityTypeBuilder.Property(e => e.Delay).HasDefaultValue(5000).IsRequired();
            entityTypeBuilder.Property(e => e.FailCount).HasDefaultValue(0).IsRequired();

            entityTypeBuilder.HasMany(e => e.Expressions)
                .WithOne(exp => exp.Compute)
                .HasForeignKey(exp => exp.ComputeGuid)
                .OnDelete(DeleteBehavior.Cascade);
            entityTypeBuilder.HasMany(e => e.ChildComputes)
                .WithOne(cc => cc.ParentCompute)
                .HasForeignKey(cc => cc.ParentComputeGuid)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
            entityTypeBuilder.HasMany(e => e.ParentComputes)
                .WithOne(pc => pc.ChildCompute)
                .HasForeignKey(cc => cc.ParentComputeGuid)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
            // entityTypeBuilder.HasOne(e => e.ParentCompute)
            //     .WithMany(pc => pc.ChildComputes)
            //     .HasForeignKey(cc => cc.ParentComputeGuid)
            //     .IsRequired(false)
            //     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}