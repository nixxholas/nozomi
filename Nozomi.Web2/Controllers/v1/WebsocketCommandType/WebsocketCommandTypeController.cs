using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing;

namespace Nozomi.Web2.Controllers.v1.WebsocketCommandType
{
    public class WebsocketCommandTypeController : BaseApiController<WebsocketCommandTypeController>,
        IWebsocketCommandTypeController
    {
        public WebsocketCommandTypeController(ILogger<WebsocketCommandTypeController> logger) : base(logger)
        {
        }

        [Authorize]
        [HttpGet]
        public IActionResult All()
        {
            return Ok(NozomiServiceConstants.websocketCommandType);
        }
    }
}