using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairRequest;
using Nozomi.Data.WebModels;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Areas.v1.CurrencyPairRequest
{
    [ApiController]
    public class CurrencyPairRequestController : BaseController<CurrencyPairRequestController>, ICurrencyPairRequestController
    {
        private readonly ICurrencyPairRequestService _currencyPairRequestService;
        
        public CurrencyPairRequestController(ILogger<CurrencyPairRequestController> logger,
            ICurrencyPairRequestService currencyPairRequestService) : base(logger)
        {
            _currencyPairRequestService = currencyPairRequestService;
        }

        [HttpGet]
        public NozomiResult<JsonResult> All(bool includeNested)
        {
            var res = _currencyPairRequestService.GetAllActive();
            
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
            var res = _currencyPairRequestService.Create(new Data.WebModels.CurrencyPairRequest()
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
                ResultType = _currencyPairRequestService.Update(obj, userId)
                    ? NozomiResultType.Success
                    : NozomiResultType.Failed,
                Data = new JsonResult(string.Empty)
            };
        }

        [Authorize]
        [HttpDelete("{id}")]
        public NozomiResult<JsonResult> Delete(long id, bool hardDelete = false, long userId = 0)
        {
            return new NozomiResult<JsonResult>()
            {
                ResultType = _currencyPairRequestService.Delete(id, hardDelete, userId)
                    ? NozomiResultType.Success
                    : NozomiResultType.Failed,
                Data = new JsonResult(string.Empty)
            };
        }                                                 

        [HttpPost("{requestId}")]
        public NozomiResult<JsonResult> ManualPoll(long requestId, long userId = 0)
        {
            return new NozomiResult<JsonResult>()
            {
                ResultType = _currencyPairRequestService.ManualPoll(requestId, userId) 
                    ? NozomiResultType.Success 
                    : NozomiResultType.Failed,
                Data = new JsonResult(string.Empty)
            };
        }
    }
}