using System.Collections;
using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ICurrencyCurrencyPairEvent
    {
        ICollection<CurrencyCurrencyPair> ObtainCounterCurrencyPairs(ICollection<CurrencyCurrencyPair> mainCCPs);
    }
}