using System.Collections.Generic;
using Nozomi.Data;

namespace Nozomi.Web2.Controllers.v1.CurrencyPairComponent
{
    public interface ICurrencyPairComponentController
    {
        NozomiResult<ICollection<Data.Models.Web.Component>> AllByRequestId(long requestId, bool includeNested = false);

        NozomiResult<ICollection<Data.Models.Web.Component>> All(int index = 0, bool includeNested = false);
    }
}
