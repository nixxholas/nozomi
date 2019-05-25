using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Websocket;

namespace Nozomi.Repo.Data.Mappings.WebModels.WebsocketModels
{
    public class WebsocketRequestMap : BaseMap<WebsocketRequest>
    {
        public WebsocketRequestMap(EntityTypeBuilder<WebsocketRequest> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasOne(wsr => wsr.CurrencyPair)
                .WithMany(cp => cp.WebsocketRequests).HasForeignKey(wsr => wsr.CurrencyPairId);
            entityTypeBuilder.HasMany(wsr => wsr.WebsocketCommands)
                .WithOne(wsc => wsc.WebsocketRequest).HasForeignKey(wsc => wsc.WebsocketRequestId);

            entityTypeBuilder.HasData(
                // Binance's Websocket-based ticker data stream
                new WebsocketRequest
                {
                    Id = 13,
                    CurrencyPairId = 9,
                    Guid = Guid.Parse("6f9d8fe7-71f4-42b8-ac31-526f559549a3"),
                    RequestType = RequestType.WebSocket,
                    ResponseType = ResponseType.Json,
                    DataPath = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                    Delay = 0
                },
                new WebsocketRequest
                {
                    Id = 14,
                    CurrencyPairId = 10,
                    Guid = Guid.Parse("dc33dc82-26e5-4eef-af44-78e1efce2d1f"),
                    RequestType = RequestType.WebSocket,
                    ResponseType = ResponseType.Json,
                    DataPath = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                    Delay = 0
                }
            );
        }
    }
}