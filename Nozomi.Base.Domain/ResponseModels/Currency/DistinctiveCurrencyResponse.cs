using System;

namespace Nozomi.Data.ResponseModels.Currency
{
    public class DistinctiveCurrencyResponse
    {
        public string Name { get; set; }
        
        public string Abbreviation { get; set; }
        
        public DateTime LastUpdated { get; set; }
        
        public decimal WeeklyAvgPrice { get; set; }
        
        public decimal DailyVolume { get; set; }
    }
}