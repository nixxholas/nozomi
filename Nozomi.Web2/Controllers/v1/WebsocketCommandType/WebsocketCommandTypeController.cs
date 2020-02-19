using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Nozomi.Web2.Controllers.v1.WebsocketCommandType
{
    public class WebsocketCommandTypeController : BaseApiController<WebsocketCommandTypeController>,
        IWebsocketCommandTypeController
    {
        public WebsocketCommandTypeController(ILogger<WebsocketCommandTypeController> logger) : base(logger)
        {
        }

        public IActionResult All()
        {
            throw new System.NotImplementedException();
        }
    }
}