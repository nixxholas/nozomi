using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Preprocessing;

namespace Nozomi.Ticker.Controllers.APIs.v1
{
    [Route("/api/[controller]/[action]")]
    [ApiVersion(GlobalApiVariables.V1_MAJOR_VERSION, Deprecated = false)]
    public class BaseController<T> : ControllerBase where T : class
    {
        private readonly UserManager<User> _userManager;
        protected readonly ILogger<T> _logger;
        
        public BaseController(ILogger<T> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        protected Task<User> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}