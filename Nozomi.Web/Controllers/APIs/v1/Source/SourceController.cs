using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Source;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Controllers.APIs.v1.Source
{
    public class SourceController : BaseController<SourceController>, ISourceController
    {
        private readonly ISourceEvent _sourceEvent;

        public SourceController(ILogger<SourceController> logger, NozomiUserManager userManager,
            ISourceEvent sourceEvent)
            : base(logger, userManager)
        {
            _sourceEvent = sourceEvent;
        }

        [HttpGet]
        public NozomiResult<ICollection<CurrencyResponse>> History(long sourceId, long days = 7)
        {
            try
            {
                //TODO: Implementation again
//                var res = _historicalDataEvent.GetSimpleCurrencyHistory(sourceId, days);
//
//                if (res == null) throw new ArgumentNullException();
                return new NozomiResult<ICollection<CurrencyResponse>>(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                return new NozomiResult<ICollection<CurrencyResponse>>(NozomiResultType.Failed,
                    "Invalid source or days input.");
            }
        }

        [HttpGet]
        public NozomiResult<ICollection<Data.Models.Currency.Source>> All()
        {
            return new NozomiResult<ICollection<Data.Models.Currency.Source>>(_sourceEvent.GetAllActive(true).ToList());
        }
    }
}
