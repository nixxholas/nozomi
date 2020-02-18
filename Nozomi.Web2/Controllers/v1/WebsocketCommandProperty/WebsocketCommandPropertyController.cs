using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL.Helpers.Enumerator;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;
using Nozomi.Preprocessing.Statics;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Web2.Controllers.v1.WebsocketCommandProperty
{
    public class WebsocketCommandPropertyController : BaseApiController<WebsocketCommandPropertyController>,
    IWebsocketCommandPropertyController
    {
        private readonly IWebsocketCommandPropertyEvent _websocketCommandPropertyEvent;
        
        public WebsocketCommandPropertyController(ILogger<WebsocketCommandPropertyController> logger,
            IWebsocketCommandPropertyEvent websocketCommandPropertyEvent) : base(logger)
        {
            _websocketCommandPropertyEvent = websocketCommandPropertyEvent;
        }
        
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

        public IActionResult GetByCommand(string commandGuid)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult Create(CreateWebsocketCommandPropertyInputModel vm)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult Update(UpdateWebsocketCommandPropertyInputModel vm)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult Delete(string websocketCommandPropertyGuid)
        {
            throw new System.NotImplementedException();
        }
    }
}