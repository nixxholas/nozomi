using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Service.Events.Analysis.Interfaces;

namespace Nozomi.Web2.Controllers.APIs.v1.AnalysedHistoricItem
{
    public class AnalysedHistoricItemController : BaseApiController<AnalysedHistoricItemController>, IAnalysedHistoricItemController
    {
        private readonly IAnalysedHistoricItemEvent _analysedHistoricItemEvent;

        public AnalysedHistoricItemController(ILogger<AnalysedHistoricItemController> logger,
            IAnalysedHistoricItemEvent analysedHistoricItemEvent)
            : base(logger)
        {
            _analysedHistoricItemEvent = analysedHistoricItemEvent;
        }

        [HttpGet("{analysedComponentId}")]
        public Task<long> Count(long analysedComponentId)
        {
            return Task.FromResult(_analysedHistoricItemEvent.Count(analysedComponentId));
        }

        [HttpGet]
        public Task<NozomiResult<ICollection<Data.Models.Web.Analytical.AnalysedHistoricItem>>> GetAll(
            long analysedComponentId, int index = 0)
        {
            return Task.FromResult(new NozomiResult<ICollection<Data.Models.Web.Analytical.AnalysedHistoricItem>>(
                _analysedHistoricItemEvent.GetAll(analysedComponentId, TimeSpan.Zero, index)));
        }
    }
}
