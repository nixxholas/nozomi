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
using Nozomi.Infra.Auth.Services.ApiKey;

namespace Nozomi.Auth.Controllers.ApiKey
{
    [SecurityHeaders]
    public class ApiKeyController : BaseController<ApiKeyController>, IApiKeyController
    {
        private readonly UserManager<User> _userManager;
        private readonly IApiKeyService _apiKeyService;
        
        public ApiKeyController(ILogger<ApiKeyController> logger, IWebHostEnvironment webHostEnvironment,
            UserManager<User> userManager, IApiKeyService apiKeyService) 
            : base(logger, webHostEnvironment)
        {
            _userManager = userManager;
            _apiKeyService = apiKeyService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            // Validate
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Safetynet
            if (user == null)
                return BadRequest("Please reauthenticate again!");
            
            // Generate the API Key
            _apiKeyService.GenerateApiKey(user.Id);
            
            return Ok("API Key successfully created!"); // OK!
        }

        public async Task<IActionResult> Revoke(string apiKey)
        {
            throw new System.NotImplementedException();
        }
    }
}