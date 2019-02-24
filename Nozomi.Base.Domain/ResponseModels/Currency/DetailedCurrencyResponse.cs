using System;
using System.Collections.Generic;

namespace Nozomi.Data.ResponseModels.Currency
{
    public class DetailedCurrencyResponse : DistinctiveTickerResponse
    {
        // Stores the top X days of volume requested.
        public IDictionary<DateTime, string> HistoricalVolume { get; set; }
    }
}