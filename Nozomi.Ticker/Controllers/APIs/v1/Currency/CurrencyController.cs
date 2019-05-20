using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Controllers.APIs.v1.Currency
{
    [ApiController]
    public class CurrencyController : BaseController<CurrencyController>, ICurrencyController
    {
        private readonly ICurrencyEvent _currencyEvent;
        private readonly ICurrencyService _currencyService;
        
        public CurrencyController(ILogger<CurrencyController> logger, NozomiUserManager userManager,
            ICurrencyEvent currencyEvent, ICurrencyService currencyService) : base(logger, userManager)
        {
            _currencyEvent = currencyEvent;
            _currencyService = currencyService;
        }

        [Authorize]
        [HttpPost]
        public ActionResult<NozomiResult<string>> Create([FromBody]CreateCurrency createCurrency)
        {
            if (_currencyEvent.Any(createCurrency))
            {
                return BadRequest(new NozomiResult<string>()
                {
                    ResultType = NozomiResultType.Failed,
                    Message = "A currency with the data provided already exists."
                });
            }
            
            return Ok(_currencyService.Create(createCurrency));
        }

        [HttpGet("{abbreviation}")]
        public NozomiResult<DetailedCurrencyResponse> Detailed(string abbreviation)
        {
            return new NozomiResult<DetailedCurrencyResponse>(_currencyEvent.GetDetailedByAbbreviation(abbreviation, 
                new List<AnalysedComponentType>()
                {
                    AnalysedComponentType.CurrentAveragePrice
                }));
        }

        [Authorize]
        [HttpPost]
        public NozomiResult<string> Update(UpdateCurrency updateCurrency)
        {
            return _currencyService.Update(updateCurrency);
        }
    }
}