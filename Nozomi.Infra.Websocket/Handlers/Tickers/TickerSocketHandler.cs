using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Nozomi.Infra.Websocket.Managers;

namespace Nozomi.Infra.Websocket.Handlers.Tickers
{
    public class TickerSocketHandler : WebSocketHandler
    {
        public TickerSocketHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public override Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            throw new NotImplementedException();
        }
    }
}