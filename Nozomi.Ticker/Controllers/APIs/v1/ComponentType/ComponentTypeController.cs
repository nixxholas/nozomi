using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Ticker.Controllers.APIs.v1.ComponentType
{
    public class ComponentTypeController : BaseController<ComponentTypeController>, IComponentTypeController
    {
        private readonly IComponentTypeEvent _componentTypeEvent;
        
        public ComponentTypeController(ILogger<ComponentTypeController> logger, NozomiUserManager userManager,
            IComponentTypeEvent componentTypeEvent) : base(logger, userManager)
        {
            _componentTypeEvent = componentTypeEvent;
        }

        [HttpGet]
        public NozomiResult<JsonResult> All()
        {
            return new NozomiResult<JsonResult>(new JsonResult(_componentTypeEvent.All()));
        }
    }
}