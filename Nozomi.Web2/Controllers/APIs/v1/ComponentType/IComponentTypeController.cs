using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Web2.Controllers.APIs.v1.ComponentType
{
    public interface IComponentTypeController
    {
        NozomiResult<JsonResult> All();
    }
}