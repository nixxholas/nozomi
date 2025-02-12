using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL.Helpers.Enumerator;
using Nozomi.Data.ViewModels.WebsocketCommand;
using Nozomi.Preprocessing.Attributes;
using Nozomi.Preprocessing.Statics;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web2.Controllers.v1.WebsocketCommand
{
    public class WebsocketCommandController : BaseApiController<WebsocketCommandController>, IWebsocketCommandController
    {
        private readonly IWebsocketCommandEvent _websocketCommandEvent;
        private readonly IWebsocketCommandService _websocketCommandService;
        
        public WebsocketCommandController(ILogger<WebsocketCommandController> logger,
            IWebsocketCommandEvent websocketCommandEvent, IWebsocketCommandService websocketCommandService) : base(logger)
        {
            _websocketCommandEvent = websocketCommandEvent;
            _websocketCommandService = websocketCommandService;
        }

        [Authorize]
        [HttpGet("{guid}")]
        [Throttle(Name = "WebsocketCommand/Get", Milliseconds = 1000)]
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
                    return Ok(_websocketCommandEvent.View(guid, true));
                    
                return Ok(_websocketCommandEvent.View(guid, true, sub));
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpGet("{requestGuid}")]
        [Throttle(Name = "WebsocketCommand/ViewByRequest", Milliseconds = 1000)]
        public IActionResult ViewByRequest(string requestGuid)
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
                    return Ok(_websocketCommandEvent.ViewAllByRequest(requestGuid, true));
                
                return Ok(_websocketCommandEvent.ViewAllByRequest(requestGuid, true, sub));
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpPost]
        [Throttle(Name = "WebsocketCommand/Create", Milliseconds = 2500)]
        public IActionResult Create([FromBody]CreateWebsocketCommandInputModel vm)
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
                    _websocketCommandService.Create(vm);
                else
                    _websocketCommandService.Create(vm, sub);
                
                return Ok("Websocket Command successfully created!");
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpPut]
        [Throttle(Name = "WebsocketCommand/Update", Milliseconds = 2500)]
        public IActionResult Update([FromBody]UpdateWebsocketCommandInputModel vm)
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
                    _websocketCommandService.Update(vm, sub);
                else
                    _websocketCommandService.Update(vm, sub);
                
                return Ok("Websocket Command successfully updated!");
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpDelete("{websocketCommandGuid}")]
        [Throttle(Name = "WebsocketCommand/Delete", Milliseconds = 2000)]
        public IActionResult Delete(string websocketCommandGuid)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub))
            {
                _websocketCommandService.Delete(websocketCommandGuid, sub);
                
                return Ok("Websocket Command successfully deleted!");
            }

            return BadRequest("Please re-authenticate again");
        }
    }
}