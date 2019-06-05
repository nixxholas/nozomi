using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairRequest;
using Nozomi.Data.Models.Web;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Ticker.Controllers.APIs.v1.CurrencyPairRequest
{
    [ApiController]
    public class CurrencyPairRequestController : BaseController<CurrencyPairRequestController>, ICurrencyPairRequestController
    {
        private readonly IRequestEvent _requestEvent;
        private readonly IRequestService _requestService;
        
        public CurrencyPairRequestController(ILogger<CurrencyPairRequestController> logger, NozomiUserManager userManager,
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
        public NozomiResult<JsonResult> Create([FromBody]CreateCurrencyPairRequest obj, long userId = 0)
        {
            var res = _requestService.Create(new Request()
            {
                CurrencyPairId = obj.CurrencyPairId,
                RequestType = obj.RequestType,
                DataPath = obj.DataPath,
                Delay = obj.Delay,
                RequestComponents = obj.RequestComponents
                    .Select(rc => new RequestComponent()
                    {
                        ComponentType = rc.ComponentType,
                        QueryComponent = rc.QueryComponent
                    })
                    .ToList(),
                RequestProperties = obj.RequestProperties
                    .Select(rp => new RequestProperty()
                    {
                        RequestPropertyType = rp.RequestPropertyType,
                        Key = rp.Key,
                        Value = rp.Value
                    })
                    .ToList()
            });
            
            return new NozomiResult<JsonResult>()
            {
                ResultType = (res > 0) ? NozomiResultType.Success : NozomiResultType.Failed,
                Data = new JsonResult(string.Empty)
            };
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