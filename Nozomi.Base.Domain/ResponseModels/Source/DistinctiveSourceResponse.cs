namespace Nozomi.Data.ResponseModels.Source
{
    public class DistinctiveSourceResponse
    {
        public string Name { get; set; }
        
        public string Abbreviation { get; set; }
        
        public decimal DailyVolume { get; set; }
        
        public long TickerCount { get; set; }
    }
}