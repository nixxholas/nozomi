using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.ResponseModels.CurrencyPair;
using Nozomi.Data.ViewModels.CurrencyPair;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Statics;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web2.Controllers.v1.CurrencyPair
{
    [ApiController]
    public class CurrencyPairController : BaseApiController<CurrencyPairController>, ICurrencyPairController
    {
        private readonly ICurrencyPairEvent _currencyPairEvent;
        private readonly ICurrencyPairService _currencyPairService;
        private readonly ITickerEvent _tickerEvent;

        public CurrencyPairController(ICurrencyPairEvent currencyPairEvent, ITickerEvent tickerEvent, 
            ICurrencyPairService currencyPairService, ILogger<CurrencyPairController> logger)
            : base(logger)
        {
            _currencyPairEvent = currencyPairEvent;
            _currencyPairService = currencyPairService;
            _tickerEvent = tickerEvent;
        }

        [HttpGet]
        public IActionResult All([FromQuery]int page = 0, [FromQuery]int itemsPerPage = 50, 
            [FromQuery]string sourceGuid = null, [FromQuery]string mainTicker = null, 
            [FromQuery]bool orderAscending = true, [FromQuery]string orderingParam = "TickerPair")
        {
            if (page >= 0 && itemsPerPage <= NozomiServiceConstants.CurrencyPairTakeoutLimit)
            {
                return Ok(_currencyPairEvent.All(page, itemsPerPage, sourceGuid, mainTicker, orderAscending, 
                    orderingParam));
            }

            return BadRequest("Invalid request.");
        }

        [HttpGet]
        public IActionResult Count(string mainTicker = null)
        {
            return Ok(_currencyPairEvent.GetCount(mainTicker));
        }

        [Authorize(Roles = NozomiPermissions.AllowAllStaffRoles)]
        [HttpPost]
        public IActionResult Create([FromBody]CreateCurrencyPairViewModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (string.IsNullOrWhiteSpace(sub))
                return BadRequest("Please login again. Your session may have expired!");
            
            if (vm.IsValid())
            {
                // Create the entity
                _currencyPairService.Create(vm, sub);
                return Ok();
            }

            return BadRequest("Invalid payload.");
        }

        [HttpGet("{id}")]
        public Task Get(long id)
        {
            return _tickerEvent.GetById(id);
        }

        [Authorize]
        [HttpGet]
        public ICollection<DistinctCurrencyPairResponse> ListAll()
        {
            return _currencyPairEvent.ListAll();
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("{abbrv}")]
        [Obsolete]
        public NozomiResult<ICollection<Data.Models.Currency.CurrencyPair>> Ticker(string abbrv)
        {
            return new NozomiResult<ICollection<Data.Models.Currency.CurrencyPair>>(
                _currencyPairEvent.GetAllByTickerPairAbbreviation(abbrv, true));
        }
    }
}
