using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairRequest;
using Nozomi.Data.WebModels;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Areas.v1.CurrencyPairRequest
{
    public class CurrencyPairRequestController : BaseController<CurrencyPairRequestController>, ICurrencyPairRequestController
    {
        private readonly ICurrencyPairRequestService _currencyPairRequestService;
        
        public CurrencyPairRequestController(ILogger<CurrencyPairRequestController> logger,
            ICurrencyPairRequestService currencyPairRequestService) : base(logger)
        {
            _currencyPairRequestService = currencyPairRequestService;
        }

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

        public NozomiResult<JsonResult> Create(CreateCurrencyPairRequest obj, long userId = 0)
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

        public NozomiResult<JsonResult> Update(UpdateCurrencyPairRequest obj, long userId = 0)
        {
            return new NozomiResult<JsonResult>()
            {
                ResultType = _currencyPairRequestService.Update(obj, userId)
                    ? NozomiResultType.Success
                    : NozomiResultType.Failed,
                Data = new JsonResult(string.Empty)
            };
        }

        public NozomiResult<JsonResult> Delete(long id, bool hardDelete = false, long userId = 0)
        {
            throw new System.NotImplementedException();
        }

        public NozomiResult<JsonResult> ManualPoll(long requestId, long userId = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}