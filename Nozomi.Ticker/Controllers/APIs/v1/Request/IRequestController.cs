using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairRequest;

namespace Nozomi.Ticker.Controllers.APIs.v1.Request
{
    public interface IRequestController
    {
        NozomiResult<JsonResult> All(bool includeNested);

        NozomiResult<string> Create(CreateCurrencyPairRequest obj, long userId = 0);

        NozomiResult<JsonResult> Update(UpdateCurrencyPairRequest obj, long userId = 0);

        NozomiResult<string> Delete(long id, bool hardDelete = false, long userId = 0);

        NozomiResult<JsonResult> ManualPoll(long requestId, long userId = 0);
    }
}