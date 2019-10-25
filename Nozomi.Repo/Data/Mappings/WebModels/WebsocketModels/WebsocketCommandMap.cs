using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.Models.Websocket;
using Nozomi.Repo.BCL;

namespace Nozomi.Repo.Data.Mappings.WebModels.WebsocketModels
{
    public class WebsocketCommandMap : BaseMap<WebsocketCommand>
    {
        public WebsocketCommandMap(EntityTypeBuilder<WebsocketCommand> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(wsc => wsc.Id).HasName("WebsocketCommand_PK_Id");
            entityTypeBuilder.Property(wsc => wsc.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasAlternateKey(r => r.Guid);
            entityTypeBuilder.Property(r => r.Guid).HasDefaultValueSql("uuid_generate_v4()");

            entityTypeBuilder.Property(wsc => wsc.CommandType).IsRequired();
            entityTypeBuilder.Property(wsc => wsc.Name).IsRequired(false);
            entityTypeBuilder.Property(wsc => wsc.Delay).HasDefaultValue(0);

            entityTypeBuilder.HasOne(wsc => wsc.Request)
                .WithMany(wsr => wsr.WebsocketCommands).HasForeignKey(wsc => wsc.RequestId);
            entityTypeBuilder.HasMany(wsc => wsc.WebsocketCommandProperties)
                .WithOne(wscp => wscp.WebsocketCommand).HasForeignKey(wscp => wscp.WebsocketCommandId);
        }
    }
}