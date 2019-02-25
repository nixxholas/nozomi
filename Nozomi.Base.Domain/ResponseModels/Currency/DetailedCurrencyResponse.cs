using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.ResponseModels.Currency
{
    public class DetailedCurrencyResponse : DistinctiveCurrencyResponse
    {
        public IDictionary<ComponentType, IDictionary<DateTime, string>> Historical { get; set; }
    }
}