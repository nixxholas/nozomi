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

        ICollection<RequestComponent> GetByMainCurrency(string mainCurrencyAbbrv, 
            ICollection<ComponentType> componentTypes);
        
        NozomiResult<RequestComponent> Get(long id, bool includeNested = false);
    }
}