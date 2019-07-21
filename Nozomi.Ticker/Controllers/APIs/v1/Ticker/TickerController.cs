using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.ResponseModels.Ticker;
using Nozomi.Data.ResponseModels.TickerPair;
using Nozomi.Infra.Admin.Service.Services.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Controllers.APIs.v1.Ticker
{
    public class TickerController : BaseController<TickerController>, ITickerController
    {
        private readonly ITickerEvent _tickerEvent;
        private readonly ITickerService _tickerService;
        
        public TickerController(ILogger<TickerController> logger, NozomiUserManager userManager,
            ITickerEvent tickerEvent, ITickerService tickerService) : base(logger, userManager)
        {
            _tickerEvent = tickerEvent;
            _tickerService = tickerService;
        }

        [Authorize]
        [HttpDelete]
        public NozomiResult<string> Delete(string tickerSymbol, string exchangeAbbreviation)
        {
            return _tickerService.Delete(tickerSymbol, exchangeAbbreviation);
        }

        [HttpGet]
        public NozomiResult<ICollection<TickerPairResponse>> GetTickerPairSources()
        {
            return new NozomiResult<ICollection<TickerPairResponse>>(_tickerEvent.GetAllTickerPairSources());
        }

//        [HttpGet]
//        public Task<DataTableResult<UniqueTickerResponse>> GetAllForDataTables(int draw = 0)
//        {
//            return Task.FromResult(_tickerEvent.GetAllForDatatable(draw));
//        }

        [HttpGet("{index}")]
        public async Task<NozomiResult<ICollection<UniqueTickerResponse>>> GetAllAsync(int index = 0)
        {
            if (index < 0) return new NozomiResult<ICollection<UniqueTickerResponse>>(
                NozomiResultType.Failed, "Please enter a proper index.");
            
            return await _tickerEvent.GetAll(index);
        }

        [HttpGet]
        public NozomiResult<ICollection<TickerByExchangeResponse>> Get(string symbol, string exchangeAbbrv = null)
        {
            if (string.IsNullOrEmpty(symbol)) return new NozomiResult<ICollection<TickerByExchangeResponse>>(
                NozomiResultType.Failed, "Please enter a symbol.");
            
            return _tickerEvent.GetByAbbreviation(symbol, exchangeAbbrv);
        }
    }
}