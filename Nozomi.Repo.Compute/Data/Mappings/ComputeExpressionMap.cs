using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Compute.Data.Mappings
{
    public class ComputeExpressionMap : BaseMap<ComputeExpression>
    {
        public ComputeExpressionMap(EntityTypeBuilder<ComputeExpression> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(ce => ce.Guid).HasName("ComputeExpression_PK_Guid");
            entityTypeBuilder.Property(ce => ce.Guid).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(ce => ce.Type)
                .HasDefaultValue(ComputeExpressionType.Generic)
                .IsRequired();

            entityTypeBuilder.Property(ce => ce.Expression).IsRequired();
            entityTypeBuilder.Property(ce => ce.Value).IsRequired(false);

            entityTypeBuilder.HasOne(ce => ce.Compute)
                .WithMany(c => c.Expressions)
                .HasForeignKey(e => e.ComputeGuid)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}