using System.Diagnostics;
using System.Threading.Tasks;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Data.HubModels.Interfaces
{
    public interface ITickerHubClient
    {
        Task<NozomiResult<CurrencyPair>> ReturnPayload();
    }
}