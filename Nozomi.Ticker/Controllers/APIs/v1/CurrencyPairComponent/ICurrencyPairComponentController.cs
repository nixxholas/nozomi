using System.Collections.Generic;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.AreaModels.v1.RequestComponent;
using Nozomi.Data.Models.Web;

namespace Nozomi.Ticker.Controllers.APIs.v1.CurrencyPairComponent
{
    public interface ICurrencyPairComponentController
    {
        NozomiResult<ICollection<RequestComponent>> AllByRequestId(long requestId, bool includeNested = false);

        NozomiResult<ICollection<RequestComponent>> All(int index = 0, bool includeNested = false);

        NozomiResult<string> Create(CreateRequestComponent createRequestComponent);

        NozomiResult<string> Update(UpdateCurrencyPairComponent updateCurrencyPairComponent);

        NozomiResult<string> Delete(long id, long userId = 0, bool hardDelete = false);
    }
}