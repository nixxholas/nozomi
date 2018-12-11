using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencySource;
using Nozomi.Data.CurrencyModels;
using Nozomi.Service.Services.Interfaces;
using Swashbuckle.AspNetCore.Examples;

namespace Nozomi.Ticker.Areas.v1.CurrencySource
{
    public class CurrencySourceController : BaseController<CurrencySourceController>, ICurrencySourceController
    {
        private readonly ISourceService _sourceService;
        
        public CurrencySourceController(ILogger<CurrencySourceController> logger,
            ISourceService sourceService) : base(logger)
        {
            _sourceService = sourceService;
        }

        [HttpGet]
        public NozomiResult<ICollection<Source>> All()
        {
            return new NozomiResult<ICollection<Source>>(_sourceService.GetAllActive(false).ToList());
        }

        [HttpPost]
        public NozomiResult<JsonResult> Create([FromBody]CreateSource source)
        {
            if (source.IsValid())
            {
                var svcRes = _sourceService.Create(source);
                
                return new NozomiResult<JsonResult>()
                {
                    ResultType = svcRes.ResultType,
                    Message = svcRes.Message
                };
            }
            
            return new NozomiResult<JsonResult>()
            {
                ResultType = NozomiResultType.Failed,
                Message = "Invalid payload."
            };
        }

        [HttpPut]
        public NozomiResult<JsonResult> Update([FromBody]UpdateSource source)
        {
            return new NozomiResult<JsonResult>()
            {
                ResultType = _sourceService.Update(source) ? NozomiResultType.Success : NozomiResultType.Failed,
                Data = new JsonResult("")
            };
        }

        [HttpDelete]
        public NozomiResult<JsonResult> Delete([FromBody]DeleteSource source)
        {
            return new NozomiResult<JsonResult>()
            {
                ResultType = _sourceService.Delete(source.Id, source.HardDelete) 
                    ? NozomiResultType.Success : NozomiResultType.Failed,
                Data = new JsonResult("")
            };
        }
    }
}