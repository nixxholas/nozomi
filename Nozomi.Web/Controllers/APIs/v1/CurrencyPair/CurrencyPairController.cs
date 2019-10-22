using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.ResponseModels.CurrencyPair;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Web.Controllers.APIs.v1.CurrencyPair
{
    [ApiController]
    public class CurrencyPairController : BaseApiController<CurrencyPairController>, ICurrencyPairController
    {
        private readonly ICurrencyPairEvent _currencyPairEvent;
        private readonly ITickerEvent _tickerEvent;

        public CurrencyPairController(ICurrencyPairEvent currencyPairEvent,
            ITickerEvent tickerEvent, ILogger<CurrencyPairController> logger)
            : base(logger)
        {
            _currencyPairEvent = currencyPairEvent;
            _tickerEvent = tickerEvent;
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
        public NozomiResult<ICollection<Data.Models.Currency.CurrencyPair>> Ticker(string abbrv)
        {
            return new NozomiResult<ICollection<Data.Models.Currency.CurrencyPair>>(
                _currencyPairEvent.GetAllByTickerPairAbbreviation(abbrv, true));
        }
    }
}
