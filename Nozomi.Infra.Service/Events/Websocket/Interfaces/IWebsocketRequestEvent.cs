using System.Collections.Generic;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Websocket;

namespace Nozomi.Service.Events.Websocket.Interfaces
{
    public interface IWebsocketRequestEvent
    {
        ICollection<WebsocketRequest> GetAllByRequestType(RequestType requestType);

        IDictionary<string, ICollection<WebsocketRequest>> GetAllByRequestTypeUniqueToURL(RequestType requestType);
    }
}