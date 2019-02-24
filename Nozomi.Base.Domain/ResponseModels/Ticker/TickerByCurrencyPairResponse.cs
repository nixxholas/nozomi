namespace Nozomi.Data.ResponseModels.Ticker
{
    public class TickerByCurrencyPairResponse : TickerByExchangeResponse
    {
        public long CurrencyPairId { get; set; }
    }
}