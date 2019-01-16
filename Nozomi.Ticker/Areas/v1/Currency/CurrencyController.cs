using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Areas.v1.Currency
{
    [ApiController]
    public class CurrencyController : BaseController<CurrencyController>, ICurrencyController
    {
        private readonly ICurrencyService _currencyService;
        
        public CurrencyController(ILogger<CurrencyController> logger, NozomiUserManager userManager,
            ICurrencyService currencyService) : base(logger, userManager)
        {
            _currencyService = currencyService;
        }

        [Authorize]
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

        [Authorize]
        [HttpPost]
        public NozomiResult<string> Update(UpdateCurrency updateCurrency)
        {
            return _currencyService.Update(updateCurrency);
        }
    }
}