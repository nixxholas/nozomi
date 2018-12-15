using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Areas.v1.Currency
{
    public class CurrencyController : BaseController<CurrencyController>, ICurrencyController
    {
        private ICurrencyService _currencyService;
        
        public CurrencyController(ILogger<CurrencyController> logger,
            ICurrencyService currencyService) : base(logger)
        {
            _currencyService = currencyService;
        }

        [HttpPost]
        public ActionResult<NozomiResult<string>> Create([FromBody]CreateCurrency createCurrency)
        {
            if (_currencyService.Any(createCurrency))
            {
                return BadRequest(new NozomiResult<string>()
                {
                    ResultType = NozomiResultType.Failed,
                    Message = "A currency with the data provided already exists."
                });
            }
            
            return Ok(_currencyService.Create(createCurrency));
        }

        [HttpPost]
        public NozomiResult<string> Update(UpdateCurrency updateCurrency)
        {
            return _currencyService.Update(updateCurrency);
        }
    }
}