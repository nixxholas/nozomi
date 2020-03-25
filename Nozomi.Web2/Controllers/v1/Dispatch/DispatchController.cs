using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.Dispatch;
using Nozomi.Preprocessing.Attributes;
using Nozomi.Preprocessing.Statics;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Web2.Controllers.v1.Dispatch
{
    public class DispatchController : BaseApiController<DispatchController>, IDispatchController
    {
        private readonly IDispatchEvent _dispatchEvent;
        
        public DispatchController(ILogger<DispatchController> logger, IDispatchEvent dispatchEvent) : base(logger)
        {
            _dispatchEvent = dispatchEvent;
        }

        [Authorize(Roles = NozomiPermissions.AllowAllStaffRoles)]
        [HttpPost]
        [Throttle(Name = "Dispatch/Index", Milliseconds = 5000)]
        public async Task<IActionResult> Fetch(DispatchInputModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (!string.IsNullOrWhiteSpace(sub))
                return Ok(await _dispatchEvent.Dispatch(vm));

            return BadRequest("Please re-authenticate again");
        }
    }
}