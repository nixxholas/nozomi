using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Ticker.Controllers.APIs.v1.AnalysedHistoricItem
{
    public class AnalysedHistoricItemController : BaseController<AnalysedHistoricItemController>, IAnalysedHistoricItemController
    {
        private readonly IAnalysedHistoricItemEvent _analysedHistoricItemEvent;
        
        public AnalysedHistoricItemController(ILogger<AnalysedHistoricItemController> logger, 
            NozomiUserManager nozomiUserManager, IAnalysedHistoricItemEvent analysedHistoricItemEvent) 
            : base(logger, nozomiUserManager)
        {
            _analysedHistoricItemEvent = analysedHistoricItemEvent;
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