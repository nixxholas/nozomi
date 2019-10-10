using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Ticker.Controllers.APIs.v1.RequestPropertyType
{
    public class RequestPropertyTypeController : BaseController<RequestPropertyTypeController>, IRequestPropertyTypeController
    {
        private readonly IRequestPropertyTypeEvent _requestPropertyTypeEvent;
        
        public RequestPropertyTypeController(ILogger<RequestPropertyTypeController> logger, UserManager<User> userManager,
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