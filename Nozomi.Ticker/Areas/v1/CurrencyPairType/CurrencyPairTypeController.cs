using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nozomi.Base.Core.Helpers.JSON;
using Nozomi.Data;
using Nozomi.Service.Services.Enumerators.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Areas.v1.CurrencyPairType
{
    [ApiController]
    public class CurrencyPairTypeController : BaseController<CurrencyPairTypeController>, ICurrencyPairTypeController
    {
        private ICurrencyPairTypeService _currencyPairTypeService;
        
        public CurrencyPairTypeController(ILogger<CurrencyPairTypeController> logger,
            ICurrencyPairTypeService currencyPairTypeService) : base(logger)
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