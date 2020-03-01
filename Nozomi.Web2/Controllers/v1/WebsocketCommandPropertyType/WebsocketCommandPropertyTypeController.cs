using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Attributes;

namespace Nozomi.Web2.Controllers.v1.WebsocketCommandPropertyType
{
    public class WebsocketCommandPropertyTypeController : BaseApiController<WebsocketCommandPropertyTypeController>,
        IWebsocketCommandPropertyTypeController
    {
        public WebsocketCommandPropertyTypeController(ILogger<WebsocketCommandPropertyTypeController> logger) : base(logger)
        {
        }

        [Authorize]
        [HttpGet]
        [Throttle(Milliseconds = 1000)]
        public IActionResult All()
        {
            return Ok(NozomiServiceConstants.websocketCommandPropertyType);
        }
    }
}