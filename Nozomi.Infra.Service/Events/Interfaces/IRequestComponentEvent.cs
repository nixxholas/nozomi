using System;
using System.Collections.Generic;
using Nozomi.Data;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IRequestComponentEvent
    {
        ICollection<RequestComponent> AllByRequestId(long requestId, bool includeNested = false);

        ICollection<RequestComponent> All(int index = 0, bool includeNested = false);

        decimal ComputeDifference(string baseCurrencyAbbrv, string comparingCurrencyAbbrv,
            ComponentType componentType);

        ICollection<RequestComponent> GetByMainCurrency(string mainCurrencyAbbrv, 
            ICollection<ComponentType> componentTypes);

        /// <summary>
        /// Allows the caller to obtain all RequestComponents relevant to the currency
        /// pair in question via the abbreviation method. (i.e. ETHUSD)
        /// </summary>
        /// <param name="analysedComponentId">The unique identifier of the analysed component
        /// that is related to the ticker in question.</param>
        /// <returns>Collection of request components related to the component</returns>
        ICollection<RequestComponent> GetAllByCorrelation(long analysedComponentId,
            Func<RequestComponent, bool> predicate = null);

        NozomiResult<RequestComponent> Get(long id, bool includeNested = false);
    }
}