using System.Collections.Generic;

namespace Nozomi.Data.ViewModels.Admin.Currency
{
    public class CurrencyViewModel
    {
        /// <summary>
        /// Stores all currency data to the currency.
        /// </summary>
        public Models.Categorisation.Item Item { get; set; }
        
        public IEnumerable<Models.Categorisation.Source> CurrencySourcesOptions { get; set; }
        
        public ICollection<Models.Categorisation.ItemType> CurrencyTypes { get; set; }
        
        public ICollection<Models.Categorisation.ItemPair> CurrencyPairs { get; set; }
    }
}