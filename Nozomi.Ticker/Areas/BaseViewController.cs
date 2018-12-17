using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.Models.Identity;

namespace Nozomi.Ticker.Areas
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BaseViewController<TController> : Controller where TController : class
    {
        public readonly ILogger<TController> _logger;
        public readonly UserManager<User> _userManager;
        
        public BaseViewController(ILogger<TController> logger,
            UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }
        
        #region Helpers

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        protected Task<User> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        protected IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}