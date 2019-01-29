using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Data;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.ResponseModels;
using Nozomi.Preprocessing.Hubs.Enumerators;

namespace Nozomi.Service.Hubs.Interfaces
{
    public interface ITickerHubClient
    {
        /// <summary>
        /// Centralized method/function capable of traversing to
        /// the appropriate endpoint of TickerHub.
        /// </summary>
        /// <param name="hubGroup"></param>
        void BroadcastData(TickerHubGroup hubGroup);
        
        void Register(TickerHubGroup hubGroup);

        void Unregister(TickerHubGroup hubGroup);
        
        Task<IDictionary<KeyValuePair<string, string>, DistinctiveTickerResponse>> Tickers();
    }
}