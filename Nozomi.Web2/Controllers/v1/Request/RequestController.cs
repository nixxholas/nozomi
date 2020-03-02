using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.BCL.Helpers.Enumerator;
using Nozomi.Data;
using Nozomi.Data.ViewModels.Request;
using Nozomi.Preprocessing.Attributes;
using Nozomi.Preprocessing.Statics;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Web2.Controllers.v1.Request
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

        [Authorize]
        [HttpGet]
        [Throttle(Name = "Request/All", Milliseconds = 1000)]
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
        [HttpPost]
        [Throttle(Name = "Request/Create", Milliseconds = 2500)]
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

        [Authorize]
        [HttpGet]
        [Throttle(Name = "Request/GetAll", Milliseconds = 1000)]
        public IActionResult GetAll()
        {
            var identity = (ClaimsIdentity) User.Identity;

            // Since we get the sub,
            if (identity.Claims.Any(c => c.Type.Equals(JwtClaimTypes.Subject)))
            {
                var roles = identity.Claims.Where(c => c.Type.Equals(JwtClaimTypes.Role));
                
                if (roles.Any(r => NozomiPermissions.AllStaffRoles // If any roles matches a staff role
                    .Any(e => e.GetDescription().Equals(r.Value))))
                {
                    // Return null created by entities as well
                    return Ok(_requestEvent.ViewAll());
                }
                
                return Ok(_requestEvent.ViewAll(identity.Claims
                    .SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value));
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpPut]
        [Throttle(Name = "Request/Update", Milliseconds = 2500)]
        public IActionResult Update([FromBody]UpdateRequestViewModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub))
            {
                return Ok(_requestService.Update(vm, sub));
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpDelete("{guid}")]
        [Throttle(Name = "Request/Delete", Milliseconds = 2000)]
        public IActionResult Delete(string guid)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub))
            {
                _requestService.Delete(guid, true, sub);
                return Ok("Request successfully deleted!");
            }

            return BadRequest("Please re-authenticate again");
        }
    }
}
