namespace Nozomi.Data.ResponseModels.CurrencyPair
{
    public class DistinctCurrencyPairResponse : CurrencyPairResponse
    {
        public string MainTicker { get; set; }
        
        public string CounterTicker { get; set; }
        
        public string SourceAbbreviation { get; set; }
        
        public string SourceName { get; set; }
    }
}