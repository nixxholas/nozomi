using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nozomi.Data.Models.Web;

namespace Nozomi.Service.HostedServices.RequestTypes.Interfaces
{
    public interface IHttpGetRequestSyncingService
    {
        Task<bool> ProcessCurrencyPairRequests(ICollection<CurrencyPairRequest> requests);

        Task<bool> ProcessCurrencyRequests(ICollection<CurrencyRequest> requests);

        bool Update(JToken currToken, ResponseType resType, IEnumerable<RequestComponent> requestComponents);
    }
}