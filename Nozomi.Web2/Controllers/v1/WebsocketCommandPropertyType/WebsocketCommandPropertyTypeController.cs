using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Nozomi.Web2.Controllers.v1.WebsocketCommandPropertyType
{
    public class WebsocketCommandPropertyTypeController : BaseApiController<WebsocketCommandPropertyTypeController>,
        IWebsocketCommandPropertyTypeController
    {
        public WebsocketCommandPropertyTypeController(ILogger<WebsocketCommandPropertyTypeController> logger) : base(logger)
        {
        }

        public IActionResult All()
        {
            throw new System.NotImplementedException();
        }
    }
}