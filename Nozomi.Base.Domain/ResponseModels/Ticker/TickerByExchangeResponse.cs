namespace Nozomi.Data.ResponseModels.Ticker
{
    public class TickerByExchangeResponse : TickerResponse
    {
        public string Exchange { get; set; }
        public string ExchangeAbbrv { get; set; }
    }
}