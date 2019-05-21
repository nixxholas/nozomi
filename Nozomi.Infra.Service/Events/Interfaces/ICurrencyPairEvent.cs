using System.Collections.Generic;
using Nozomi.Base.Core;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ICurrencyPairEvent
    {
        ICollection<CurrencyPair> GetAllByCounterCurrency(
            string counterCurrencyAbbrv = CoreConstants.GenericCounterCurrency);

        ICollection<CurrencyPair> GetAllByTickerPairAbbreviation(string tickerPairAbbreviation, bool track = false);
    }
}