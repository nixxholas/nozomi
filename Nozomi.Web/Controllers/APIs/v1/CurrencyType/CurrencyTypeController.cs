using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.AnalysedComponent;
using Nozomi.Preprocessing;
using Nozomi.Service.Events.Analysis.Interfaces;

namespace Nozomi.Web.Controllers.APIs.v1.CurrencyType
{
    public class CurrencyTypeController : BaseApiController<CurrencyTypeController>, ICurrencyTypeController
    {
        private readonly IAnalysedComponentEvent _analysedComponentEvent;

        public CurrencyTypeController(ILogger<CurrencyTypeController> logger,
            IAnalysedComponentEvent analysedComponentEvent)
            : base(logger)
        {
            _analysedComponentEvent = analysedComponentEvent;
        }

        [HttpGet("{page}")]
        public ICollection<ExtendedAnalysedComponentResponse<EpochValuePair<string>>> GetAll(int page = 0)
        {
            #if DEBUG
            var testRes = _analysedComponentEvent.GetAllCurrencyTypeAnalysedComponents(page, true, true)
                .Select(ac => new ExtendedAnalysedComponentResponse<EpochValuePair<string>>
                {
                    ParentName = ac.CurrencyType.Name,
                    ComponentType = NozomiServiceConstants.analysedComponentTypes
                        .SingleOrDefault(act => act.Value.Equals((int) ac.ComponentType)).Key,
                    Historical = ac.AnalysedHistoricItems
                        .Select(ahi => new EpochValuePair<string>
                        {
                            Time = (ahi.HistoricDateTime.ToUniversalTime() - CoreConstants.Epoch).TotalSeconds,
                            Value = ahi.Value
                        })
                        .OrderBy(dvp => dvp.Time)
                        .ToList(),
                    Value = ac.Value
                })
                .ToList();
            #endif

            return _analysedComponentEvent.GetAllCurrencyTypeAnalysedComponents(page, true, true)
                .Select(ac => new ExtendedAnalysedComponentResponse<EpochValuePair<string>>
                {
                    ParentName = ac.CurrencyType.Name,
                    ComponentType = NozomiServiceConstants.analysedComponentTypes
                        .SingleOrDefault(act => act.Value.Equals((int) ac.ComponentType)).Key,
                    Historical = ac.AnalysedHistoricItems
                        .Select(ahi => new EpochValuePair<string>
                        {
                            Time = (ahi.HistoricDateTime.ToUniversalTime() - CoreConstants.Epoch).TotalSeconds,
                            Value = ahi.Value
                        })
                        .OrderBy(dvp => dvp.Time)
                        .ToList(),
                    Value = ac.Value
                })
                .ToList();
        }
    }
}
