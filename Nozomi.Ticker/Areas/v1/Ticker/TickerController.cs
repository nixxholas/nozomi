using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.ResponseModels;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Areas.v1.Ticker
{
    public class TickerController : BaseController<TickerController>, ITickerController
    {
        private readonly ITickerService _tickerService;
        
        public TickerController(ILogger<TickerController> logger,
            ITickerService tickerService) : base(logger)
        {
            _tickerService = tickerService;
        }

        [HttpGet]
        public NozomiResult<ICollection<DistinctiveTickerResponse>> Get(string symbol, bool includeNested = false)
        {
            return _tickerService.GetByAbbreviation(symbol);
        }
    }
}