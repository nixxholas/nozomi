using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Preprocessing.Attributes;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Web2.Controllers.v1.ComponentType
{
    public class ComponentTypeController : BaseApiController<ComponentTypeController>, IComponentTypeController
    {
        private readonly IComponentTypeEvent _componentTypeEvent;

        public ComponentTypeController(ILogger<ComponentTypeController> logger,
            IComponentTypeEvent componentTypeEvent) : base(logger)
        {
            _componentTypeEvent = componentTypeEvent;
        }

        [Authorize]
        [HttpGet]
        [Throttle(Name = "ComponentType/All", Milliseconds = 1000)]
        public IActionResult All()
        {
            return Ok(_componentTypeEvent.All());
        }
    }
}
