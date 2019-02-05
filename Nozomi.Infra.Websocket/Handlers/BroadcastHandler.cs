using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Nozomi.Infra.Websocket.Managers;

namespace Nozomi.Infra.Websocket.Handlers
{
    public class BroadcastHandler : WebSocketHandler
    {
        public BroadcastHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public override ICollection<string> SubscribedClients { get; set; }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var socketId = WebSocketConnectionManager.GetId(socket);
             await SendMessageToAllAsync($"{socketId} is now connected");
         }
 
         public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
         {
             var socketId = WebSocketConnectionManager.GetId(socket);
             var message = $"{socketId} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";
 
             await SendMessageToAllAsync(message);
         }
    }
 }