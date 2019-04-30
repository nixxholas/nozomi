using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Enumerators.Interfaces;

namespace Nozomi.Ticker.Areas.v1.RequestPropertyType
{
    public class RequestPropertyTypeController : BaseController<RequestPropertyTypeController>, IRequestPropertyTypeController
    {
        private readonly IRequestPropertyTypeService _requestPropertyTypeService;
        
        public RequestPropertyTypeController(ILogger<RequestPropertyTypeController> logger, NozomiUserManager userManager,
            IRequestPropertyTypeService requestPropertyTypeService) : base(logger, userManager)
        {
            _requestPropertyTypeService = requestPropertyTypeService;
        }

        [HttpGet]
        public NozomiResult<JsonResult> All()
        {
            return new NozomiResult<JsonResult>(new JsonResult(_requestPropertyTypeService.All()));
        }
    }
}