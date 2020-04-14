using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.BCL.Helpers.Enumerator;
using Nozomi.Data;
using Nozomi.Data.ViewModels.Request;
using Nozomi.Infra.Payment;
using Nozomi.Infra.Payment.Events.Bootstripe;
using Nozomi.Preprocessing.Attributes;
using Nozomi.Preprocessing.Statics;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Web2.Controllers.v1.Request
{
    [ApiController]
    public class RequestController : BaseApiController<RequestController>, IRequestController
    {
        private readonly IBootstripeEvent _bootstripeEvent;
        private readonly IRequestEvent _requestEvent;
        private readonly IRequestService _requestService;

        public RequestController(ILogger<RequestController> logger, IBootstripeEvent bootstripeEvent,
            IRequestEvent requestEvent, IRequestService requestService) : base(logger)
        {
            _bootstripeEvent = bootstripeEvent;
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
        public async Task<IActionResult> Create([FromBody]CreateRequestInputModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (!string.IsNullOrWhiteSpace(sub))
            {
                // Obtain the current user's plan
                var userPlan = await _bootstripeEvent.GetUserCurrentPlanIdAsync(sub);
                if (string.IsNullOrEmpty(userPlan)) return BadRequest("Please subscribe before attempting to " +
                                                                      "create a request!");
                
                // Get the user's quota for requests
                var usersPlanQuotaStr = await _bootstripeEvent.GetPlanMetadataValue(userPlan,
                    NozomiPaymentConstants.StripeRequestQuotaMetadataKey);
                var userRequestCount = _requestEvent.Count(sub);
                if (!long.TryParse(usersPlanQuotaStr, out var usersPlanQuota) // If the quota is parseable 
                    || usersPlanQuota <= userRequestCount) // Or if the quota is less than or equal to the count of
                    // requests
                    return BadRequest("Quota hit."); // Good try buddy ;) 

                _requestService.Create(vm, sub); // Else let him through!

                return Ok("Request successfully created!");
            }

            return BadRequest("Please login again. Your session may have expired!");
        }
        
        [Authorize]
        [HttpGet("{guid}")]
        [Throttle(Name = "Request/Create", Milliseconds = 1000)]
        public IActionResult Get(string guid)
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
                    return Ok(_requestEvent.View(guid));
                }
                
                return Ok(_requestEvent.View(guid, identity.Claims
                    .SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value));
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpGet]
        [Throttle(Name = "Request/GetAll", Milliseconds = 1000)]
        public IActionResult GetAll([FromQuery]int index = 0)
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
                
                return Ok(_requestEvent.ViewAll(index, identity.Claims
                    .SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value));
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpPut]
        [Throttle(Name = "Request/Update", Milliseconds = 2500)]
        public IActionResult Update([FromBody]UpdateRequestInputModel vm)
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
