using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Preprocessing;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Ticker.Controllers.APIs.v1
{
    [Route("/api/[controller]/[action]")]
    [ApiVersion(GlobalApiVariables.V1_MAJOR_VERSION, Deprecated = false)]
    public class BaseController<T> : ControllerBase where T : class
    {
        private readonly NozomiUserManager _userManager;
        protected readonly ILogger<T> _logger;
        
        public BaseController(ILogger<T> logger, NozomiUserManager nozomiUserManager)
        {
            _logger = logger;
            _userManager = nozomiUserManager;
        }

        protected Task<User> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}