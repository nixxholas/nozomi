namespace Nozomi.Data.ResponseModels.Ticker
{
    public class UniqueTickerResponse : TickerByCurrencyPairResponse
    {
        public string TickerAbbreviation { get; set; }
    }
}