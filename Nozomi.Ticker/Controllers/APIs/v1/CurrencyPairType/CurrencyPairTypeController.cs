using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Enumerators.Interfaces;

namespace Nozomi.Ticker.Controllers.APIs.v1.CurrencyPairType
{
    [ApiController]
    public class CurrencyPairTypeController : BaseController<CurrencyPairTypeController>, ICurrencyPairTypeController
    {
        private ICurrencyPairTypeService _currencyPairTypeService;
        
        public CurrencyPairTypeController(ILogger<CurrencyPairTypeController> logger, NozomiUserManager userManager,
            ICurrencyPairTypeService currencyPairTypeService) : base(logger, userManager)
        {
            _currencyPairTypeService = currencyPairTypeService;
        }

        [HttpGet]
        public NozomiResult<JsonResult> All()
        {
            return new NozomiResult<JsonResult>(new JsonResult(_currencyPairTypeService.All()));
        }
    }
}