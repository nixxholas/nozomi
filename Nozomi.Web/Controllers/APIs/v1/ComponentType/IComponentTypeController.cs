using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Web.Controllers.APIs.v1.ComponentType
{
    public interface IComponentTypeController
    {
        NozomiResult<JsonResult> All();
    }
}