using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairRequest;
using Nozomi.Data.AreaModels.v1.Requests;

namespace Nozomi.Ticker.Controllers.APIs.v1.Request
{
    public interface IRequestController
    {
        NozomiResult<JsonResult> All(bool includeNested);

        NozomiResult<string> Create(CreateRequest obj, long userId = 0);

        NozomiResult<string> Update(UpdateRequest obj, long userId = 0);

        NozomiResult<string> Delete(long id, bool hardDelete = false, long userId = 0);

        NozomiResult<JsonResult> ManualPoll(long requestId, long userId = 0);
    }
}