using System.Collections;
using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Infra.Admin.Service.Events.Interfaces
{
    public interface ICurrencyPairSourceCurrencyAdminEvent
    {
        ICollection<Currency> GetCounterCurrenciesByAbbreviation(string mainAbbreviation);
    }
}