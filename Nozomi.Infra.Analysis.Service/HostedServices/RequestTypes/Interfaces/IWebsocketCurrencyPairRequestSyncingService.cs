using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Websocket;

namespace Nozomi.Infra.Analysis.Service.HostedServices.RequestTypes.Interfaces
{
    public interface IWebsocketCurrencyPairRequestSyncingService
    {
        Task<bool> Process(ICollection<WebsocketRequest> cpr, string payload);

        bool Update(JToken token, ResponseType resType, IEnumerable<RequestComponent> requestComponents);
    }
}