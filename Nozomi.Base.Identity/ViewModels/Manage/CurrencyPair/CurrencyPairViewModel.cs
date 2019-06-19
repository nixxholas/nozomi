using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Base.Identity.ViewModels.Manage.CurrencyPair
{
    public class CurrencyPairViewModel
    {
        public Data.Models.Currency.CurrencyPair CurrencyPair { get; set; }
        
        public IEnumerable<Data.Models.Currency.Currency> Currencies { get; set; }
        
        public List<KeyValuePair<string, int>> CurrencyPairTypes { get; set; }
        
        public IEnumerable<Source> Sources { get; set; }
        
    }
}