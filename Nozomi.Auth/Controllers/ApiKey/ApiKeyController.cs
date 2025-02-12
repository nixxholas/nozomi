using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.Auth.ViewModels.ApiKey;
using Nozomi.Infra.Auth.Events.ApiKey;
using Nozomi.Infra.Auth.Services.ApiKey;
using Nozomi.Preprocessing.Attributes;

namespace Nozomi.Auth.Controllers.ApiKey
{
    [SecurityHeaders]
    public class ApiKeyController : BaseController<ApiKeyController>, IApiKeyController
    {
        private readonly UserManager<User> _userManager;
        private readonly IApiKeyEvent _apiKeyEvent;
        private readonly IApiKeyService _apiKeyService;
        
        public ApiKeyController(ILogger<ApiKeyController> logger, IWebHostEnvironment webHostEnvironment,
            UserManager<User> userManager, IApiKeyEvent apiKeyEvent, IApiKeyService apiKeyService) 
            : base(logger, webHostEnvironment)
        {
            _userManager = userManager;
            _apiKeyEvent = apiKeyEvent;
            _apiKeyService = apiKeyService;
        }

        [Throttle(Milliseconds = 1000, Name = "ApiKey/All", Message = "Hey don't spam!")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> All()
        {
            // Validate
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Safetynet
            if (user == null)
                return BadRequest("Please reauthenticate again!");

            return Ok(_apiKeyEvent.ViewAll(user.Id)); // OK!
        }

        [Throttle(Milliseconds = 1500, Name = "ApiKey/Create", Message = "Hey don't spam!")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("{label}")]
        public async Task<IActionResult> Create(string label = null)
        {
            // Validate
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Safetynet
            if (user == null)
                return BadRequest("Please reauthenticate again!");
            
            // Generate the API Key
            _apiKeyService.GenerateApiKey(user.Id, label);
            
            return Ok("API Key successfully created!"); // OK!
        }

        [Throttle(Milliseconds = 2500, Name = "ApiKey/Reveal", Message = "Hey don't spam!")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Reveal()
        {
            // Validate
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Safetynet
            if (user == null)
                return BadRequest("Please reauthenticate again!");
            
            // Reveal the API Key
            return Ok(_apiKeyEvent.Reveal(user.Id));
        }

        [Throttle(Milliseconds = 2500, Name = "ApiKey/Revoke", Message = "Hey don't spam!")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> Revoke([FromBody] RevokeInputViewModel vm)
        {
            // Validate
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Safetynet
            if (user == null)
                return BadRequest("Please reauthenticate again!");
            
            // Generate the API Key
            _apiKeyService.RevokeApiKey(vm.ApiKeyGuid, user.Id);
            
            return Ok("API Key successfully revoked."); // OK!
        }
    }
}