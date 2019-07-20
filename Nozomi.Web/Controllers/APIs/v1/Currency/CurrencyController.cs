using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Web.Controllers.APIs.v1.Currency
{
    [ApiController]
    public class CurrencyApiController : BaseApiController<CurrencyApiController>, ICurrencyController
    {
        private readonly ICurrencyEvent _currencyEvent;

        public CurrencyApiController(ILogger<CurrencyApiController> logger, NozomiUserManager userManager,
            ICurrencyEvent currencyEvent) : base(logger, userManager)
        {
            _currencyEvent = currencyEvent;
        }

        [HttpGet("{slug}")]
        public NozomiResult<DetailedCurrencyResponse> Detailed(string slug)
        {
            return new NozomiResult<DetailedCurrencyResponse>(_currencyEvent.GetDetailedBySlug(slug,
                new List<AnalysedComponentType>()
                {
                    AnalysedComponentType.CurrentAveragePrice
                }));
        }

        [HttpGet("{index}")]
        public ICollection<DetailedCurrencyResponse> GetAllDetailed([FromQuery]string currencyType = "CRYPTO", int index = 0)
        {
            return _currencyEvent.GetAllDetailed(currencyType, index);
        }
    }
}
