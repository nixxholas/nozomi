using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nozomi.Data;
using Nozomi.Data.RequestModels;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Controllers.v1
{
    public class CurrencySourceController : BaseController<CurrencySourceController>
    {
        private readonly ISourceService _sourceService;
        
        public CurrencySourceController(ILogger<CurrencySourceController> logger,
            ISourceService sourceService) : base(logger)
        {
            _sourceService = sourceService;
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
    }
}