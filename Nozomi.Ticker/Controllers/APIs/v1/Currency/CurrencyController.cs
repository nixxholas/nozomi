using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Service.Events.Interfaces;
using ICurrencyService = Nozomi.Infra.Admin.Service.Services.Interfaces.ICurrencyService;

namespace Nozomi.Ticker.Controllers.APIs.v1.Currency
{
    [ApiController]
    public class CurrencyController : BaseController<CurrencyController>, ICurrencyController
    {
        private readonly ICurrencyEvent _currencyEvent;
        private readonly ICurrencyService _currencyService;
        
        public CurrencyController(ILogger<CurrencyController> logger, UserManager<User> userManager,
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

        [HttpGet("{slug}")]
        public NozomiResult<GeneralisedCurrencyResponse> Detailed(string slug)
        {
            return new NozomiResult<GeneralisedCurrencyResponse>(_currencyEvent.GetDetailedBySlug(slug, null,
                new List<AnalysedComponentType>()
                {
                    AnalysedComponentType.CurrentAveragePrice
                }));
        }

        [HttpGet("{index}")]
        public ICollection<GeneralisedCurrencyResponse> GetAllDetailed([FromQuery]string currencyType = "CRYPTO", int index = 0)
        {
            return _currencyEvent.GetAllDetailed(currencyType, index);
        }

        [Authorize]
        [HttpPost]
        public NozomiResult<string> Update(UpdateCurrency updateCurrency)
        {
            return _currencyService.Update(updateCurrency);
        }
    }
}