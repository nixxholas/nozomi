using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nozomi.Data.Models.Web;

namespace Nozomi.Infra.Analysis.Service.HostedServices.RequestTypes.Interfaces
{
    public interface IWSRequestSyncingService
    {
        bool Process(ICollection<Request> cpr, string payload);

        bool Update(JToken token, ResponseType resType, IEnumerable<Component> requestComponents);
    }
}