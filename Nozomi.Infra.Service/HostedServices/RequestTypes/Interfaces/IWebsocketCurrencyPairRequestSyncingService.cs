using System.Threading.Tasks;
using Nozomi.Data.WebModels;

namespace Nozomi.Service.HostedServices.RequestTypes.Interfaces
{
    public interface IWebsocketCurrencyPairRequestSyncingService
    {
        bool IsRequestNeeded(CurrencyPairRequest cpr);

        Task<bool> Process(CurrencyPairRequest cpr);
    }
}