using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencySource;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Events.Memory.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Controllers.APIs.v1.Source
{
    public class SourceController : BaseController<SourceController>, ISourceController
    {
        private readonly ISourceEvent _sourceEvent;
        private readonly ISourceService _sourceService;
        private readonly IHistoricalDataEvent _historicalDataEvent;
        
        public SourceController(ILogger<SourceController> logger, NozomiUserManager userManager,
            ISourceEvent sourceEvent,IHistoricalDataEvent historicalDataEvent, ISourceService sourceService) 
            : base(logger, userManager)
        {
            _sourceEvent = sourceEvent;
            _historicalDataEvent = historicalDataEvent;
            _sourceService = sourceService;
        }

        [HttpGet]
        public NozomiResult<ICollection<DistinctiveCurrencyResponse>> History(long sourceId, long days = 7)
        {
            try
            {
                var res = _historicalDataEvent.GetSimpleCurrencyHistory(sourceId, days);
            
                if (res == null) throw new ArgumentNullException();
                return new NozomiResult<ICollection<DistinctiveCurrencyResponse>>(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                
                return new NozomiResult<ICollection<DistinctiveCurrencyResponse>>(NozomiResultType.Failed,
                    "Invalid source or days input.");
            }
        }

        [HttpGet]
        public NozomiResult<ICollection<Data.Models.Currency.Source>> All()
        {
            return new NozomiResult<ICollection<Data.Models.Currency.Source>>(_sourceEvent.GetAllActive(true).ToList());
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