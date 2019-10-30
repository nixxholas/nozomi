using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.RequestComponent;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web.Controllers.APIs.v1.RequestComponent
{
    [ApiController]
    public class RequestComponentController : BaseApiController<RequestComponentController>, IRequestComponentController
    {
        private readonly IRequestComponentService _requestComponentService;

        public RequestComponentController(ILogger<RequestComponentController> logger,
            IRequestComponentService requestComponentService) : base(logger)
        {
            _requestComponentService = requestComponentService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody]CreateRequestComponentViewModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (!string.IsNullOrWhiteSpace(sub))
            {
                _requestComponentService.Create(vm, sub);

                return Ok();
            }

            return BadRequest("Please login again. Your session may have expired!");
        }
    }
}
