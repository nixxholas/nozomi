using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.WebModels;
using Nozomi.Service.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Nozomi.Ticker.Areas.v1.CurrencyPairComponent
{
    public class CurrencyPairComponentController : BaseController<CurrencyPairComponentController>, ICurrencyPairComponentController
    {
        private readonly ICurrencyPairComponentService _currencyPairComponentService;
        
        public CurrencyPairComponentController(ILogger<CurrencyPairComponentController> logger,
            ICurrencyPairComponentService currencyPairComponentService) 
            : base(logger)
        {
            _currencyPairComponentService = currencyPairComponentService;
        }

        [HttpGet]
        //[SwaggerResponse(200, "Request components obtained.", typeof(NozomiResult<ICollection<RequestComponent>>))]
        //[SwaggerResponse(400, "The request ID is invalid.")]
        [SwaggerOperation(
            Summary = "Obtains all Request Components related to the specific Request ID."
        )]
        public NozomiResult<ICollection<RequestComponent>> AllByRequestId(long requestId, bool includeNested = false)
        {
            return new NozomiResult<ICollection<RequestComponent>>
                (_currencyPairComponentService.AllByRequestId(requestId, includeNested));
        }

        [HttpGet]
        public NozomiResult<ICollection<RequestComponent>> All(bool includeNested = false)
        {
            return new NozomiResult<ICollection<RequestComponent>>
                (_currencyPairComponentService.All(includeNested));
        }

        [HttpPost]
        public NozomiResult<string> Create(CreateCurrencyPairComponent createCurrencyPairComponent)
        {
            return _currencyPairComponentService.Create(createCurrencyPairComponent);
        }

        [HttpPost]
        public NozomiResult<string> Update(UpdateCurrencyPairComponent updateCurrencyPairComponent)
        {
            return _currencyPairComponentService.Update(updateCurrencyPairComponent);
        }

        [HttpDelete]
        public NozomiResult<string> Delete(long id, long userId = 0, bool hardDelete = false)
        {
            return _currencyPairComponentService.Delete(id, userId, hardDelete);
        }
    }
}