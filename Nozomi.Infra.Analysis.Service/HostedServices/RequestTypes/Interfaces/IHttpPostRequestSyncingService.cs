using System.Threading.Tasks;
using Nozomi.Data.Models.Web;

namespace Nozomi.Infra.Analysis.Service.HostedServices.RequestTypes.Interfaces
{
    public interface IHttpPostRequestSyncingService
    {
        Task<bool> Process(Request req);
    }
}
