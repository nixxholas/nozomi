using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.UI;
using Nozomi.Data;
using Nozomi.Data.ResponseModels;
using Nozomi.Preprocessing;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Identity.Requirements;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Areas.v1.Ticker
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

        [HttpGet]
        public Task<DataTableResult<UniqueTickerResponse>> GetAllForDataTables(int Draw = 0)
        {
            return Task.FromResult(_tickerEvent.GetAllForDatatable(Draw));
        }

        [HttpGet("{index}")]
        public async Task<NozomiResult<ICollection<UniqueTickerResponse>>> GetAllAsync(int index = 0)
        {
            return await _tickerService.GetAll(index);
        }

        [HttpGet]
        public NozomiResult<ICollection<DistinctiveTickerResponse>> Get(string symbol, bool includeNested = false)
        {
            return _tickerService.GetByAbbreviation(symbol);
        }
    }
}