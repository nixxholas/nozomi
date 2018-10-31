using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Nozomi.Ticker.Areas.v1
{
    [Route("/api/[controller]/[action]")]
    [ApiVersion("v1.1", Deprecated = false)]
    public class BaseController<T> : ControllerBase where T : class
    {
        private readonly ILogger<T> _logger;
        
        public BaseController(ILogger<T> logger)
        {
            _logger = logger;
        }
    }
}