using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.Models.Websocket;
using Nozomi.Repo.BCL;

namespace Nozomi.Repo.Data.Mappings.WebModels.WebsocketModels
{
    public class WebsocketCommandPropertyMap : BaseMap<WebsocketCommandProperty>
    {
        public WebsocketCommandPropertyMap(EntityTypeBuilder<WebsocketCommandProperty> entityTypeBuilder) 
            : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(wscp => wscp.Id).HasName("WebsocketCommandProperty_PK_Id");
            entityTypeBuilder.Property(wscp => wscp.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasAlternateKey(r => r.Guid);
            entityTypeBuilder.Property(r => r.Guid).HasDefaultValueSql("uuid_generate_v4()");

            entityTypeBuilder.Property(wscp => wscp.CommandPropertyType).IsRequired();
            entityTypeBuilder.Property(wscp => wscp.Key).IsRequired(false).HasDefaultValue();
            entityTypeBuilder.Property(wscp => wscp.Value).IsRequired();

            entityTypeBuilder.HasOne(wscp => wscp.WebsocketCommand)
                .WithMany(wsc => wsc.WebsocketCommandProperties)
                .HasForeignKey(wscp => wscp.WebsocketCommandId);
        }
    }
}