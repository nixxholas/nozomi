using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL.Helpers.Enumerator;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Preprocessing.Attributes;
using Nozomi.Preprocessing.Statics;
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

        [Authorize]
        [HttpGet]
        [Throttle(Milliseconds = 1000)]
        public IActionResult AllByRequest([FromQuery]string requestGuid, [FromQuery]int index = 0, 
            [FromQuery]int itemsPerPage = 50, [FromQuery]bool includeNested = false)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (!string.IsNullOrWhiteSpace(sub))
            {
                if (!Guid.TryParse(requestGuid, out var guid))
                    return BadRequest("Invalid request GUID, please make sure you are handing over a valid guid.");
                
                if (index < 0 || itemsPerPage <= 0)
                    return BadRequest("Invalid index or itemsPerIndex");
                
                // Obtain the user's roles
                var role = ((ClaimsIdentity) User.Identity).Claims
                    .Where(c => c.Type.Equals(JwtClaimTypes.Role));
                
                // If the user is a staff, let him access the entity indefinitely
                if (role.Any(r => NozomiPermissions.AllStaffRoles
                    .Any(sr => sr.GetDescription().Equals(r.Value))))
                {
                    return Ok(_componentEvent.GetAllByRequest(requestGuid, includeNested, index, itemsPerPage));
                }
            
                return Ok(_componentEvent.GetAllByRequest(requestGuid, includeNested, index, itemsPerPage, sub));
            }

            return BadRequest("Please login again. Your session may have expired!");
        }
        
        [HttpGet]
        [Throttle(Milliseconds = 1000)]
        public IActionResult All([FromQuery]int index = 0, [FromQuery]int itemsPerPage = 50, 
            [FromQuery]bool includeNested = false)
        {
            if (index < 0 || itemsPerPage <= 0)
                return BadRequest("Invalid index or itemsPerIndex");
            
            return Ok(_componentEvent.All(index, itemsPerPage, includeNested));
        }

        [Authorize(Roles = NozomiPermissions.AllowAllStaffRoles)]
        [HttpPost]
        [Throttle(Milliseconds = 2500)]
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
