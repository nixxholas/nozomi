using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.ViewModels.Request;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Web2.Controllers.APIs.v1.Request
{
    [ApiController]
    public class RequestController : BaseApiController<RequestController>, IRequestController
    {
        private readonly IRequestEvent _requestEvent;
        private readonly IRequestService _requestService;

        public RequestController(ILogger<RequestController> logger,
            IRequestEvent requestEvent, IRequestService requestService) : base(logger)
        {
            _requestEvent = requestEvent;
            _requestService = requestService;
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

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var identity = (ClaimsIdentity) User.Identity;

            // Since we get the sub,
            if (identity.Claims.Any(c => c.Type.Equals(JwtClaimTypes.Subject)))
            {
                return Ok(_requestEvent.GetAll(identity.Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value));
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody]CreateRequestViewModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (!string.IsNullOrWhiteSpace(sub))
            {
                _requestService.Create(vm, sub);

                return Ok();
            }

            return BadRequest("Please login again. Your session may have expired!");
        }
    }
}
