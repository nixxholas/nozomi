using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Data.ResponseModels;
using Nozomi.Service.Hubs;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Areas.v1.CurrencyPair
{
    [ApiController]
    public class CurrencyPairController : BaseController<CurrencyPairController>, ICurrencyPairController
    {
        private readonly ICurrencyPairService _currencyPairService;
        private readonly ITickerService _tickerService;
        private readonly IHubContext<TickerHub> _tickerHubContext;

        public CurrencyPairController(IHubContext<TickerHub> tickerHubContext, ICurrencyPairService currencyPairService,
            ITickerService tickerService, ILogger<CurrencyPairController> logger)
            : base(logger)
        {
            _tickerHubContext = tickerHubContext;
            _currencyPairService = currencyPairService;
            _tickerService = tickerService;
        }

        [Authorize]
        [HttpPost]
        public async Task<NozomiResult<string>> Create([FromBody]CreateCurrencyPair currencyPair)
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
            return _tickerService.GetById(id);
        }

        [HttpGet("{abbreviation}")]
        public NozomiResult<ICollection<DistinctiveTickerResponse>> Ticker(string abbreviation, string exchangeAbbrv = null)
        {
            return _tickerService.GetByAbbreviation(abbreviation, exchangeAbbrv);
        }
        
        
    }
}