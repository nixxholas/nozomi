using System.Collections.Generic;
using Nozomi.Data.Models.Analytical;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Ticker.Areas.Users.Controllers.Manage.Currency
{
    public class CurrenciesViewModel
    {
        public ICollection<Data.Models.Currency.Currency> Currencies { get; set; }
        
        public ICollection<CurrencyType> CurrencyTypes { get; set; }
        
        public IEnumerable<Source> CurrencySources { get; set; }
        
        public IEnumerable<AnalysedComponent> AnalysedComponents { get; set; }
    }
}