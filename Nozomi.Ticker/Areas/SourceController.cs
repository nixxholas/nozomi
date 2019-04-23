using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.Source;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Ticker.Areas
{
    public class SourceController : BaseViewController<SourceController>
    {
        private ISourceEvent _sourceEvent { get; set; }
        
        public SourceController(ILogger<SourceController> logger, NozomiSignInManager signInManager, 
            NozomiUserManager userManager, ISourceEvent sourceEvent) : base(logger, signInManager, userManager)
        {
            _sourceEvent = sourceEvent;
        }
        
        // GET All Sources
        [HttpGet]
        [Authorize(Roles = "Owner, Administrator, Staff")]
        public async Task<IActionResult> Sources()
        {
            var vm = new SourcesViewModel
            {
                Sources = _sourceEvent.GetAllActive(true).ToList());
            };

            return View(vm);

        }
        
        // GET Source by abbreviation
        [HttpGet("{controller}/{abbrv}")]
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