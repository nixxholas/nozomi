using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Web.Controllers.APIs.v1.AnalysedHistoricItem
{
    public class AnalysedHistoricItemApiController : BaseApiController<AnalysedHistoricItemApiController>, IAnalysedHistoricItemController
    {
        private readonly IAnalysedHistoricItemEvent _analysedHistoricItemEvent;

        public AnalysedHistoricItemApiController(ILogger<AnalysedHistoricItemApiController> logger,
            NozomiUserManager nozomiUserManager, IAnalysedHistoricItemEvent analysedHistoricItemEvent)
            : base(logger, nozomiUserManager)
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
