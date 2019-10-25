using System.Collections.Generic;
using Nozomi.Data;
using Nozomi.Data.Models;

namespace Nozomi.Web.Controllers.APIs.v1.CurrencyPairComponent
{
    public interface ICurrencyPairComponentController
    {
        NozomiResult<ICollection<RequestComponent>> AllByRequestId(long requestId, bool includeNested = false);

        NozomiResult<ICollection<RequestComponent>> All(int index = 0, bool includeNested = false);
    }
}
