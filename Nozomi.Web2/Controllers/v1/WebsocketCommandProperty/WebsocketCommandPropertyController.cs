using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;

namespace Nozomi.Web2.Controllers.v1.WebsocketCommandProperty
{
    public class WebsocketCommandPropertyController : BaseApiController<WebsocketCommandPropertyController>,
    IWebsocketCommandPropertyController
    {
        public WebsocketCommandPropertyController(ILogger<WebsocketCommandPropertyController> logger) : base(logger)
        {
        }
        
        public IActionResult Get(string guid)
        {
            throw new System.NotImplementedException();
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