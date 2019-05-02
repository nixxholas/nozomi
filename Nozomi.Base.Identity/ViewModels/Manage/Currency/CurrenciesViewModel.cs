using System.Collections;
using System.Collections.Generic;

namespace Nozomi.Base.Identity.ViewModels.Manage.Currency
{
    public class CurrenciesViewModel
    {
        public ICollection<Data.Models.Currency.Currency> Currencies { get; set; }
    }
}