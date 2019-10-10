using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Web.Controllers.APIs.v1.ComponentType
{
    public class ComponentTypeController : BaseApiController<ComponentTypeController>, IComponentTypeController
    {
        private readonly IComponentTypeEvent _componentTypeEvent;

        public ComponentTypeController(ILogger<ComponentTypeController> logger,
            IComponentTypeEvent componentTypeEvent) : base(logger)
        {
            _componentTypeEvent = componentTypeEvent;
        }

        [Authorize(Roles = "Staff")]
        [HttpGet]
        public NozomiResult<JsonResult> All()
        {
            return new NozomiResult<JsonResult>(new JsonResult(_componentTypeEvent.All()));
        }
    }
}
