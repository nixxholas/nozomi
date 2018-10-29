using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencySource;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Areas.v1.CurrencySource
{
    public class CurrencySourceController : BaseController<CurrencySourceController>, ICurrencySourceController
    {
        private readonly ISourceService _sourceService;
        private ICurrencySourceController _currencySourceControllerImplementation;

        public CurrencySourceController(ILogger<CurrencySourceController> logger,
            ISourceService sourceService) : base(logger)
        {
            _sourceService = sourceService;
        }

        [HttpGet]
        public NozomiResult<ICollection<JsonResult>> All()
        {
            return null;
        }

        [HttpPost]
        public NozomiResult<JsonResult> Create([FromBody]CreateSource source)
        {
            if (source.IsValid())
            {
                return new NozomiResult<JsonResult>()
                {
                    ResultType = NozomiResultType.Success,
                    Data = new JsonResult(_sourceService.Create(source) ? 
                        "Source successfully created." : "Source creation failed")
                };
            }
            
            return new NozomiResult<JsonResult>()
            {
                ResultType = NozomiResultType.Failed
            };
        }

        public NozomiResult<JsonResult> Update(UpdateSource source)
        {
            return new NozomiResult<JsonResult>()
            {
                ResultType = _sourceService.Update(source) ? NozomiResultType.Success : NozomiResultType.Failed,
                Data = new JsonResult("")
            };
        }
    }
}