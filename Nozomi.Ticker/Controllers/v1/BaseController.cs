using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Nozomi.Ticker.Controllers.v1
{
    [Route("/api/[controller]")]
    [ApiVersion("v1.0", Deprecated = false)]
    public class BaseController<T> : ControllerBase where T : class
    {
        private readonly ILogger<T> _logger;
        
        public BaseController(ILogger<T> logger)
        {
            _logger = logger;
        }
    }
}