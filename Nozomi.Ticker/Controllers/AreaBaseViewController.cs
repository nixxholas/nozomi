using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Service.Identity.Managers;
using Nozomi.Ticker.Areas.Users.Controllers;

namespace Nozomi.Ticker.Controllers
{
    [Route("[area]/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AreaBaseViewController<TController>: Controller where TController: class
    {
        public readonly ILogger<TController> _logger;
        public readonly NozomiSignInManager _signInManager;
        public readonly NozomiUserManager _userManager;

        public AreaBaseViewController(ILogger<TController> logger,
            NozomiSignInManager signInManager, NozomiUserManager userManager)
        {
            _logger = logger;
            _signInManager = signInManager;
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