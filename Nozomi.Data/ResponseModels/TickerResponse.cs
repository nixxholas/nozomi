using System;
using System.Collections.Generic;

namespace Nozomi.Data.ResponseModels
{
    public class TickerResponse
    {
        public DateTime LastUpdated { get; set; }
        
        public Dictionary<string, string> Properties { get; set; }
    }
}