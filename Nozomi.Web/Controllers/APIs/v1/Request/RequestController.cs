using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.ViewModels;

namespace Nozomi.Web.Controllers.APIs.v1.Request
{
    [ApiController]
    public class RequestController : BaseApiController<RequestController>, IRequestController
    {
        private readonly IRequestEvent _requestEvent;
        private readonly INewRequestService _newRequestService;

        public RequestController(ILogger<RequestController> logger,
            IRequestEvent requestEvent, INewRequestService newRequestService) : base(logger)
        {
            _requestEvent = requestEvent;
            _newRequestService = newRequestService;
        }

        [Authorize(Roles = "Staff")]
        [HttpGet]
        public NozomiResult<JsonResult> All(bool includeNested)
        {
            var res = _requestEvent.GetAllActive();

            return new NozomiResult<JsonResult>()
            {
                ResultType = res != null ? NozomiResultType.Success : NozomiResultType.Failed,
                Data = res != null ? new JsonResult(res) :
                    new JsonResult("We're currently unable to obtain all of the Currency Pair Requests.")
            };
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody]CreateRequestViewModel vm)
        {
            _newRequestService.Create(vm);
            return Ok();
        }
    }
}
