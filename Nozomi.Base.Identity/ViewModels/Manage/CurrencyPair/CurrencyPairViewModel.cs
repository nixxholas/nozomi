using System.Collections.Generic;

namespace Nozomi.Base.Identity.ViewModels.Manage.CurrencyPair
{
    public class CurrencyPairViewModel
    {
        public ICollection<Data.Models.Currency.CurrencyPair> CurrencyPairs { get; set; }
    }
}