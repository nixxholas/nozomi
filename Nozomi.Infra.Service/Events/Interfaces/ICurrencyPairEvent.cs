using System.Collections.Generic;
using Nozomi.Base.Core;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ICurrencyPairEvent
    {
        ICollection<CurrencyPair> GetAllByCounterCurrency(
            string counterCurrencyAbbrv = CoreConstants.GenericCounterCurrency);

        ICollection<CurrencyPair> GetAllByTickerPairAbbreviation(string tickerPairAbbreviation, bool track = false);

        ICollection<AnalysedComponent> GetAnalysedComponents(long analysedComponentId, bool track = false);
    }
}