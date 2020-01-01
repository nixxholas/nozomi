using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Web2.Controllers.v1.CurrencyPairType
{
    [ApiController]
    public class CurrencyPairTypeController : BaseApiController<CurrencyPairTypeController>, ICurrencyPairTypeController
    {
        private readonly ICurrencyPairTypeEvent _currencyPairTypeEvent;

        public CurrencyPairTypeController(ILogger<CurrencyPairTypeController> logger,
            ICurrencyPairTypeEvent currencyPairTypeEvent) : base(logger)
        {
            _currencyPairTypeEvent = currencyPairTypeEvent;
        }

        [HttpGet]
        public IActionResult All()
        {
            return Ok(_currencyPairTypeEvent.All());
        }
    }
}
