using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class RequestMap : BaseMap<Request>
    {
        public RequestMap(EntityTypeBuilder<Request> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(r => r.Id).HasName("Request_PK_Id");
            entityTypeBuilder.Property(r => r.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasAlternateKey(r => r.Guid).HasName("Request_AK_Guid");
            entityTypeBuilder.Property(r => r.Guid).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(r => r.Delay).HasDefaultValue(0).IsRequired();
            entityTypeBuilder.Property(r => r.FailureDelay).HasDefaultValue(3600000).IsRequired();
            
            // We need this to determine the type of request to execute with
            entityTypeBuilder.Property(r => r.RequestType).IsRequired();

            entityTypeBuilder.Property(r => r.ResponseType).IsRequired().HasDefaultValue(ResponseType.Json);
            
            // Sometimes, some APIs don't really have a deep declaration requirement
            entityTypeBuilder.Property(r => r.DataPath).IsRequired(false);

            entityTypeBuilder.HasMany(r => r.AnalysedComponents).WithOne(ac => ac.Request)
                .HasForeignKey(ac => ac.RequestId)
                .IsRequired(false);
            entityTypeBuilder.HasMany(r => r.RequestComponents).WithOne(rc => rc.Request)
                .HasForeignKey(rc => rc.RequestId);
            entityTypeBuilder.HasMany(r => r.RequestProperties).WithOne(rp => rp.Request)
                .HasForeignKey(rp => rp.RequestId);
        }
    }
}