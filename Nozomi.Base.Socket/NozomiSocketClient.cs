using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Nozomi.Base.Socket
{
    public class NozomiSocketClient
    {
        private readonly Task _initializingTask;
        public ClientWebSocket ClientWebSocket { get; set; }

        public NozomiSocketClient(string wsUrl)
        {
            ClientWebSocket = new ClientWebSocket();
            _initializingTask = Init(wsUrl);
        }
        
        private async Task Init(string wsUrl)
        {
            try
            {
                await ClientWebSocket.ConnectAsync(new Uri(wsUrl), CancellationToken.None);
            }
            catch (WebSocketException ex)
            {
                // TODO: log
            }
        }

        public async Task<bool> Dispatch(byte[] msg, bool endOfMsg = true, 
            CancellationToken token = new CancellationToken())
        {
            try
            {
                await ClientWebSocket.SendAsync(msg, WebSocketMessageType.Binary, endOfMsg, token);
            }
            catch (WebSocketException ex)
            {
                // TODO: log
                return false;
            }

            return true;
        }
    }
}