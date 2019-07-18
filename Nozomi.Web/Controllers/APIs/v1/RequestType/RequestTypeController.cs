using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Enumerators.Interfaces;

namespace Nozomi.Ticker.Controllers.APIs.v1.RequestType
{
    public class RequestTypeController : BaseController<RequestTypeController>, IRequestTypeController
    {
        private readonly IRequestTypeEvent _requestTypeEvent;

        public RequestTypeController(ILogger<RequestTypeController> logger, NozomiUserManager userManager,
            IRequestTypeEvent requestTypeEvent) : base(logger, userManager)
        {
            _requestTypeEvent = requestTypeEvent;
        }

        [HttpGet]
        public NozomiResult<JsonResult> All()
        {
            return new NozomiResult<JsonResult>(new JsonResult(_requestTypeEvent.All()));
        }
    }
}
