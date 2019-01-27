using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.WebModels.WebsocketModels;

namespace Nozomi.Repo.Data.Mappings.WebModels.WebsocketModels
{
    public class WebsocketRequestMap : BaseMap<WebsocketRequest>
    {
        public WebsocketRequestMap(EntityTypeBuilder<WebsocketRequest> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(wsr => wsr.Id).HasName("WebsocketRequest_PK_Id");
            entityTypeBuilder.Property(wsr => wsr.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasOne(wsr => wsr.CurrencyPair)
                .WithMany(cp => cp.WebsocketRequests).HasForeignKey(wsr => wsr.CurrencyPairId);
            entityTypeBuilder.HasMany(wsr => wsr.WebsocketCommands)
                .WithOne(wsc => wsc.WebsocketRequest).HasForeignKey(wsc => wsc.WebsocketRequestId);
        }
    }
}