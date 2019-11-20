using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web2.Controllers.APIs.v1.Component
{
    [ApiController]
    public class ComponentController : BaseApiController<ComponentController>, IComponentController
    {
        private readonly IComponentService _componentService;

        public ComponentController(ILogger<ComponentController> logger,
            IComponentService componentService) : base(logger)
        {
            _componentService = componentService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody]CreateComponentViewModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (!string.IsNullOrWhiteSpace(sub))
            {
                _componentService.Create(vm, sub);

                return Ok();
            }

            return BadRequest("Please login again. Your session may have expired!");
        }
    }
}
