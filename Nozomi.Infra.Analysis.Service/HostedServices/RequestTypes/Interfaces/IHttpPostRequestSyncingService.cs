using System.Threading.Tasks;
using Nozomi.Data.Models;

namespace Nozomi.Infra.Analysis.Service.HostedServices.RequestTypes.Interfaces
{
    public interface IHttpPostRequestSyncingService
    {
        Task<bool> Process(Request req);
    }
}
