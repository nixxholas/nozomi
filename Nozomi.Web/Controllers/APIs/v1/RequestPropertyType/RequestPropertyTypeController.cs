using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Web.Controllers.APIs.v1.RequestPropertyType
{
    public class RequestPropertyTypeController : BaseApiController<RequestPropertyTypeController>, IRequestPropertyTypeController
    {
        private readonly IRequestPropertyTypeEvent _requestPropertyTypeEvent;

        public RequestPropertyTypeController(ILogger<RequestPropertyTypeController> logger, UserManager<User> userManager,
            IRequestPropertyTypeEvent requestPropertyTypeEvent) : base(logger, userManager)
        {
            _requestPropertyTypeEvent = requestPropertyTypeEvent;
        }

        [Authorize(Roles = "Staff")]
        [HttpGet]
        public NozomiResult<JsonResult> All()
        {
            return new NozomiResult<JsonResult>(new JsonResult(_requestPropertyTypeEvent.All()));
        }
    }
}
