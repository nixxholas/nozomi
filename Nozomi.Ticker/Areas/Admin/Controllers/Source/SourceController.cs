using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Source;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers.Source
{
    [Area("Admin")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class SourceController : AreaBaseViewController<SourceController>
    {
        private readonly ISourceEvent _sourceEvent;
        private readonly ISourceService _sourceService;
        
        public SourceController(ILogger<SourceController> logger, ISourceEvent sourceEvent, ISourceService sourceService, 
            SignInManager<User> signInManager, UserManager<User> userManager)
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
                Sources = _sourceEvent.GetAllNonDeleted(true).ToList()
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

            if (!ModelState.IsValid)
                return BadRequest("Invalid payload, please provide the missing properties.");

            var result = _sourceService.Create(createSource);
            
            if (result.ResultType.Equals(NozomiResultType.Success)) return Ok(result);

            return BadRequest(result);
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