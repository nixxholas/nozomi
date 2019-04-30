using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Ticker.Controllers.APIs.v1.RequestPropertyType
{
    public interface IRequestPropertyTypeController
    {
        NozomiResult<JsonResult> All();
    }
}