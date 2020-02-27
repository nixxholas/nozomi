using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nozomi.Data.Models.Web;

namespace Nozomi.Infra.Syncing.HostedServices.RequestTypes.Interfaces
{
    public interface IWSRequestSyncingService
    {
        Task<bool> Process(ICollection<Request> cpr, string payload);

        bool Update(JToken token, ResponseType resType, IEnumerable<Component> requestComponents);
    }
}