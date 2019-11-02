using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;
using Nozomi.Base.Core.Responses;
using Nozomi.Data;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Preprocessing;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Web.Controllers.APIs.v1.Currency
{
    [ApiController]
    public class CurrencyController : BaseApiController<CurrencyController>, ICurrencyController
    {
        private readonly IAnalysedHistoricItemEvent _analysedHistoricItemEvent;
        private readonly ICurrencyEvent _currencyEvent;

        public CurrencyController(ILogger<CurrencyController> logger,
            IAnalysedHistoricItemEvent analysedHistoricItemEvent, ICurrencyEvent currencyEvent)
            : base(logger)
        {
            _analysedHistoricItemEvent = analysedHistoricItemEvent;
            _currencyEvent = currencyEvent;
        }

        [HttpGet]
        public IActionResult All([FromQuery]string currencyType = "CRYPTO", [FromQuery]int itemsPerIndex = 20,
            [FromQuery]int index = 0, [FromQuery]AnalysedComponentType sortType = AnalysedComponentType.MarketCap,
            [FromQuery]bool orderDescending = true, [FromQuery]ICollection<AnalysedComponentType> typesToTake = null,
            [FromQuery]ICollection<AnalysedComponentType> typesToDeepen = null)
        {
            return Ok(_currencyEvent.All(currencyType, itemsPerIndex, index, sortType, orderDescending, typesToTake,
                typesToDeepen));
        }

        [HttpGet]
        public IActionResult GetCountByType(string currencyType = "CRYPTO")
        {
            var count = _currencyEvent.GetCountByType(currencyType);

            if (count > 0)
                return Ok(count);

            throw new ArgumentNullException("Invalid or empty currency type.");
        }

        [HttpGet]
        public NozomiResult<ICollection<string>> ListAll()
        {
            return new NozomiResult<ICollection<string>>(_currencyEvent.ListAll());
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
        public ICollection<GeneralisedCurrencyResponse> GetAllDetailed([FromQuery]string currencyType = "CRYPTO",
            int index = 0, int countPerIndex = 20)
        {
            return _currencyEvent.GetAllDetailed(currencyType, index, countPerIndex);
        }

        [HttpGet]
        public NozomiResult<IReadOnlyDictionary<string, long>> GetSlugToIdMap()
        {
            return new NozomiResult<IReadOnlyDictionary<string, long>>(_currencyEvent.ListAllMapped());
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
