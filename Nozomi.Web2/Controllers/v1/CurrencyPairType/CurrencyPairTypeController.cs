using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
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
        public NozomiResult<JsonResult> All()
        {
            return new NozomiResult<JsonResult>(new JsonResult(_currencyPairTypeEvent.All()));
        }
    }
}
