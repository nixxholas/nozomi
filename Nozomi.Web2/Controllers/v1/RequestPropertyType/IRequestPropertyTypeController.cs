using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Web2.Controllers.v1.RequestPropertyType
{
    public interface IRequestPropertyTypeController
    {
        NozomiResult<JsonResult> All();
    }
}