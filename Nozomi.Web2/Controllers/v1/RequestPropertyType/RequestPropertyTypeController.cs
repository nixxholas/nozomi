using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Preprocessing.Attributes;
using Nozomi.Preprocessing.Statics;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Web2.Controllers.v1.RequestPropertyType
{
    public class RequestPropertyTypeController : BaseApiController<RequestPropertyTypeController>, 
        IRequestPropertyTypeController
    {
        private readonly IRequestPropertyTypeEvent _requestPropertyTypeEvent;

        public RequestPropertyTypeController(ILogger<RequestPropertyTypeController> logger,
            IRequestPropertyTypeEvent requestPropertyTypeEvent) : base(logger)
        {
            _requestPropertyTypeEvent = requestPropertyTypeEvent;
        }

        [Authorize]
        [HttpGet]
        public IActionResult All()
        {
            return Ok(_requestPropertyTypeEvent.All());
        }
    }
}