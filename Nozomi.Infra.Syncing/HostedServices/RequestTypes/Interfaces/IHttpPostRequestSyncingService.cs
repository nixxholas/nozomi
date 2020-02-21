using System.Threading.Tasks;
using Nozomi.Data.Models.Web;

namespace Nozomi.Infra.Syncing.HostedServices.RequestTypes.Interfaces
{
    public interface IHttpPostRequestSyncingService
    {
        Task<bool> Process(Request req);
    }
}
