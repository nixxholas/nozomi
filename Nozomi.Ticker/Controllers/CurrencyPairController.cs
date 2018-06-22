using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Nozomi.Data;
using Nozomi.Data.CurrencyModels;
using Nozomi.Service.Hubs;

namespace Nozomi.Ticker.Controllers
{
    [ApiController]
    public class CurrencyPairController : ControllerBase
    {
        private readonly IHubContext<TickerHub> _tickerHubContext;

        public CurrencyPairController(IHubContext<TickerHub> tickerHubContext)
        {
            _tickerHubContext = tickerHubContext;
        }
        
        [HttpPost]
        public async Task CreateCurrencyPair(CurrencyPair currencyPair)
        {
            
        }
    }
}