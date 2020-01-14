using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nozomi.Data;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Component;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IComponentEvent
    {
        IEnumerable<ComponentViewModel> All(string requestGuid, int index = 0, int itemsPerIndex = 50,
            bool includeNested = false, string userId = null);
        
        IEnumerable<ComponentViewModel> All(long requestId, int index = 0, int itemsPerIndex = 50,
            bool includeNested = false, string userId = null);
        
        IEnumerable<ComponentViewModel> All(int index = 0, int itemsPerIndex = 50, bool includeNested = false);
        
        ICollection<Component> All(int index = 0, bool includeNested = false);

        long GetPredicateCount(Expression<Func<Component, bool>> predicate);

        long GetCorrelationPredicateCount(long analysedComponentId, Expression<Func<Component, bool>> predicate);

        /// <summary>
        /// Allows the caller to obtain all RequestComponents relevant to the currency
        /// pair in question via the abbreviation method. (i.e. ETHUSD)
        /// </summary>
        /// <param name="analysedComponentId">The unique identifier of the analysed component
        /// that is related to the ticker in question.</param>
        /// <returns>Collection of request components related to the component</returns>
        ICollection<Component> GetAllByCorrelation(long analysedComponentId, bool track = false, int index = 0, 
            bool ensureValid = true, ICollection<ComponentType> componentTypes = null);

        /// <summary>
        /// Obtains all RequestComponents relevant to the currency given, utilizing it as
        /// the base currency of the tickers to be queried. (i.e USD => USDETH, USDBTC)
        /// </summary>
        /// <param name="currencyId">The unique identifier of the base currency</param>
        /// <returns>Collection of request components related to the currency</returns>
        ICollection<Data.Models.Web.Component> GetAllByCurrency(long currencyId, bool track = false, int index = 0);

        ICollection<Component> GetAllTickerPairCompsByCurrency(long currencyId, bool track = false, int index = 0);

        ICollection<Component> GetAllByRequest(long requestId, bool includeNested = false);

        IEnumerable<ComponentViewModel> GetAllByRequest(string guid, bool includeNested = false, 
            int index = 0, int itemsPerPage = 50);

        ICollection<Component> GetByMainCurrency(string mainCurrencyAbbrv, 
            ICollection<ComponentType> componentTypes);
        
        NozomiResult<Component> Get(long id, bool includeNested = false);
    }
}