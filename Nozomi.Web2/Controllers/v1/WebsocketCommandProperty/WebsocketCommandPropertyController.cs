using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL.Helpers.Enumerator;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;
using Nozomi.Preprocessing.Attributes;
using Nozomi.Preprocessing.Statics;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web2.Controllers.v1.WebsocketCommandProperty
{
    public class WebsocketCommandPropertyController : BaseApiController<WebsocketCommandPropertyController>,
    IWebsocketCommandPropertyController
    {
        private readonly IWebsocketCommandPropertyEvent _websocketCommandPropertyEvent;
        private readonly IWebsocketCommandPropertyService _websocketCommandPropertyService;
        
        public WebsocketCommandPropertyController(ILogger<WebsocketCommandPropertyController> logger,
            IWebsocketCommandPropertyEvent websocketCommandPropertyEvent,
            IWebsocketCommandPropertyService websocketCommandPropertyService) : base(logger)
        {
            _websocketCommandPropertyEvent = websocketCommandPropertyEvent;
            _websocketCommandPropertyService = websocketCommandPropertyService;
        }
        
        
        [Authorize]
        [HttpGet("{guid}")]
        [Throttle(Name = "WebsocketCommmandProperty/Get", Milliseconds = 1000)]
        public IActionResult Get(string guid)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub))
            {
                var roles = ((ClaimsIdentity) User.Identity)
                    .Claims.Where(c => c.Type.Equals(JwtClaimTypes.Role));
                
                if (roles.Any(r => NozomiPermissions.AllStaffRoles
                    .Any(s => s.GetDescription().Equals(r.Value))))
                    return Ok(_websocketCommandPropertyEvent.View(guid, false));
                    
                return Ok(_websocketCommandPropertyEvent.View(guid, true, sub));
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpGet("{commandGuid}")]
        [Throttle(Name = "WebsocketCommmandProperty/GetByCommand", Milliseconds = 1000)]
        public IActionResult GetByCommand(string commandGuid)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub))
            {
                var roles = ((ClaimsIdentity) User.Identity)
                    .Claims.Where(c => c.Type.Equals(JwtClaimTypes.Role));
                
                if (roles.Any(r => NozomiPermissions.AllStaffRoles
                    .Any(s => s.GetDescription().Equals(r.Value))))
                    return Ok(_websocketCommandPropertyEvent.GetAllByCommand(commandGuid, false));
                
                return Ok(_websocketCommandPropertyEvent.GetAllByCommand(commandGuid, false));
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpPost]
        [Throttle(Name = "WebsocketCommmandProperty/Create", Milliseconds = 2500)]
        public IActionResult Create(CreateWebsocketCommandPropertyInputModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub))
            {
                var roles = ((ClaimsIdentity) User.Identity)
                    .Claims.Where(c => c.Type.Equals(JwtClaimTypes.Role));
                
                if (roles.Any(r => NozomiPermissions.AllStaffRoles
                    .Any(s => s.GetDescription().Equals(r.Value))))
                    _websocketCommandPropertyService.Create(vm);
                else
                    _websocketCommandPropertyService.Create(vm, sub);
                
                return Ok("Websocket Command property successfully created!");
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpPut]
        [Throttle(Name = "WebsocketCommmandProperty/Update", Milliseconds = 2500)]
        public IActionResult Update(UpdateWebsocketCommandPropertyInputModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub))
            {
                var roles = ((ClaimsIdentity) User.Identity)
                    .Claims.Where(c => c.Type.Equals(JwtClaimTypes.Role));
                
                if (roles.Any(r => NozomiPermissions.AllStaffRoles
                    .Any(s => s.GetDescription().Equals(r.Value))))
                    _websocketCommandPropertyService.Update(vm);
                else
                    _websocketCommandPropertyService.Update(vm, sub);
                
                return Ok("Websocket Command property successfully updated!");
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpDelete("{websocketCommandPropertyGuid}")]
        [Throttle(Name = "WebsocketCommmandProperty/Delete", Milliseconds = 2000)]
        public IActionResult Delete(string websocketCommandPropertyGuid)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub))
            {
                var roles = ((ClaimsIdentity) User.Identity)
                    .Claims.Where(c => c.Type.Equals(JwtClaimTypes.Role));
                
                if (roles.Any(r => NozomiPermissions.AllStaffRoles
                    .Any(s => s.GetDescription().Equals(r.Value))))
                    _websocketCommandPropertyService.Delete(websocketCommandPropertyGuid);
                else
                    _websocketCommandPropertyService.Delete(websocketCommandPropertyGuid, sub);
                
                return Ok("Websocket Command property successfully deleted!");
            }

            return BadRequest("Please re-authenticate again");
        }
    }
}