using Microsoft.Extensions.Logging;

namespace Nozomi.Ticker.Areas.v1.CurrencyPairRequest
{
    public class CurrencyPairRequestController : BaseController<CurrencyPairRequestController>, ICurrencyPairRequestController
    {
        public CurrencyPairRequestController(ILogger<CurrencyPairRequestController> logger) : base(logger)
        {
        }
        
        
    }
}