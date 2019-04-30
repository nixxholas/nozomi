using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Ticker.Controllers.APIs.v1.RequestType
{
    public interface IRequestTypeController
    {
        NozomiResult<JsonResult> All();
    }
}