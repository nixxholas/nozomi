using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Nozomi.Auth.Controllers
{
    public class BaseController<T> : Controller where T : class
    {
        protected readonly ILogger<T> _logger;

        public BaseController(ILogger<T> logger)
        {
            _logger = logger;
        }
    }
}