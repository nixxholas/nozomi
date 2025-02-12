using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Preprocessing.Attributes;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Web2.Controllers.v1.RequestType
{
    public class RequestTypeController : BaseApiController<RequestTypeController>, IRequestTypeController
    {
        private readonly IRequestTypeEvent _requestTypeEvent;

        public RequestTypeController(ILogger<RequestTypeController> logger,
            IRequestTypeEvent requestTypeEvent) : base(logger)
        {
            _requestTypeEvent = requestTypeEvent;
        }

        [Authorize]
        [HttpGet]
        [Throttle(Name = "RequestType/All", Milliseconds = 1000)]
        public NozomiResult<JsonResult> All()
        {
            return new NozomiResult<JsonResult>(new JsonResult(_requestTypeEvent.All()));
        }
    }
}
