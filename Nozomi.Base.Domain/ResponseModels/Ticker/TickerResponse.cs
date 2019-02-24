using System;
using System.Collections.Generic;

namespace Nozomi.Data.ResponseModels.Ticker
{
    public class TickerResponse
    {
        public DateTime LastUpdated { get; set; }
        
        public ICollection<KeyValuePair<string, string>> Properties { get; set; }
    }
}