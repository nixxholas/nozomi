using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Nozomi.Preprocessing.Abstracts
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BaseApiController<T> : Controller where T : class
    {
        protected readonly ILogger<T> _logger;

        public BaseApiController(ILogger<T> logger)
        {
            _logger = logger;
        }
    }
}