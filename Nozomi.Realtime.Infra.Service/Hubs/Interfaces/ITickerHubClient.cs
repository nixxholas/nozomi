using System.Collections.Generic;
using Nozomi.Data.ResponseModels;

namespace Nozomi.Realtime.Infra.Service.Hubs.Interfaces
{
    /// <summary>
    /// ITickerHubClient
    ///
    /// Interface that declares the endpoints for the Client.
    ///
    /// These methods are invoked by the server.
    /// </summary>
    public interface ITickerHubClient
    {
        void Tickers(IDictionary<KeyValuePair<string, string>, DistinctiveTickerResponse> tickers);
    }
}