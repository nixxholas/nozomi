using System.Collections;
using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Base.Identity.ViewModels.Manage.Currency
{
    public class CurrenciesViewModel
    {
        public ICollection<Data.Models.Currency.Currency> Currencies { get; set; }
        
        public ICollection<CurrencyType> CurrencyTypes { get; set; }
        
        public IEnumerable<Source> CurrencySources { get; set; }
    }
}