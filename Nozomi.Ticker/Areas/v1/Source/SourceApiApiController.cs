using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencySource;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Areas.v1.Source
{
    public class SourceApiApiController : BaseController<SourceApiApiController>, ISourceApiController
    {
        private readonly ISourceEvent _sourceEvent;
        private readonly ISourceService _sourceService;
        
        public SourceApiApiController(ILogger<SourceApiApiController> logger, NozomiUserManager userManager,
            ISourceEvent sourceEvent, ISourceService sourceService) : base(logger, userManager)
        {
            _sourceEvent = sourceEvent;
            _sourceService = sourceService;
        }

        [HttpGet]
        public NozomiResult<ICollection<Data.CurrencyModels.Source>> All()
        {
            return new NozomiResult<ICollection<Data.CurrencyModels.Source>>(_sourceEvent.GetAllActive(false).ToList());
        }

        [Authorize]
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

        [Authorize]
        [HttpPut]
        public NozomiResult<JsonResult> Update([FromBody]UpdateSource source)
        {
            return new NozomiResult<JsonResult>()
            {
                ResultType = _sourceService.Update(source) ? NozomiResultType.Success : NozomiResultType.Failed,
                Data = new JsonResult("")
            };
        }

        [Authorize]
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