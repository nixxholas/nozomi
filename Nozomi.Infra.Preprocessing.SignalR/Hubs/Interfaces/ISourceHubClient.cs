using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Data.ResponseModels;

namespace Nozomi.Infra.Preprocessing.SignalR.Hubs.Interfaces
{
    public interface ISourceHubClient
    {
        /// <summary>
        /// Dispatches the tickers relevant to the specific exchange 
        /// </summary>
        /// <returns></returns>
        Task Tickers(ICollection<UniqueTickerResponse> tickers);
    }
}