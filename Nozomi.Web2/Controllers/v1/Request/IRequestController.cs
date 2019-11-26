using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Web2.Controllers.v1.Request
{
    public interface IRequestController
    {
        NozomiResult<JsonResult> All(bool includeNested);
    }
}
