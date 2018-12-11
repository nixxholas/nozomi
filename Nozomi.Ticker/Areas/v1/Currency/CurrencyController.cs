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
        public NozomiResult<string> Create(CreateCurrency createCurrency)
        {
            return _currencyService.Create(createCurrency);
        }
    }
}