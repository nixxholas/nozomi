using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Attributes;

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
        [Throttle(Name = "WebsocketCommmandType/All", Milliseconds = 1000)]
        public IActionResult All()
        {
            return Ok(NozomiServiceConstants.websocketCommandType);
        }
    }
}