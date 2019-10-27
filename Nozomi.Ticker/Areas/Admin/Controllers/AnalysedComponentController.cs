using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Admin.Domain.AreaModels.AnalysedComponent;
using Nozomi.Base.Auth.Models;
using Nozomi.Infra.Admin.Service.Services.Interfaces;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class AnalysedComponentController : AreaBaseViewController<AnalysedComponentController>
    {
        private readonly IAnalysedComponentService _analysedComponentService;
        public AnalysedComponentController(ILogger<AnalysedComponentController> logger, 
            IAnalysedComponentService analysedComponentService, SignInManager<User> signInManager,
            UserManager<User> userManager)
            : base(logger, signInManager, userManager)
        {
            _analysedComponentService = analysedComponentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateAnalysedComponent createAnalysedComponent)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            var res = _analysedComponentService.Create(createAnalysedComponent);

            if (res > 0)
                return Ok("Analysed component successfully created!");

            return BadRequest("Invalid component payload.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            var res = _analysedComponentService.Delete(id, false);

            if (res)
                return Ok("Analysed component successfully created!");
            
            return BadRequest("Invalid Analysed Component.");
        }
    }
}