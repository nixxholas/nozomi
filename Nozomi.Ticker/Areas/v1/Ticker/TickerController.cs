using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.ResponseModels;

namespace Nozomi.Ticker.Areas.v1.Ticker
{
    public class TickerController : BaseController<TickerController>, ITickerController
    {
        public TickerController(ILogger<TickerController> logger) : base(logger)
        {
        }

        public NozomiResult<ICollection<DistinctiveTickerResponse>> Get(long currencySourceId, string symbol, bool includeNested = false)
        {
            throw new System.NotImplementedException();
        }
    }
}