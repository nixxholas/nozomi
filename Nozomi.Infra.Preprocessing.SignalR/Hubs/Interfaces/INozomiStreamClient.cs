using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Data.ResponseModels.CurrencyType;
using Nozomi.Data.ResponseModels.Ticker;

namespace Nozomi.Infra.Preprocessing.SignalR.Hubs.Interfaces
{
    public interface INozomiStreamClient
    {
        Task Currencies(ICollection<GeneralisedCurrencyResponse> currencies);
        /// <summary>
        /// Dispatches 
        /// </summary>
        /// <returns></returns>
        Task Tickers(ICollection<UniqueTickerResponse> tickers);

        Task CurrencyTypes(ICollection<CurrencyTypeResponse> currencyTypes);
        Task BroadcastData(JObject data);
    }
}