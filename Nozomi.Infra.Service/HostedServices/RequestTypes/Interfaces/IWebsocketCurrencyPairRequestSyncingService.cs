using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Data.WebModels;
using Nozomi.Data.WebModels.WebsocketModels;

namespace Nozomi.Service.HostedServices.RequestTypes.Interfaces
{
    public interface IWebsocketCurrencyPairRequestSyncingService
    {
        bool IsRequestNeeded(WebsocketRequest cpr);

        Task<bool> Process(ICollection<WebsocketRequest> cpr, string payload);
    }
}