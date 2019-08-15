using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;
using Nozomi.Base.Core.Responses;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Data;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Preprocessing;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Web.Controllers.APIs.v1.Currency
{
    [ApiController]
    public class CurrencyController : BaseApiController<CurrencyController>, ICurrencyController
    {
        private readonly IAnalysedHistoricItemEvent _analysedHistoricItemEvent;
        private readonly ICurrencyEvent _currencyEvent;

        public CurrencyController(ILogger<CurrencyController> logger, UserManager<User> userManager,
            IAnalysedHistoricItemEvent analysedHistoricItemEvent, ICurrencyEvent currencyEvent)
            : base(logger, userManager)
        {
            _analysedHistoricItemEvent = analysedHistoricItemEvent;
            _currencyEvent = currencyEvent;
        }

        [HttpGet("{slug}")]
        public NozomiResult<DetailedCurrencyResponse> Detailed(string slug)
        {
            return new NozomiResult<DetailedCurrencyResponse>(_currencyEvent.GetDetailedBySlug(slug,
                new List<Data.Models.Currency.ComponentType>()
                {
                    Data.Models.Currency.ComponentType.CirculatingSupply
                },
                new List<AnalysedComponentType>()
                {
                    AnalysedComponentType.MarketCap,
                    AnalysedComponentType.CurrentAveragePrice,
                    AnalysedComponentType.HourlyAveragePrice
                }));
        }

        [HttpGet("{index}")]
        public ICollection<GeneralisedCurrencyResponse> GetAllDetailed([FromQuery]string currencyType = "CRYPTO", int index = 0)
        {
            return _currencyEvent.GetAllDetailed(currencyType, index);
        }

        /// <summary>
        /// Obtain the historical data for the currency.
        ///
        /// We'll integrate time scale as soon as possible.
        /// </summary>
        [HttpGet("{slug}/{index}/{perPage}")]
        public NozomiPaginatedResult<EpochValuePair<decimal>> Historical(string slug, int index = 0, int perPage = 0)
        {
            var res = _analysedHistoricItemEvent.GetCurrencyPriceHistory(slug, index, perPage);

            if (!res.Data.Any()) return new NozomiPaginatedResult<EpochValuePair<decimal>>();

            return new NozomiPaginatedResult<EpochValuePair<decimal>>
            {
                Pages = res.Pages,
                ElementsPerPage = res.ElementsPerPage,
                Data = res.Data
                   .Select(ahi => new EpochValuePair<decimal>
                   {
                       Time = (ahi.HistoricDateTime.ToUniversalTime() - CoreConstants.Epoch).TotalSeconds,
                       Value = decimal.Parse(ahi.Value)
                   }).ToList()
            };
        }
    }
}
