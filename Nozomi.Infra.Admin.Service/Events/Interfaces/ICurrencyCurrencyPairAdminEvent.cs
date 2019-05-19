using System.Collections;
using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Infra.Admin.Service.Events.Interfaces
{
    public interface ICurrencyCurrencyPairAdminEvent
    {
        ICollection<CurrencyPairSourceCurrency> GetCounterCurrenciesByAbbreviation(string mainAbbreviation);
    }
}