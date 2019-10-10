using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Ticker.Controllers.APIs.v1.RequestType
{
    public class RequestTypeController : BaseController<RequestTypeController>, IRequestTypeController
    {
        private readonly IRequestTypeEvent _requestTypeEvent;
        
        public RequestTypeController(ILogger<RequestTypeController> logger, UserManager<User> userManager,
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