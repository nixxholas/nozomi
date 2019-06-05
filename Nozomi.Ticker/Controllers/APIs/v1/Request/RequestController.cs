using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairRequest;
using Nozomi.Data.AreaModels.v1.RequestComponent;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Web;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Ticker.Controllers.APIs.v1.Request
{
    [ApiController]
    public class RequestController : BaseController<RequestController>, IRequestController
    {
        private readonly IRequestEvent _requestEvent;
        private readonly IRequestService _requestService;
        
        public RequestController(ILogger<RequestController> logger, NozomiUserManager userManager,
            IRequestEvent requestEvent, IRequestService requestService) : base(logger, userManager)
        {
            _requestEvent = requestEvent;
            _requestService = requestService;
        }

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
        [HttpPost("{userId}")]
        public NozomiResult<string> Create([FromBody]CreateRequest obj, long userId = 0)
        {
            return _requestService.Create(obj);
        }

        [Authorize]
        [HttpPost("{userId}")]
        public NozomiResult<JsonResult> Update([FromBody]UpdateCurrencyPairRequest obj, long userId = 0)
        {
            return new NozomiResult<JsonResult>()
            {
                ResultType = _requestService.Update(obj, userId)
                    ? NozomiResultType.Success
                    : NozomiResultType.Failed,
                Data = new JsonResult(string.Empty)
            };
        }

        [Authorize]
        [HttpDelete("{id}")]
        public NozomiResult<string> Delete(long id, bool hardDelete = false, long userId = 0)
        {
            return _requestService.Delete(id, hardDelete, userId);
        }                                                 

        [HttpPost("{requestId}")]
        public NozomiResult<JsonResult> ManualPoll(long requestId, long userId = 0)
        {
            return new NozomiResult<JsonResult>()
            {
                ResultType = _requestService.ManualPoll(requestId, userId) 
                    ? NozomiResultType.Success 
                    : NozomiResultType.Failed,
                Data = new JsonResult(string.Empty)
            };
        }
    }
}