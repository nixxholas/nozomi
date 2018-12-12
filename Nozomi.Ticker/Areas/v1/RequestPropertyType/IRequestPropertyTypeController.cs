using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Ticker.Areas.v1.RequestPropertyType
{
    public interface IRequestPropertyTypeController
    {
        NozomiResult<JsonResult> All();
    }
}