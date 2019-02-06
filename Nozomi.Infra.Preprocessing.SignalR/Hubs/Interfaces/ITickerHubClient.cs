using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Nozomi.Infra.Preprocessing.SignalR.Hubs.Interfaces
{
    public interface ITickerHubClient
    {
        /// <summary>
        /// Dispatches 
        /// </summary>
        /// <returns></returns>
        Task Tickers();
        Task BroadcastData(JObject data);
    }
}