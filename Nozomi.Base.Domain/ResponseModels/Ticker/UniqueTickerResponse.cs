namespace Nozomi.Data.ResponseModels.Ticker
{
    public class UniqueTickerResponse : TickerByCurrencyPairResponse
    {   
        public string MainTickerAbbreviation { get; set; }
        
        public string MainTickerName { get; set; }
        
        public string CounterTickerAbbreviation { get; set; }
        
        public string CounterTickerName { get; set; }
    }
}