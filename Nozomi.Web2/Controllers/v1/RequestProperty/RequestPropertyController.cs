using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL.Helpers.Enumerator;
using Nozomi.Data.ViewModels.RequestProperty;
using Nozomi.Preprocessing.Attributes;
using Nozomi.Preprocessing.Statics;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web2.Controllers.v1.RequestProperty
{
    public class RequestPropertyController : BaseApiController<RequestPropertyController>, IRequestPropertyController
    {
        private readonly IRequestPropertyEvent _requestPropertyEvent;
        private readonly IRequestPropertyService _requestPropertyService;
        
        public RequestPropertyController(ILogger<RequestPropertyController> logger,
            IRequestPropertyEvent requestPropertyEvent, IRequestPropertyService requestPropertyService) : base(logger)
        {
            _requestPropertyEvent = requestPropertyEvent;
            _requestPropertyService = requestPropertyService;
        }

        [Authorize]
        [HttpGet("{guid}")]
        [Throttle(Name = "RequestProperty/Get", Milliseconds = 1000)]
        public IActionResult Get(string guid)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;
            var roles = ((ClaimsIdentity) User.Identity)
                .Claims.Where(c => c.Type.Equals(JwtClaimTypes.Role));

            if (roles.Any(r => NozomiPermissions.AllStaffRoles
                .Any(e => e.GetDescription().Equals(r.Value))))
                return Ok(_requestPropertyEvent.GetByGuid(guid));

            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub) && !string.IsNullOrEmpty(guid))
                return Ok(_requestPropertyEvent.GetByGuid(guid, sub));

            if (!string.IsNullOrWhiteSpace(sub) && string.IsNullOrEmpty(guid))
                return BadRequest("Please enter a valid ID!");
            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpGet("{requestGuid}")]
        [Throttle(Name = "RequestProperty/GetAllByRequest", Milliseconds = 1000)]
        public IActionResult GetAllByRequest(string requestGuid)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;
            var roles = ((ClaimsIdentity) User.Identity)
                .Claims.Where(c => c.Type.Equals(JwtClaimTypes.Role));

            if (roles.Any(r => NozomiPermissions.AllStaffRoles
                .Any(e => e.GetDescription().Equals(r.Value))))
                return Ok(_requestPropertyEvent.GetByRequest(requestGuid));
            
            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub) && !string.IsNullOrEmpty(requestGuid))
                return Ok(_requestPropertyEvent.GetByRequest(requestGuid, sub));

            if (!string.IsNullOrWhiteSpace(sub) && string.IsNullOrEmpty(requestGuid))
                return BadRequest("Please enter a valid ID!");
            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpPost]
        [Throttle(Name = "RequestProperty/Create", Milliseconds = 2500)]
        public async Task<IActionResult> Create([FromBody]CreateRequestPropertyInputModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub))
            {
                await _requestPropertyService.Create(vm, sub);

                return Ok();
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpPut]
        [Throttle(Name = "RequestProperty/Update", Milliseconds = 2500)]
        public async Task<IActionResult> Update([FromBody]UpdateRequestPropertyInputModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub))
            {
                await _requestPropertyService.Update(vm, sub);

                return Ok();
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpDelete("{guid}")]
        [Throttle(Name = "RequestProperty/Delete", Milliseconds = 2000)]
        public async Task<IActionResult> Delete(string guid)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub) && !string.IsNullOrEmpty(guid))
            {
                await _requestPropertyService.Delete(guid, sub);

                return Ok();
            }

            if (!string.IsNullOrEmpty(sub) && string.IsNullOrEmpty(guid))
                return BadRequest("Invalid request property ID!");
            return BadRequest("Please re-authenticate again");
        }
    }
}