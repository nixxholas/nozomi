namespace Nozomi.Data.ResponseModels.Ticker
{
    public class DistinctiveTickerResponse : TickerResponse
    {
        public string Exchange { get; set; }
        public string ExchangeAbbrv { get; set; }
    }
}