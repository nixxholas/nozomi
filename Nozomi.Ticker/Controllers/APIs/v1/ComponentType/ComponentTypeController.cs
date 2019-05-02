using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Enumerators.Interfaces;

namespace Nozomi.Ticker.Controllers.APIs.v1.ComponentType
{
    public class ComponentTypeController : BaseController<ComponentTypeController>, IComponentTypeController
    {
        private readonly IComponentTypeService _componentTypeService;
        
        public ComponentTypeController(ILogger<ComponentTypeController> logger, NozomiUserManager userManager,
            IComponentTypeService componentTypeService) : base(logger, userManager)
        {
            _componentTypeService = componentTypeService;
        }

        [HttpGet]
        public NozomiResult<JsonResult> All()
        {
            return new NozomiResult<JsonResult>(new JsonResult(_componentTypeService.All()));
        }
    }
}