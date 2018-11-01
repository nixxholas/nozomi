using System.Collections;
using System.Collections.Generic;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.WebModels;

namespace Nozomi.Ticker.Areas.v1.CurrencyPairComponent
{
    public interface ICurrencyPairComponentController
    {
        NozomiResult<ICollection<RequestComponent>> AllByRequestId(long requestId, bool includeNested = false);

        NozomiResult<ICollection<RequestComponent>> All(bool includeNested = false);

        NozomiResult<string> Create(CreateCurrencyPairComponent createCurrencyPairComponent);

        NozomiResult<string> Update(UpdateCurrencyPairComponent updateCurrencyPairComponent);

        NozomiResult<string> Delete(long id, long userId = 0, bool hardDelete = false);
    }
}