using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Data.ViewModels.Source;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Users.Controllers
{
    [Area("Users")]
    public class SourceController : BaseViewController<SourceController>
    {
        private ISourceEvent _sourceEvent { get; set; }
        
        public SourceController(ILogger<SourceController> logger, SignInManager<User> signInManager, 
            UserManager<User> userManager, ISourceEvent sourceEvent) : base(logger, signInManager, userManager)
        {
            _sourceEvent = sourceEvent;
        }
        
        // GET Source by abbreviation
        [HttpGet("/[controller]/{abbrv}")]
        public IActionResult Source([FromRoute]string abbrv)
        {
            var vm = new ViewSourceModel
            {
                Source = _sourceEvent.Get(abbrv.ToUpper())
            };
            
            return View(vm);
        }
    }
}