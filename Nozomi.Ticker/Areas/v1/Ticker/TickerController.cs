using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.ResponseModels;
using Nozomi.Preprocessing;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Identity.Requirements;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Areas.v1.Ticker
{
    public class TickerController : BaseController<TickerController>, ITickerController
    {
        private readonly ITickerService _tickerService;
        
        public TickerController(ILogger<TickerController> logger, NozomiUserManager userManager,
            ITickerService tickerService) : base(logger, userManager)
        {
            _tickerService = tickerService;
        }

        [HttpGet("{index}")]
        public async Task<NozomiResult<ICollection<UniqueTickerResponse>>> GetAllAsync(int index = 0)
        {
            return await _tickerService.Get(index);
        }

        [Authorize(Policy = ApiTokenRequirement.ApiTokenRequirementName)]
        [HttpGet]
        public NozomiResult<ICollection<DistinctiveTickerResponse>> Get(string symbol, bool includeNested = false)
        {
            return _tickerService.GetByAbbreviation(symbol);
        }
    }
}