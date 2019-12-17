using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.AnalysedComponent;
using Nozomi.Data.ViewModels.CurrencyType;
using Nozomi.Preprocessing;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Web2.Controllers.v1.CurrencyType
{
    public class CurrencyTypeController : BaseApiController<CurrencyTypeController>, ICurrencyTypeController
    {
        private readonly IAnalysedComponentEvent _analysedComponentEvent;
        private readonly ICurrencyTypeEvent _currencyTypeEvent;

        public CurrencyTypeController(ILogger<CurrencyTypeController> logger,
            IAnalysedComponentEvent analysedComponentEvent, ICurrencyTypeEvent currencyTypeEvent)
            : base(logger)
        {
            _analysedComponentEvent = analysedComponentEvent;
            _currencyTypeEvent = currencyTypeEvent;
        }

        [HttpGet]
        public IActionResult All()
        {
            return Ok(_currencyTypeEvent.All());
        }

        [HttpGet("{page}")]
        [Obsolete]
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

        [HttpGet]
        public IActionResult ListAll()
        {
            return Ok(_currencyTypeEvent.ListAll());
        }
    }
}
