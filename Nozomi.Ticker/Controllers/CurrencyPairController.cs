using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Nozomi.Data;
using Nozomi.Data.CurrencyModels;
using Nozomi.Service.Hubs;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class CurrencyPairController : ControllerBase
    {
        private readonly ICurrencyPairService _currencyPairService;
        private readonly IHubContext<TickerHub> _tickerHubContext;

        public CurrencyPairController(IHubContext<TickerHub> tickerHubContext, ICurrencyPairService currencyPairService)
        {
            _tickerHubContext = tickerHubContext;
            _currencyPairService = currencyPairService;
        }
        
        [HttpPost("Create")]
        public async Task CreateCurrencyPair(CurrencyPair currencyPair)
        {
            var res = _currencyPairService.Create(currencyPair, currencyPair.CreatedBy);
        }
    }
}