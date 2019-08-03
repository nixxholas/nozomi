using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Web.Controllers.APIs.v1.Request
{
    [ApiController]
    public class RequestController : BaseApiController<RequestController>, IRequestController
    {
        private readonly IRequestEvent _requestEvent;

        public RequestController(ILogger<RequestController> logger, UserManager<User> userManager,
            IRequestEvent requestEvent) : base(logger, userManager)
        {
            _requestEvent = requestEvent;
        }

        [Authorize(Roles = "Staff")]
        [HttpGet]
        public NozomiResult<JsonResult> All(bool includeNested)
        {
            var res = _requestEvent.GetAllActive();

            return new NozomiResult<JsonResult>()
            {
                ResultType = res != null ? NozomiResultType.Success : NozomiResultType.Failed,
                Data = res != null ? new JsonResult(res) :
                    new JsonResult("We're currently unable to obtain all of the Currency Pair Requests.")
            };
        }
    }
}
