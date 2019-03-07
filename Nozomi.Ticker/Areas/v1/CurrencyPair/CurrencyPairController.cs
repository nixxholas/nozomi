using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Data.ResponseModels;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Hubs;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Areas.v1.CurrencyPair
{
    [ApiController]
    public class CurrencyPairController : BaseController<CurrencyPairController>, ICurrencyPairController
    {
        private readonly ICurrencyPairService _currencyPairService;
        private readonly ITickerEvent _tickerEvent;
        private readonly IHubContext<NozomiStreamHub> _tickerHubContext;

        public CurrencyPairController(IHubContext<NozomiStreamHub> tickerHubContext, NozomiUserManager userManager,
            ICurrencyPairService currencyPairService, ITickerEvent tickerEvent,
            ILogger<CurrencyPairController> logger)
            : base(logger, userManager)
        {
            _tickerHubContext = tickerHubContext;
            _currencyPairService = currencyPairService;
            _tickerEvent = tickerEvent;
        }

        [Authorize]
        [HttpPost]
        public async Task<NozomiResult<string>> Create([FromBody] CreateCurrencyPair currencyPair)
        {
            if (_currencyPairService.Create(currencyPair, 0 /* 0 for now */))
            {
                return new NozomiResult<string>()
                {
                    ResultType = NozomiResultType.Success,
                    Data = "Currency pair successfully created!"
                };
            }

            return new NozomiResult<string>()
            {
                ResultType = NozomiResultType.Failed,
                Data = "An error has occurred"
            };
        }

        [HttpGet("{id}")]
        public Task Ticker(long id)
        {
            return _tickerEvent.GetById(id);
        }
    }
}