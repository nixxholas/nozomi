using Nozomi.Data.ResponseModels.Currency;

namespace Nozomi.Data.ViewModels.Admin.Currency
{
    public class CurrencyViewModel
    {
        /// <summary>
        /// Stores all currency data to the currency.
        /// </summary>
        public AbbrvUniqueCurrencyResponse Currency { get; set; }
    }
}