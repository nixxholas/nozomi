using System.Collections;
using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Infra.Admin.Service.Events.Interfaces
{
    public interface CurrencyPairSourceCurrencyAdminEvent
    {
        ICollection<CurrencyPairSourceCurrency> GetCounterCurrenciesByAbbreviation(string mainAbbreviation);
    }
}