using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class ComputeValueMap : BaseMap<ComputeValue>
    {
        public ComputeValueMap(EntityTypeBuilder<ComputeValue> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(cv => new {cv.ComputeGuid, cv.CreatedAt})
                .HasName("ComputeValue_CK_ComputeGuid_CreatedAt");

            entityTypeBuilder.HasAlternateKey(cv => cv.Guid).HasName("ComputeValue_AK_Guid");
            entityTypeBuilder.Property(cv => cv.Guid).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(cv => cv.Value).IsRequired();

            entityTypeBuilder.HasOne(cv => cv.Compute)
                .WithMany(c => c.Values)
                .HasForeignKey(v => v.ComputeGuid)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}