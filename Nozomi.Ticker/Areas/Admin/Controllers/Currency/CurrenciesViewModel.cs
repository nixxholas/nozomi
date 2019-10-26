using System.Collections.Generic;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Ticker.Areas.Admin.Controllers.Currency
{
    public class CurrenciesViewModel
    {
        public ICollection<Data.Models.Currency.Currency> Currencies { get; set; }
        
        public ICollection<CurrencyType> CurrencyTypes { get; set; }
        
        public IEnumerable<Data.Models.Currency.Source> CurrencySources { get; set; }
        
        public IEnumerable<AnalysedComponent> AnalysedComponents { get; set; }
    }
}