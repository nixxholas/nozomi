using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL;
using Nozomi.Base.BCL.Responses;
using Nozomi.Data;
using Nozomi.Data.ViewModels.Currency;
using Nozomi.Preprocessing.Attributes;
using Nozomi.Preprocessing.Statics;
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

        [Authorize(Roles = NozomiPermissions.AllowAllStaffRoles)]
        [HttpPost]
        [Throttle(Name = "Currency/Create", Milliseconds = 2000)]
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

        [Authorize(Roles = NozomiPermissions.AllowAllStaffRoles)]
        [HttpPut]
        [Throttle(Name = "Currency/Edit", Milliseconds = 2000)]
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
        [Throttle(Name = "Currency/All", Milliseconds = 1000)]
        public IActionResult All([FromQuery]string currencyType = "CRYPTO", [FromQuery]int itemsPerIndex = 20,
            [FromQuery]int index = 0, [FromQuery]CurrencySortingEnum currencySortType = CurrencySortingEnum.None, 
            [FromQuery]Data.Models.Web.Analytical.AnalysedComponentType sortType = 
                Data.Models.Web.Analytical.AnalysedComponentType.MarketCap, [FromQuery]bool orderDescending = true, 
            [FromQuery]ICollection<Data.Models.Web.Analytical.AnalysedComponentType> typesToTake = null,
            [FromQuery]ICollection<Data.Models.Web.Analytical.AnalysedComponentType> typesToDeepen = null)
        {
            return Ok(_currencyEvent.All(currencyType, itemsPerIndex, index, currencySortType, sortType, 
                orderDescending, typesToTake, typesToDeepen));
        }

        [HttpGet]
        public IActionResult GetCountByType(string currencyType)
        {
            var count = _currencyEvent.GetCountByType(currencyType);

            return Ok(count);
        }

        [HttpGet("{slug}")]
        [Throttle(Name = "Currency/GetPairCount", Milliseconds = 1000)]
        public IActionResult GetPairCount([FromRoute]string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return BadRequest("Invalid slug input.");
            
            return Ok(_currencyPairEvent.GetCount(slug));
        }

        [HttpGet("{slug}")]
        [Throttle(Name = "Currency/Get", Milliseconds = 250)]
        public CurrencyViewModel Get(string slug)
        {
            return _currencyEvent.Get(slug);
        }

        [HttpGet]
        [Throttle(Name = "Currency/ListAllSlugs", Milliseconds = 1000)]
        public NozomiResult<ICollection<string>> ListAllSlugs()
        {
            return new NozomiResult<ICollection<string>>(_currencyEvent.ListAllSlugs());
        }

        [HttpGet]
        [Throttle(Name = "Currency/List", Milliseconds = 250)]
        public IActionResult List([FromQuery]string slug = null)
        {
            return Ok(_currencyEvent.All(slug));
        }

        [HttpGet]
        [Throttle(Name = "Currency/ListAll", Milliseconds = 100)]
        public ICollection<CurrencyViewModel> ListAll([FromQuery]int page = 0, [FromQuery]int itemsPerPage = 50, 
            [FromQuery]string currencyTypeName = null, [FromQuery]bool orderAscending = true, 
            [FromQuery]CurrencySortingEnum orderingParam = CurrencySortingEnum.None)
        {
            return _currencyEvent.ListAll(page, itemsPerPage, currencyTypeName, orderAscending, orderingParam).ToList();
        }

        [HttpGet]
        [Throttle(Name = "Currency/GetSlugToIdMap", Milliseconds = 2000)]
        [Obsolete]
        public NozomiResult<IReadOnlyDictionary<string, long>> GetSlugToIdMap()
        {
            return new NozomiResult<IReadOnlyDictionary<string, long>>(_currencyEvent.ListAllMapped());
        }
    }
}
