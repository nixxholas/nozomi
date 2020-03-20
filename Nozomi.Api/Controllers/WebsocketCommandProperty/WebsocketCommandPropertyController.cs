using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Api.Controllers.WebsocketCommandProperty
{
    public class WebsocketCommandPropertyController : BaseApiController<WebsocketCommandPropertyController>,
        IWebsocketCommandPropertyController
    {
        private readonly INozomiRedisEvent _nozomiRedisEvent;
        private readonly IWebsocketCommandPropertyEvent _websocketCommandPropertyEvent;
        
        public WebsocketCommandPropertyController(ILogger<WebsocketCommandPropertyController> logger,
            INozomiRedisEvent nozomiRedisEvent, IWebsocketCommandPropertyEvent websocketCommandPropertyEvent) 
            : base(logger)
        {
            _nozomiRedisEvent = nozomiRedisEvent;
            _websocketCommandPropertyEvent = websocketCommandPropertyEvent;
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