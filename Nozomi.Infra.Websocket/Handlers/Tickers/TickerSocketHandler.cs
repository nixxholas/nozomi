using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Nozomi.Infra.Websocket.Managers;

namespace Nozomi.Infra.Websocket.Handlers.Tickers
{
    public class TickerSocketHandler : WebSocketHandler
    {
        public TickerSocketHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
            SubscribedClients = new List<string>();
        }

        public sealed override ICollection<string> SubscribedClients { get; set; }

        public override Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public override Task SubscribeAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);
            SubscribedClients.Add(socketId);

            return Task.CompletedTask;
        }
    }
}