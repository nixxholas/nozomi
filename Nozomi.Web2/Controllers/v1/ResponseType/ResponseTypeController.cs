using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Attributes;

namespace Nozomi.Web2.Controllers.v1.ResponseType
{
    public class ResponseTypeController : BaseApiController<ResponseTypeController>, IResponseTypeController
    {
        public ResponseTypeController(ILogger<ResponseTypeController> logger) : base(logger)
        {
        }

        [Authorize]
        [HttpGet]
        [Throttle(Name = "ResponseType/All", Milliseconds = 1000)]
        public NozomiResult<JsonResult> All()
        {
            return new NozomiResult<JsonResult>(new JsonResult(NozomiServiceConstants.responseTypes));
        }
    }
}
