using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.WebModels;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class RequestPropertyMap
    {
        public RequestPropertyMap(EntityTypeBuilder<RequestProperty> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(rp => rp.Id).HasName("RequestProperty_PK_Id");
            entityTypeBuilder.Property(rp => rp.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(rp => rp.RequestPropertyType).IsRequired();

            entityTypeBuilder.Property(rp => rp.Key).IsRequired(false);
            entityTypeBuilder.Property(rp => rp.Value).IsRequired(false);

            entityTypeBuilder.HasOne(rp => rp.Request).WithMany(r => r.RequestProperties)
                .HasForeignKey(rp => rp.RequestId);
        }
    }
}