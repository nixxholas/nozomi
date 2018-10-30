using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Data;
using Nozomi.Data.ResponseModels;

namespace Nozomi.Ticker.Areas.v1.CurrencyPair
{
    public interface ICurrencyPairController
    {
        /// <summary>
        /// Creates a currency pair with the following parameters and child objects.
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/v1/CurrencyPair/Create
        ///     {
        ///        "id": 1,
        ///        "apiUrl": "https://coinhako.com/api/v2",
        ///        "DefaultComponent": "0",
        ///        "CurrencySourceId": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="currencyPair">Object containing the currency pair information.</param>
        /// <returns>NozomiResult</returns>
        Task<NozomiResult<Data.CurrencyModels.CurrencyPair>> Create(Data.CurrencyModels.CurrencyPair currencyPair);

        /// <summary>
        /// An explicitly-defined variation of the GET Ticker API, allows the user to explicitly call a single ticker
        /// when the primary key is known to the caller.
        /// </summary>
        /// <param name="id">Unique identifier </param>
        /// <returns></returns>
        Task Ticker(long id);

        /// <summary>
        /// Returns a list of prices from various exchanges (if available) for the specific Ticker Abbreviation.
        /// </summary>
        /// <param name="abbreviation">The abbreviation of the ticker, i.e. ETHUSD</param>
        /// <returns>Ticker Price Array</returns>
        NozomiResult<ICollection<DistinctiveTickerResponse>> Ticker(string abbreviation, string exchangeAbbrv = null);
    }
}