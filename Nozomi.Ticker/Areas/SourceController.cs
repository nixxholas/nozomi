using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Ticker.Areas
{
    public class SourceController : BaseViewController<SourceController>
    {
        public SourceController(ILogger<SourceController> logger, NozomiSignInManager signInManager, 
            NozomiUserManager userManager) : base(logger, signInManager, userManager)
        {
        }
        
        // GET
        [HttpGet("{id}")]
        public IActionResult View([FromRoute]long id)
        {
            return View();
        }
    }
}