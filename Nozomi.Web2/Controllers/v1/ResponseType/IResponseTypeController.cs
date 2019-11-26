using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Web2.Controllers.v1.ResponseType
{
    public interface IResponseTypeController
    {
        NozomiResult<JsonResult> All();
    }
}
