using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Web.Websocket;

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

            entityTypeBuilder.HasData(
                // Binance's Websocket-based ticker data stream
                new WebsocketRequest
                {
                    Id = 13,
                    CurrencyPairId = 9,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.WebSocket,
                    ResponseType = ResponseType.Json,
                    DataPath = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                    Delay = 0,
                    RequestComponents = new List<RequestComponent>()
                    {
                        new RequestComponent
                        {
                            ComponentType = ComponentType.VOLUME,
                            Identifier = "data/s=>ETHBTC",
                            QueryComponent = "v"
                        },
                        new RequestComponent
                        {
                            ComponentType = ComponentType.Ask,
                            Identifier = "data/s=>ETHBTC",
                            QueryComponent = "a"
                        },
                        new RequestComponent
                        {
                            ComponentType = ComponentType.Ask_Size,
                            Identifier = "data/s=>ETHBTC",
                            QueryComponent = "A"
                        },
                        new RequestComponent
                        {
                            ComponentType = ComponentType.Bid,
                            Identifier = "data/s=>ETHBTC",
                            QueryComponent = "b"
                        },
                        new RequestComponent
                        {
                            ComponentType = ComponentType.Bid_Size,
                            Identifier = "data/s=>ETHBTC",
                            QueryComponent = "B"
                        }
                    }
                },
                new WebsocketRequest
                {
                    Id = 14,
                    CurrencyPairId = 10,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.WebSocket,
                    ResponseType = ResponseType.Json,
                    DataPath = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                    Delay = 0,
                    WebsocketCommands = new List<WebsocketCommand>(),
                    RequestComponents = new List<RequestComponent>()
                    {
                        new RequestComponent
                        {
                            ComponentType = ComponentType.VOLUME,
                            Identifier = "data/s=>KNCETH",
                            QueryComponent = "v"
                        },
                        new RequestComponent
                        {
                            ComponentType = ComponentType.Ask,
                            Identifier = "data/s=>KNCETH",
                            QueryComponent = "a"
                        },
                        new RequestComponent
                        {
                            ComponentType = ComponentType.Ask_Size,
                            Identifier = "data/s=>KNCETH",
                            QueryComponent = "A"
                        },
                        new RequestComponent
                        {
                            ComponentType = ComponentType.Bid,
                            Identifier = "data/s=>KNCETH",
                            QueryComponent = "b"
                        },
                        new RequestComponent
                        {
                            ComponentType = ComponentType.Bid_Size,
                            Identifier = "data/s=>KNCETH",
                            QueryComponent = "B"
                        }
                    }
                }
            );
        }
    }
}