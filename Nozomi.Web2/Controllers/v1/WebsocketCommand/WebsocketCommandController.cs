using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.WebsocketCommand;

namespace Nozomi.Web2.Controllers.v1.WebsocketCommand
{
    public class WebsocketCommandController : BaseApiController<WebsocketCommandController>, IWebsocketCommandController

    {
        public WebsocketCommandController(ILogger<WebsocketCommandController> logger) : base(logger)
        {
        }

        public IActionResult Get(string guid)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult GetByRequest(string requestGuid)
        {
            throw new System.NotImplementedException();
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