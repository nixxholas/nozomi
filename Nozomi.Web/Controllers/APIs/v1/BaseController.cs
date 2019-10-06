using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Preprocessing;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Web.Controllers.APIs.v1
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
