using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;
using Nozomi.Base.Core.Responses;
using Nozomi.Data;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Data.ViewModels.Currency;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web2.Controllers.v1.Currency
{
    [ApiController]
    public class CurrencyController : BaseApiController<CurrencyController>, ICurrencyController
    {
        private readonly IAnalysedHistoricItemEvent _analysedHistoricItemEvent;
        private readonly ICurrencyEvent _currencyEvent;
        private readonly ICurrencyPairEvent _currencyPairEvent;
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ILogger<CurrencyController> logger,
            IAnalysedHistoricItemEvent analysedHistoricItemEvent, ICurrencyEvent currencyEvent, 
            ICurrencyPairEvent currencyPairEvent, ICurrencyService currencyService)
            : base(logger)
        {
            _analysedHistoricItemEvent = analysedHistoricItemEvent;
            _currencyEvent = currencyEvent;
            _currencyPairEvent = currencyPairEvent;
            _currencyService = currencyService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateCurrencyViewModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (!string.IsNullOrWhiteSpace(sub))
            {
                _currencyService.Create(vm, sub);

                return Ok();
            }

            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpPut]
        public IActionResult Edit(ModifyCurrencyViewModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (!string.IsNullOrWhiteSpace(sub))
            {
                _currencyService.Edit(vm, sub);

                return Ok();
            }

            return BadRequest("Please re-authenticate again");
        }

        [HttpGet]
        public IActionResult All([FromQuery]string currencyType = "CRYPTO", [FromQuery]int itemsPerIndex = 20,
            [FromQuery]int index = 0, [FromQuery]Data.Models.Web.Analytical.AnalysedComponentType sortType = 
                Data.Models.Web.Analytical.AnalysedComponentType.MarketCap, [FromQuery]bool orderDescending = true, 
            [FromQuery]ICollection<Data.Models.Web.Analytical.AnalysedComponentType> typesToTake = null,
            [FromQuery]ICollection<Data.Models.Web.Analytical.AnalysedComponentType> typesToDeepen = null)
        {
            return Ok(_currencyEvent.All(currencyType, itemsPerIndex, index, sortType, orderDescending, typesToTake,
                typesToDeepen));
        }

        [HttpGet]
        public IActionResult GetCountByType(string currencyType)
        {
            var count = _currencyEvent.GetCountByType(currencyType);

            return Ok(count);
        }

        [HttpGet("{slug}")]
        public IActionResult GetPairCount([FromRoute]string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return BadRequest("Invalid slug input.");
            
            return Ok(_currencyPairEvent.GetCount(slug));
        }

        [HttpGet("{slug}")]
        public CurrencyViewModel Get(string slug)
        {
            return _currencyEvent.Get(slug);
        }

        [HttpGet]
        public NozomiResult<ICollection<string>> ListAllSlugs()
        {
            return new NozomiResult<ICollection<string>>(_currencyEvent.ListAllSlugs());
        }

        [HttpGet]
        public ICollection<CurrencyViewModel> ListAll([FromQuery]int page = 0, [FromQuery]int itemsPerPage = 50, 
            [FromQuery]string currencyTypeName = null, [FromQuery]bool orderAscending = true, 
            [FromQuery]string orderingParam = "Name")
        {
            return _currencyEvent.ListAll(page, itemsPerPage, currencyTypeName, orderAscending, orderingParam).ToList();
        }

        [HttpGet("{slug}")]
        public NozomiResult<DetailedCurrencyResponse> Detailed(string slug)
        {
            return new NozomiResult<DetailedCurrencyResponse>(_currencyEvent.GetDetailedBySlug(slug,
                new List<Data.Models.Currency.ComponentType>()
                {
                    Data.Models.Currency.ComponentType.CirculatingSupply
                },
                new List<Data.Models.Web.Analytical.AnalysedComponentType>()
                {
                    Data.Models.Web.Analytical.AnalysedComponentType.MarketCap,
                    Data.Models.Web.Analytical.AnalysedComponentType.CurrentAveragePrice,
                    Data.Models.Web.Analytical.AnalysedComponentType.HourlyAveragePrice
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
        [Obsolete]
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
