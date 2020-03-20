using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Api.Controllers.WebsocketCommand
{
    public class WebsocketCommandController : BaseApiController<WebsocketCommandController>, 
        IWebsocketCommandController
    {
        public WebsocketCommandController(ILogger<WebsocketCommandController> logger) : base(logger)
        {
        }
    }
}