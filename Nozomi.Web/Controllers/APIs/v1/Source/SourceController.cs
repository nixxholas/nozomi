using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web.Controllers.APIs.v1.Source
{
    public class SourceController : BaseApiController<SourceController>, ISourceController
    {
        private readonly ISourceEvent _sourceEvent;
        private readonly ISourceService _sourceService;

        public SourceController(ILogger<SourceController> logger,
            ISourceEvent sourceEvent, ISourceService sourceService)
            : base(logger)
        {
            _sourceEvent = sourceEvent;
            _sourceService = sourceService;
        }

        #if DEBUG
        [HttpGet]
        public IActionResult Bamf()
        {
            _sourceService.Bamf();

            return Ok();
        }
        #endif

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

        [HttpGet("{slug}")]
        public NozomiResult<ICollection<Data.Models.Currency.Source>> GetCurrencySources(string slug, int page = 0)
        {
            return new NozomiResult<ICollection<Data.Models.Currency.Source>>(
                _sourceEvent.GetCurrencySources(slug, page).ToList());
        }
    }
}
