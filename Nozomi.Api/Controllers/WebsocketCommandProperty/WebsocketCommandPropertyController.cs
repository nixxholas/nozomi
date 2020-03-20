using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Api.Controllers.WebsocketCommandProperty
{
    public class WebsocketCommandPropertyController : BaseApiController<WebsocketCommandPropertyController>,
        IWebsocketCommandPropertyController
    {
        public WebsocketCommandPropertyController(ILogger<WebsocketCommandPropertyController> logger) : base(logger)
        {
        }

        public IActionResult All(int index = 0)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult AllByCommand(string commandGuid, int index = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}