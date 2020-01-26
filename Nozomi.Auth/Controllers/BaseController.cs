using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Nozomi.Auth.Controllers
{
    public class BaseController<T> : Controller where T : class
    {
        protected readonly ILogger<T> _logger;
        protected readonly IWebHostEnvironment _webHostEnvironment;

        public BaseController(ILogger<T> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }
    }
}