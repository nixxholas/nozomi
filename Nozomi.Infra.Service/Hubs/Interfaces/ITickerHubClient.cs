using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Data;
using Nozomi.Data.CurrencyModels;
using Nozomi.Preprocessing.Hubs.Enumerators;

namespace Nozomi.Service.Hubs.Interfaces
{
    public interface ITickerHubClient
    {
        void Register(TickerHubGroup hubGroup);

        void Unregister(TickerHubGroup hubGroup);
        
        Task<NozomiResult<IEnumerable<CurrencyPair>>> Tickers(IEnumerable<CurrencyPair> currencyPairs = null);
        
        Task SubscribeToAll();
        
        Task BroadcastTickerUpdate();
    }
}