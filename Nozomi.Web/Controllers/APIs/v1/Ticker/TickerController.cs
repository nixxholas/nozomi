using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.ResponseModels.Ticker;
using Nozomi.Data.ResponseModels.TickerPair;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Web.Controllers.APIs.v1.Ticker
{
    public class TickerApiController : BaseApiController<TickerApiController>, ITickerController
    {
        private readonly ITickerEvent _tickerEvent;

        public TickerApiController(ILogger<TickerApiController> logger, NozomiUserManager userManager,
            ITickerEvent tickerEvent) : base(logger, userManager)
        {
            _tickerEvent = tickerEvent;
        }

        [HttpGet]
        public NozomiResult<ICollection<TickerPairResponse>> GetTickerPairSources()
        {
            return new NozomiResult<ICollection<TickerPairResponse>>(_tickerEvent.GetAllTickerPairSources());
        }

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
