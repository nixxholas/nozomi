using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Web.Controllers.APIs.v1.RequestPropertyType
{
    public interface IRequestPropertyTypeController
    {
        NozomiResult<JsonResult> All();
    }
}