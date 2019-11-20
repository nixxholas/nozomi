using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing;

namespace Nozomi.Web2.Controllers.APIs.v1
{
    [Area("api")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ApiVersion(GlobalApiVariables.V1_MAJOR_VERSION, Deprecated = false)]
    public class BaseApiController<T> : Controller where T : class
    {
        protected readonly ILogger<T> _logger;

        public BaseApiController(ILogger<T> logger)
        {
            _logger = logger;
        }
    }
}