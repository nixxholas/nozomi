using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Base.Identity.ViewModels.Manage.CurrencyPair
{
    public class CurrencyPairsViewModel
    {
        public ICollection<Data.Models.Currency.CurrencyPair> CurrencyPairs { get; set; }
        
        public IEnumerable<Data.Models.Currency.Currency> Currencies { get; set; }
        
        public IEnumerable<Source> Sources { get; set; }
        
        public List<KeyValuePair<string, int>> CurrencyPairTypes { get; set; }
    }
}