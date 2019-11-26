using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
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
        public NozomiResult<JsonResult> All()
        {
            return new NozomiResult<JsonResult>(new JsonResult(_componentTypeEvent.All()));
        }
    }
}
