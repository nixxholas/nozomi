using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.WebModels;
using Nozomi.Service.Services.Interfaces;

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

        public NozomiResult<ICollection<RequestComponent>> AllByRequestId(long requestId, bool includeNested = false)
        {
            return new NozomiResult<ICollection<RequestComponent>>
                (_currencyPairComponentService.AllByRequestId(requestId, includeNested));
        }

        public NozomiResult<ICollection<RequestComponent>> All(bool includeNested = false)
        {
            return new NozomiResult<ICollection<RequestComponent>>
                (_currencyPairComponentService.All(includeNested));
        }

        public NozomiResult<string> Create(CreateCurrencyPairComponent createCurrencyPairComponent)
        {
            return _currencyPairComponentService.Create(createCurrencyPairComponent);
        }

        public NozomiResult<string> Update(UpdateCurrencyPairComponent updateCurrencyPairComponent)
        {
            return _currencyPairComponentService.Update(updateCurrencyPairComponent);
        }

        public NozomiResult<string> Delete(long id, long userId = 0, bool hardDelete = false)
        {
            return _currencyPairComponentService.Delete(id, userId, hardDelete);
        }
    }
}