using System.Diagnostics;
using System.Threading.Tasks;
using Nozomi.Data.WebModels;

namespace Nozomi.Service.HostedServices.RequestTypes.Interfaces
{
    public interface IHttpGetCurrencyPairRequestSyncingService
    {
        Task<bool> Process(Request req);
    }
}