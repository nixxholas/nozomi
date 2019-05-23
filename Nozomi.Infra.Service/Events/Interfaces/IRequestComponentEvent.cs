using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nozomi.Data;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IRequestComponentEvent
    {
        ICollection<RequestComponent> All(int index = 0, bool includeNested = false);

        /// <summary>
        /// Allows the caller to obtain all RequestComponents relevant to the currency
        /// pair in question via the abbreviation method. (i.e. ETHUSD)
        /// </summary>
        /// <param name="analysedComponentId">The unique identifier of the analysed component
        /// that is related to the ticker in question.</param>
        /// <returns>Collection of request components related to the component</returns>
        ICollection<RequestComponent> GetAllByCorrelation(long analysedComponentId, bool track = false, int index = 0);

        /// <summary>
        /// Obtains all RequestComponents relevant to the currency given, utilizing it as
        /// the base currency of the tickers to be queried. (i.e USD => USDETH, USDBTC)
        /// </summary>
        /// <param name="currencyId">The unique identifier of the base currency</param>
        /// <returns>Collection of request components related to the currency</returns>
        ICollection<Data.Models.Web.RequestComponent> GetAllByCurrency(long currencyId, bool track = false);

        ICollection<RequestComponent> GetAllTickerPairCompsByCurrency(long currencyId, bool track = false);

        ICollection<RequestComponent> GetAllByRequest(long requestId, bool includeNested = false);

        ICollection<RequestComponent> GetByMainCurrency(string mainCurrencyAbbrv, 
            ICollection<ComponentType> componentTypes);
        
        NozomiResult<RequestComponent> Get(long id, bool includeNested = false);
    }
}