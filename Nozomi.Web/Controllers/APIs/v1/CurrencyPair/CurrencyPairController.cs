using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Web.Controllers.APIs.v1.CurrencyPair
{
    [ApiController]
    public class CurrencyPairApiController : BaseApiController<CurrencyPairApiController>, ICurrencyPairController
    {
        private readonly ICurrencyPairEvent _currencyPairEvent;
        private readonly ITickerEvent _tickerEvent;

        public CurrencyPairApiController(NozomiUserManager userManager,
            ICurrencyPairEvent currencyPairEvent, ITickerEvent tickerEvent,
            ILogger<CurrencyPairApiController> logger)
            : base(logger, userManager)
        {
            _currencyPairEvent = currencyPairEvent;
            _tickerEvent = tickerEvent;
        }

        [HttpGet("{id}")]
        public Task Get(long id)
        {
            return _tickerEvent.GetById(id);
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("{abbrv}")]
        public NozomiResult<ICollection<Data.Models.Currency.CurrencyPair>> Ticker(string abbrv)
        {
            return new NozomiResult<ICollection<Data.Models.Currency.CurrencyPair>>(
                _currencyPairEvent.GetAllByTickerPairAbbreviation(abbrv, true));
        }
    }
}
