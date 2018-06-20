using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.WebModels;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class RequestMap
    {
        public RequestMap(EntityTypeBuilder<Request> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(r => r.Id).HasName("Request_PK_Id");
            entityTypeBuilder.Property(r => r.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasAlternateKey(r => r.Guid).HasName("Request_AK_Guid");
            entityTypeBuilder.Property(r => r.Guid).ValueGeneratedOnAdd();

            // We need this to determine the type of request to execute with
            entityTypeBuilder.Property(r => r.RequestType).IsRequired();
            // Sometimes, some APIs don't really have a deep declaration requirement
            entityTypeBuilder.Property(r => r.DataPath).IsRequired(false);

            entityTypeBuilder.HasMany(r => r.RequestComponents).WithOne(rc => rc.Request)
                .HasForeignKey(rc => rc.RequestId);
            entityTypeBuilder.HasMany(r => r.RequestProperties).WithOne(rp => rp.Request)
                .HasForeignKey(rp => rp.RequestId);
        }
    }
}