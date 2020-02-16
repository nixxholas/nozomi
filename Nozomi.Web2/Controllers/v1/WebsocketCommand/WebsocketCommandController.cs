using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.WebsocketCommand;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Web2.Controllers.v1.WebsocketCommand
{
    public class WebsocketCommandController : BaseApiController<WebsocketCommandController>, IWebsocketCommandController
    {
        private readonly IWebsocketCommandEvent _websocketCommandEvent;
        
        public WebsocketCommandController(ILogger<WebsocketCommandController> logger,
            IWebsocketCommandEvent websocketCommandEvent) : base(logger)
        {
            _websocketCommandEvent = websocketCommandEvent;
        }

        [Authorize]
        [HttpGet("{guid}")]
        public IActionResult Get(string guid)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub))
            {
                return Ok(_websocketCommandEvent.View(guid, true, sub));
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpGet("{requestGuid}")]
        public IActionResult GetByRequest(string requestGuid)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub))
            {
                return Ok(_websocketCommandEvent.GetAllByRequest(requestGuid, true, sub));
            }

            return BadRequest("Please re-authenticate again");
        }

        public IActionResult Create(CreateWebsocketCommandInputModel vm)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult Update(UpdateWebsocketCommandInputModel vm)
        {
            throw new System.NotImplementedException();
        }
    }
}