using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
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

        [HttpPost]
        public async Task<NozomiResult<Data.CurrencyModels.CurrencyPair>> Create([FromBody]Data.CurrencyModels.CurrencyPair currencyPair)
        {
            if (_currencyPairService.Create(currencyPair, currencyPair.CreatedBy))
            {
                return new NozomiResult<Data.CurrencyModels.CurrencyPair>()
                {
                    ResultType = NozomiResultType.Success
                };
            }
            
            return new NozomiResult<Data.CurrencyModels.CurrencyPair>()
            {
                ResultType = NozomiResultType.Failed
            };
        }

        [HttpGet("{id}")]
        public Task Ticker(long id)
        {
            return _tickerService.GetById(id);
        }

        [HttpGet("{abbreviation}")]
        public NozomiResult<ICollection<DistinctiveTickerResponse>> Ticker(string abbreviation, [FromRoute]string exchangeAbbrv = null)
        {
            return _tickerService.GetByAbbreviation(abbreviation, exchangeAbbrv);
        }
        
        
    }
}