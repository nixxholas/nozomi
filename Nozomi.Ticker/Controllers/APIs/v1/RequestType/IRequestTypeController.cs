using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Ticker.Areas.v1.RequestType
{
    public interface IRequestTypeController
    {
        NozomiResult<JsonResult> All();
    }
}