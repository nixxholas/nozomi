using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairRequest;

namespace Nozomi.Ticker.Controllers.APIs.v1.CurrencyPairRequest
{
    public interface ICurrencyPairRequestController
    {
        NozomiResult<JsonResult> All(bool includeNested);

        NozomiResult<JsonResult> Create(CreateCurrencyPairRequest obj, long userId = 0);

        NozomiResult<JsonResult> Update(UpdateCurrencyPairRequest obj, long userId = 0);

        NozomiResult<JsonResult> Delete(long id, bool hardDelete = false, long userId = 0);

        NozomiResult<JsonResult> ManualPoll(long requestId, long userId = 0);
    }
}