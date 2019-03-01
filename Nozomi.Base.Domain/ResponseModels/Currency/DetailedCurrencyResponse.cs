using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.ResponseModels.Currency
{
    public class DetailedCurrencyResponse : DistinctiveCurrencyResponse
    {
        public Dictionary<ComponentType, List<ComponentHistoricalDatum>> Historical { get; set; }
    }
}