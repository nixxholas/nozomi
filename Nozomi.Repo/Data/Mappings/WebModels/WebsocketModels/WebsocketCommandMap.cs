using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.WebModels.WebsocketModels;

namespace Nozomi.Repo.Data.Mappings.WebModels.WebsocketModels
{
    public class WebsocketCommandMap : BaseMap<WebsocketCommand>
    {
        public WebsocketCommandMap(EntityTypeBuilder<WebsocketCommand> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(wsc => wsc.Id).HasName("WebsocketCommand_PK_Id");
            entityTypeBuilder.Property(wsc => wsc.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(wsc => wsc.CommandType).IsRequired();
            entityTypeBuilder.Property(wsc => wsc.Name).IsRequired(false);
            entityTypeBuilder.Property(wsc => wsc.Delay).HasDefaultValue(0).IsRequired(false);

            entityTypeBuilder.HasOne(wsc => wsc.WebsocketRequest)
                .WithMany(wsr => wsr.WebsocketCommands).HasForeignKey(wsc => wsc.WebsocketRequestId);
            entityTypeBuilder.HasMany(wsc => wsc.WebsocketCommandProperties)
                .WithOne(wscp => wscp.WebsocketCommand).HasForeignKey(wscp => wscp.WebsocketCommandId);
        }
    }
}