using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Ticker.Controllers.APIs.v1.CurrencyPairType
{
    [ApiController]
    public class CurrencyPairTypeController : BaseController<CurrencyPairTypeController>, ICurrencyPairTypeController
    {
        private ICurrencyPairTypeEvent _currencyPairTypeEvent;
        
        public CurrencyPairTypeController(ILogger<CurrencyPairTypeController> logger, NozomiUserManager userManager,
            ICurrencyPairTypeEvent currencyPairTypeEvent) : base(logger, userManager)
        {
            _currencyPairTypeEvent = currencyPairTypeEvent;
        }

        [HttpGet]
        public NozomiResult<JsonResult> All()
        {
            return new NozomiResult<JsonResult>(new JsonResult(_currencyPairTypeEvent.All()));
        }
    }
}