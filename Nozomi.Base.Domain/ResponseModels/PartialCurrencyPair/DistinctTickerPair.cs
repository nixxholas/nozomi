using Nozomi.Data.ResponseModels.Source;

namespace Nozomi.Data.ResponseModels.PartialCurrencyPair
{
    public class DistinctTickerPair : SourceResponse
    {
        public string MainTickerName { get; set; }
        
        public string MainTicker { get; set; }
        
        public string CounterTickerName { get; set; }
        
        public string CounterTicker { get; set; }
    }
}