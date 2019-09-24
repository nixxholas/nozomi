using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Web.Controllers.APIs.v1.CurrencyPairType
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
