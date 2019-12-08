using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Preprocessing;

namespace Nozomi.Web.Controllers.APIs.v1.ResponseType
{
    public class ResponseTypeController : BaseApiController<ResponseTypeController>, IResponseTypeController
    {
        public ResponseTypeController(ILogger<ResponseTypeController> logger) : base(logger)
        {
        }

        [Authorize]
        [HttpGet]
        public NozomiResult<JsonResult> All()
        {
            return new NozomiResult<JsonResult>(new JsonResult(NozomiServiceConstants.responseTypes));
        }
    }
}
