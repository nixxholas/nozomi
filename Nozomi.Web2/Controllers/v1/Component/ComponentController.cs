using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web2.Controllers.v1.Component
{
    [ApiController]
    public class ComponentController : BaseApiController<ComponentController>, IComponentController
    {
        private readonly IComponentEvent _componentEvent;
        private readonly IComponentService _componentService;

        public ComponentController(ILogger<ComponentController> logger, IComponentEvent componentEvent,
            IComponentService componentService) : base(logger)
        {
            _componentEvent = componentEvent;
            _componentService = componentService;
        }

        [HttpGet]
        public IActionResult All([FromQuery]int index = 0, [FromQuery]int itemsPerPage = 50, [FromQuery]bool includeNested = false)
        {
            if (index < 0 || itemsPerPage <= 0)
                return BadRequest("Invalid index or itemsPerIndex");
            
            return Ok(_componentEvent.All(index, itemsPerPage, includeNested));
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
