using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.ViewModels.Admin.Currency
{
    public class CurrencyViewModel
    {
        /// <summary>
        /// Stores all currency data to the currency.
        /// </summary>
        public Models.Currency.Currency Currency { get; set; }
        
        public IEnumerable<Models.Currency.Source> CurrencySourcesOptions { get; set; }
        
        public ICollection<Models.Currency.CurrencyType> CurrencyTypes { get; set; }
        
        public ICollection<Models.Currency.CurrencyPair> CurrencyPairs { get; set; }
    }
}