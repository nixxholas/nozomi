using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Ticker.Areas.v1.CurrencyPairRequest
{
    public interface ICurrencyPairRequestController
    {
        NozomiResult<JsonResult> All(bool includeNested);

        NozomiResult<JsonResult> Create();

        NozomiResult<JsonResult> Update();

        NozomiResult<JsonResult> Delete();

        NozomiResult<JsonResult> ManualPoll();
    }
}