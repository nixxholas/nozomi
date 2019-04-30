using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Ticker.Areas.v1.ComponentType
{
    public interface IComponentTypeController
    {
        NozomiResult<JsonResult> All();
    }
}