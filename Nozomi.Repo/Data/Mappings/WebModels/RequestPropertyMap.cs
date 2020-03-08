using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class RequestPropertyMap : BaseMap<RequestProperty>
    {
        public RequestPropertyMap(EntityTypeBuilder<RequestProperty> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(rp => rp.Id).HasName("RequestProperty_PK_Id");
            entityTypeBuilder.Property(rp => rp.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasAlternateKey(rp => rp.Guid).HasName("RequestProperty_AK_Guid");
            entityTypeBuilder.Property(rp => rp.Guid).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(rp => rp.RequestPropertyType).IsRequired();

            entityTypeBuilder.Property(rp => rp.Key).IsRequired(false);
            entityTypeBuilder.Property(rp => rp.Value).IsRequired(false);

            entityTypeBuilder.HasOne(rp => rp.Request).WithMany(r => r.RequestProperties)
                .HasForeignKey(rp => rp.RequestId);
        }
    }
}