using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nozomi.Data.ResponseModels;

namespace Nozomi.Infra.Preprocessing.SignalR.Hubs.Interfaces
{
    public interface ITickerHubClient
    {
        /// <summary>
        /// Dispatches 
        /// </summary>
        /// <returns></returns>
        Task Tickers(ICollection<UniqueTickerResponse> tickers);
        Task BroadcastData(JObject data);
    }
}