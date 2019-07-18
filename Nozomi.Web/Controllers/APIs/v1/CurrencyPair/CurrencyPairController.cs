using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Hubs;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Controllers.APIs.v1.CurrencyPair
{
    [ApiController]
    public class CurrencyPairController : BaseController<CurrencyPairController>, ICurrencyPairController
    {
        private readonly ICurrencyPairEvent _currencyPairEvent;
        private readonly ITickerEvent _tickerEvent;

        public CurrencyPairController(NozomiUserManager userManager,
            ICurrencyPairEvent currencyPairEvent, ITickerEvent tickerEvent,
            ILogger<CurrencyPairController> logger)
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
