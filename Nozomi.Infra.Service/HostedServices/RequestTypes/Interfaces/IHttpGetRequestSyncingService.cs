using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nozomi.Data.Models.Web;

namespace Nozomi.Service.HostedServices.RequestTypes.Interfaces
{
    public interface IHttpGetRequestSyncingService
    {
        Task<bool> ProcessRequest<T>(ICollection<T> requests) where T : Request;

        bool Update(JToken currToken, ResponseType resType, IEnumerable<RequestComponent> requestComponents);
    }
}