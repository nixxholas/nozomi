using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairRequest;
using Nozomi.Data.AreaModels.v1.Requests;

namespace Nozomi.Ticker.Controllers.APIs.v1.Request
{
    public interface IRequestController
    {
        NozomiResult<JsonResult> All(bool includeNested);

        NozomiResult<string> Create(CreateRequest obj, string userId = null);

        NozomiResult<string> Update(UpdateRequest obj, string userId = null);

        NozomiResult<string> Delete(long id, bool hardDelete = false, string userId = null);

        NozomiResult<JsonResult> ManualPoll(long requestId, string userId = null);
    }
}