using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.ViewModels.Manage;
using Nozomi.Data.AreaModels.v1.CurrencySource;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class SourceController : BaseViewController<SourceController>
    {
        private readonly ISourceEvent _sourceEvent;
        private readonly ISourceService _sourceService;
        
        public SourceController(ILogger<SourceController> logger, NozomiSignInManager signInManager, 
            NozomiUserManager userManager, ISourceEvent sourceEvent, ISourceService sourceService) 
            : base(logger, signInManager, userManager)
        {
            _sourceEvent = sourceEvent;
            _sourceService = sourceService;
        }

        #region Get CreateSource
        
        [HttpGet]
        public async Task<IActionResult> CreateSource()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            return View();
        }
        
        #endregion
        
        #region Get Sources
        
        [HttpGet]
        public async Task<IActionResult> Sources()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Will be using IndexViewModel for now because it does the same thing
            var vm = new IndexViewModel
            {
                Sources = _sourceEvent.GetAll(true).ToList()
            };

            return View(vm);
        }
        
        #endregion

        #region Post CreateSource
        
        [HttpPost]
        public async Task<IActionResult> CreateSource(CreateSource createSource)
        {  
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // If it works
            if (!ModelState.IsValid)
            {
                
            }

            var res = _sourceService.Create(createSource);
            
            return RedirectToAction("CreateSource");
        }
        
        #endregion

        #region Put EditSource
        
        [HttpPut("{id}")]
        public async Task<IActionResult> EditSource(long id, UpdateSource updateSource)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (id != updateSource.Id)
            {
                return BadRequest();
            }

            if (_sourceService.StaffSourceUpdate(updateSource)) return Ok();

            // Update failed.
            return NotFound();
        }
            
        #endregion
        
        #region Delete DeleteSource
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSource(long id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if(_sourceService.Delete(id)) return Ok();

            // Update failed.
            return NotFound();
        }
        
        #endregion
    }
}