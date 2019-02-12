using System;

namespace Nozomi.Data.ResponseModels
{
    public class DistinctiveCurrencyResponse
    {
        public string Name { get; set; }
        
        public string Abbreviation { get; set; }
        
        public DateTime LastUpdated { get; set; }
        
        public decimal Price { get; set; }
        
        public decimal DailyVolume { get; set; }
    }
}