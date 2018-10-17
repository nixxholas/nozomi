using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.CurrencyModels;
using Nozomi.Service.Hubs;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Ticker.Controllers.v1.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

namespace Nozomi.Ticker.Controllers.v1
{
    [ApiController]
    public class CurrencyPairController : BaseController<CurrencyPairController>, ICurrencyPairController
    {
        private readonly ICurrencyPairService _currencyPairService;
        private readonly IHubContext<TickerHub> _tickerHubContext;

        public CurrencyPairController(IHubContext<TickerHub> tickerHubContext, ICurrencyPairService currencyPairService,
            ILogger<CurrencyPairController> logger) 
            : base(logger)
        {
            _tickerHubContext = tickerHubContext;
            _currencyPairService = currencyPairService;
        }
        
        [HttpPost("create")]
        public async Task<NozomiResult<CurrencyPair>> CreateCurrencyPair(CurrencyPair currencyPair)
        {
            if (_currencyPairService.Create(currencyPair, currencyPair.CreatedBy))
            {
                return new NozomiResult<CurrencyPair>()
                {
                    ResultType = NozomiResultType.Success
                };
            }
            
            return new NozomiResult<CurrencyPair>()
            {
                ResultType = NozomiResultType.Failed
            };
        }

        [HttpGet("ticker")]
        public Task Ticker(long id)
        {
            
        }

        public Task Ticker(string abbreviation)
        {
            throw new System.NotImplementedException();
        }
    }
}