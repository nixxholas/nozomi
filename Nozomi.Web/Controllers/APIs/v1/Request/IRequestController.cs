using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Web.Controllers.APIs.v1.Request
{
    public interface IRequestController
    {
        NozomiResult<JsonResult> All(bool includeNested);
    }
}
