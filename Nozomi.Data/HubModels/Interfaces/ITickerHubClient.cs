using System.Diagnostics;
using System.Threading.Channels;
using System.Threading.Tasks;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Data.HubModels.Interfaces
{
    public interface ITickerHubClient
    {
        Task<ChannelReader<NozomiResult<CurrencyPair>>> ReturnPayload();
    }
}