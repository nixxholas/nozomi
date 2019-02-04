using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Nozomi.Infra.Websocket.Managers.Interfaces
{
    public interface IWebsocketConnectionManager
    {
        WebSocket GetSocketById(string id);

        ConcurrentDictionary<string, WebSocket> GetAll();

        string GetId(WebSocket socket);

        void AddSocket(WebSocket socket);

        Task RemoveSocket(string socketId);
    }
}