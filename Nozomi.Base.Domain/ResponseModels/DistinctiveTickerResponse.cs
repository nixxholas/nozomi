namespace Nozomi.Data.ResponseModels
{
    public class DistinctiveTickerResponse : TickerResponse
    {
        public string Exchange { get; set; }
        public string ExchangeAbbrv { get; set; }
    }
}