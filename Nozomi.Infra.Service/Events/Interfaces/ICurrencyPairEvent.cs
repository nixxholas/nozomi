using System.Collections.Generic;
using Nozomi.Base.Core;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels.CurrencyPair;
using Nozomi.Data.ViewModels.CurrencyPair;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ICurrencyPairEvent
    {
        IEnumerable<CurrencyPairViewModel> All(int page = 0, int itemsPerPage = 50, string sourceGuid = null, 
            string mainTicker = null, bool orderAscending = true, string orderingParam = "Name");
        
        long GetCount(string mainTicker = null);
        
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

        CurrencyPair Get(long id, bool track = false, string userId = null);

        ICollection<DistinctCurrencyPairResponse> ListAll();
    }
}