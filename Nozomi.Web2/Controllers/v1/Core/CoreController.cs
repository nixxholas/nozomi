using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL;
using Nozomi.Preprocessing.Attributes;

namespace Nozomi.Web2.Controllers.v1.Core
{
    public class CoreController : BaseApiController<CoreController>, ICoreController
    {
        public CoreController(ILogger<CoreController> logger) : base(logger)
        {
        }

        [HttpGet]
        [Throttle(Milliseconds = 1000)]
        public DateTime GetCurrentBuildTime()
        {
            return CoreConstants.BuildDateTime;
        }

        [Authorize]
        [HttpGet]
        [Throttle(Milliseconds = 1000)]
        public IActionResult GetUserDetails()
        {
            // var role = ((ClaimsIdentity) User.Identity).Claims.SingleOrDefault(c => c.Type == JwtClaimTypes.Role)?.Value;
            
            return Ok(Json(User.Identity));
        }
    }
}
