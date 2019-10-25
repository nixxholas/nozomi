using System.Collections.Generic;
using Nozomi.Base.Core;
using Nozomi.Data.Models.Analytical;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ResponseModels.CurrencyPair;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ICurrencyPairEvent
    {
        ICollection<CurrencyPair> GetAllByCounterCurrency(
            string counterCurrencyAbbrv = CoreConstants.GenericCounterCurrency);
        
        ICollection<CurrencyPair> GetAllByMainCurrency(
            string mainCurrencyAbbrv = CoreConstants.GenericCurrency);

        ICollection<CurrencyPair> GetAllByTickerPairAbbreviation(string tickerPairAbbreviation, bool track = false);

        /// <summary>
        /// This API could be a little confusing.. Basically allows you to look for the Currency Pair in question,
        /// obtain all of its related AnalysedComponents, obtain the specific type. If its not available,
        /// a null value will be returned of course.
        /// </summary>
        /// <param name="analysedComponentId"></param>
        /// <param name="type"></param>
        /// <param name="track"></param>
        /// <returns></returns>
        AnalysedComponent GetRelatedAnalysedComponent(long analysedComponentId, AnalysedComponentType type, bool track = false);

        ICollection<AnalysedComponent> GetAnalysedComponents(long analysedComponentId, bool track = false);
        ICollection<CurrencyPair> GetAll();

        CurrencyPair Get(long id, bool track = false, long userId = 0);

        ICollection<DistinctCurrencyPairResponse> ListAll();
    }
}