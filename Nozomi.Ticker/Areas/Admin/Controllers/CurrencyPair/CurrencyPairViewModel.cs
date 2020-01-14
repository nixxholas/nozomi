using System.Collections.Generic;

namespace Nozomi.Ticker.Areas.Admin.Controllers.CurrencyPair
{
    public class CurrencyPairViewModel
    {
        public Data.Models.Currency.CurrencyPair CurrencyPair { get; set; }
        
        public IEnumerable<Data.Models.Currency.Currency> Currencies { get; set; }
        
        public List<KeyValuePair<string, int>> CurrencyPairTypes { get; set; }
        
        public IEnumerable<Data.Models.Currency.Source> Sources { get; set; }
    }
}