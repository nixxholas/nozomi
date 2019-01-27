using System.Collections.Generic;
using Nozomi.Data.WebModels;
using Nozomi.Data.WebModels.WebsocketModels;

namespace Nozomi.Service.Events.Websocket.Interfaces
{
    public interface IWebsocketRequestEvent
    {
        ICollection<WebsocketRequest> GetAllByRequestType(RequestType requestType);
    }
}