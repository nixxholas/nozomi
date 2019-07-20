using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Web.Controllers.APIs.v1.ComponentType
{
    public class ComponentTypeApiController : BaseApiController<ComponentTypeApiController>, IComponentTypeController
    {
        private readonly IComponentTypeEvent _componentTypeEvent;

        public ComponentTypeApiController(ILogger<ComponentTypeApiController> logger, UserManager<User> userManager,
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
