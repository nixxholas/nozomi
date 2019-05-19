using System.Collections;
using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ICurrencyCurrencyPairEvent
    {
        ICollection<CurrencyPairSourceCurrency> ObtainCounterCurrencyPairs(ICollection<CurrencyPairSourceCurrency> mainCCPs);
    }
}