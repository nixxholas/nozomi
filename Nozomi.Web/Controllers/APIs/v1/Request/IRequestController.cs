using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairRequest;
using Nozomi.Data.AreaModels.v1.Requests;

namespace Nozomi.Ticker.Controllers.APIs.v1.Request
{
    public interface IRequestController
    {
        NozomiResult<JsonResult> All(bool includeNested);
    }
}
