using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;
using Nozomi.Infra.Auth.Services.User;

namespace Nozomi.Auth.Controllers.Payment
{
    public class PaymentController : BaseController<PaymentController>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        
        public PaymentController(ILogger<PaymentController> logger, IWebHostEnvironment webHostEnvironment,
            UserManager<User> userManager, IUserService userService) 
            : base(logger, webHostEnvironment)
        {
            _userManager = userManager;
            _userService = userService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetStripeCustId()
        {
            // Validate
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Modify the user's profile
            if (user != null)
            {
                // Exist
                if (!_userService.HasStripe(user.Id))
                {
                    return BadRequest("Please bootstripe first!");
                }
                
                // Obtain
                var stripeCustomerClaim = (await _userManager.GetClaimsAsync(user))
                    .SingleOrDefault(c => c.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId));

                // Check
                if (stripeCustomerClaim == null || string.IsNullOrEmpty(stripeCustomerClaim.Value))
                    return BadRequest("There was an issue while attempting to obtain your stripe id!");

                // Return
                return Ok(stripeCustomerClaim.Value);
            }

            return BadRequest("Invalid input/s, please ensure that the entries are correctly filled!");
        }
    }
}