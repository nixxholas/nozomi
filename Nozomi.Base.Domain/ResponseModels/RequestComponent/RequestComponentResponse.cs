using System;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.ResponseModels.RequestComponent
{
    public class RequestComponentResponse
    {
        public string Name { get; set; }
        
        public DateTime Timestamp { get; set; }
        
        public string Value { get; set; }
    }
}