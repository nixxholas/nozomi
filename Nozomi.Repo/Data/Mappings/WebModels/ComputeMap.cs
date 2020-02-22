using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class ComputeMap : BaseMap<Compute>
    {
        public ComputeMap(EntityTypeBuilder<Compute> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(e => e.Guid).HasName("Compute_PK_Guid");
            entityTypeBuilder.Property(e => e.Guid).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(e => e.Key).IsRequired(false);
            entityTypeBuilder.Property(e => e.Formula).IsRequired();

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