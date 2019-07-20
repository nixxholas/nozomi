using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Data;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Enumerators.Interfaces;

namespace Nozomi.Web.Controllers.APIs.v1.RequestPropertyType
{
    public class RequestPropertyTypeApiController : BaseApiController<RequestPropertyTypeApiController>, IRequestPropertyTypeController
    {
        private readonly IRequestPropertyTypeEvent _requestPropertyTypeEvent;

        public RequestPropertyTypeApiController(ILogger<RequestPropertyTypeApiController> logger, UserManager<User> userManager,
            IRequestPropertyTypeEvent requestPropertyTypeEvent) : base(logger, userManager)
        {
            _requestPropertyTypeEvent = requestPropertyTypeEvent;
        }

        [HttpGet]
        public NozomiResult<JsonResult> All()
        {
            return new NozomiResult<JsonResult>(new JsonResult(_requestPropertyTypeEvent.All()));
        }
    }
}
