namespace Nozomi.Data.Models.Currency
{
    /// <summary>
    /// Partial currency pair.
    /// </summary>
    public class PartialCurrencyPair
    {
        public long CurrencyId { get; set; }

        public long CurrencyPairId { get; set; }

        public bool IsMain { get; set; } = false;

        public CurrencyPair CurrencyPair { get; set; }
        public Currency Currency { get; set; }
    }
}
