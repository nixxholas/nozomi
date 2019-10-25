using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.Models;
using Nozomi.Repo.BCL;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class RequestMap : BaseMap<Request>
    {
        public RequestMap(EntityTypeBuilder<Request> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(r => r.Id).HasName("Request_PK_Id");
            entityTypeBuilder.Property(r => r.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasAlternateKey(r => r.Guid);
            entityTypeBuilder.Property(r => r.Guid).HasDefaultValueSql("uuid_generate_v4()");

            entityTypeBuilder.Property(r => r.Delay).HasDefaultValue(0).IsRequired();
            entityTypeBuilder.Property(r => r.FailureDelay).HasDefaultValue(3600000).IsRequired();
            
            // We need this to determine the type of request to execute with
            entityTypeBuilder.Property(r => r.RequestType).IsRequired();

            entityTypeBuilder.Property(r => r.ResponseType).IsRequired().HasDefaultValue(ResponseType.Json);
            
            // Sometimes, some APIs don't really have a deep declaration requirement
            entityTypeBuilder.Property(r => r.DataPath).IsRequired(false);

            entityTypeBuilder.HasMany(r => r.RequestComponents).WithOne(rc => rc.Request)
                .HasForeignKey(rc => rc.RequestId).OnDelete(DeleteBehavior.Cascade);
            entityTypeBuilder.HasMany(r => r.RequestProperties).WithOne(rp => rp.Request)
                .HasForeignKey(rp => rp.RequestId).OnDelete(DeleteBehavior.Cascade);
            entityTypeBuilder.HasMany(r => r.WebsocketCommands).WithOne(wsc => wsc.Request)
                .HasForeignKey(wsc => wsc.RequestId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}