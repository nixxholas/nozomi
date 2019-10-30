using System.Collections.Generic;
using Nozomi.Data;
using Nozomi.Data.Models.Web;

namespace Nozomi.Web.Controllers.APIs.v1.CurrencyPairComponent
{
    public interface ICurrencyPairComponentController
    {
        NozomiResult<ICollection<Data.Models.Web.RequestComponent>> AllByRequestId(long requestId, bool includeNested = false);

        NozomiResult<ICollection<Data.Models.Web.RequestComponent>> All(int index = 0, bool includeNested = false);
    }
}
