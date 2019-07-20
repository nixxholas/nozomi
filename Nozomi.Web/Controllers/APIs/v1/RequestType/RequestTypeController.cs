using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Data;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Enumerators.Interfaces;

namespace Nozomi.Web.Controllers.APIs.v1.RequestType
{
    public class RequestTypeApiController : BaseApiController<RequestTypeApiController>, IRequestTypeController
    {
        private readonly IRequestTypeEvent _requestTypeEvent;

        public RequestTypeApiController(ILogger<RequestTypeApiController> logger, UserManager<User> userManager,
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
