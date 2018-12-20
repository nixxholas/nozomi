using System;
using System.Collections.Generic;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.WebModels;

namespace Nozomi.Data.ResponseModels
{
    public class TickerResponse
    {
        public DateTime LastUpdated { get; set; }
        
        public ICollection<KeyValuePair<string, string>> Properties { get; set; }
    }
}