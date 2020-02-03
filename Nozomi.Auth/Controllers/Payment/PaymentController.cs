using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.Auth.ViewModels.Payment;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Infra.Auth.Events.Stripe;
using Nozomi.Infra.Auth.Services.Stripe;
using Nozomi.Infra.Auth.Services.User;
using Stripe;

namespace Nozomi.Auth.Controllers.Payment
{
    public class PaymentController : BaseController<PaymentController>
    {
        private readonly IOptions<StripeOptions> _stripeOptions;
        private readonly UserManager<User> _userManager;
        private readonly IStripeEvent _stripeEvent;
        private readonly IStripeService _stripeService;
        private readonly IUserService _userService;
        
        public PaymentController(ILogger<PaymentController> logger, IWebHostEnvironment webHostEnvironment,
            IOptions<StripeOptions> stripeOptions,
            UserManager<User> userManager, IStripeEvent stripeEvent, IStripeService stripeService, 
            IUserService userService) 
            : base(logger, webHostEnvironment)
        {
            _stripeOptions = stripeOptions;
            _userManager = userManager;
            _stripeEvent = stripeEvent;
            _stripeService = stripeService;
            _userService = userService;
        }
        
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> StripeSetupIntent()
        {
            // Validate
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Safetynet
            if (user != null)
            {
                StripeConfiguration.ApiKey = _stripeOptions.Value.SecretKey;
                var options = new SetupIntentCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                    {
                        "card",
                    },
                };
                var service = new SetupIntentService();
                var setupIntent = service.Create(options);

                // Return the payload
                if (setupIntent != null)
                    return Ok(setupIntent);
            }

            return BadRequest("There was an error attempting to obtain a setup intent!");
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetStripePubKey()
        {
            // Validate
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Safetynet
            if (user != null)
            {
                // Return only the publishable key
                return Ok(_stripeOptions.Value.PublishableKey);
            }

            return BadRequest("Invalid input/s, please ensure that the entries are correctly filled!");
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
                    return Ok(null); // Just return nothing since he/she exists
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
        
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> AddPaymentMethod([FromBody]AddPaymentMethodInputModel vm)
        {
            // Validate
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Ensure stripe binding
            if (user != null && vm != null)
            {
                var stripeUserClaim = (await _userManager.GetClaimsAsync(user))
                    .FirstOrDefault(c => c.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId));

                if (stripeUserClaim == null)
                    return BadRequest("Please bootstripe first!");
                
                // Process card addition
                if (!string.IsNullOrEmpty(vm.PaymentMethodId)
                    && !_stripeEvent.PaymentMethodExists(stripeUserClaim.Value, vm.PaymentMethodId))
                {
                    await _stripeService.AddPaymentMethod(vm.PaymentMethodId, user);
                
                    // Return
                    _logger.LogInformation($"AddCard: card of ID {vm.PaymentMethodId} added to {user.Id}");
                    return Ok("Card successfully added!");
                }
            }

            return BadRequest("Invalid card token!");
        }
        
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> ListPaymentMethods()
        {
            // Validate
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Ensure stripe binding
            if (user != null)
            {
                var stripeUserClaim = (await _userManager.GetClaimsAsync(user))
                    .FirstOrDefault(c => c.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId));

                if (stripeUserClaim == null)
                    return BadRequest("Please bootstripe first!");
                
                // Obtain the list of payment methods via Stripe
                var options = new PaymentMethodListOptions
                {
                    Customer = stripeUserClaim.Value
                };
                var paymentMethodService = new PaymentMethodService();
                var paymentMethods = paymentMethodService.List(options);
                
                // Process card addition
                if (paymentMethods != null && paymentMethods.StripeResponse.StatusCode.Equals(HttpStatusCode.OK)
                                           && paymentMethods.Data != null)
                    return Ok(paymentMethods.Data);
            }

            return BadRequest("Invalid user!");
        }
        
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemovePaymentMethod(string id)
        {
            // Validate
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Ensure stripe binding
            if (user != null && !string.IsNullOrEmpty(id))
            {
                var stripeUserClaim = (await _userManager.GetClaimsAsync(user))
                    .FirstOrDefault(c => c.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId));

                if (stripeUserClaim == null)
                    return BadRequest("Please bootstripe and add a card first!");
                
                // Process card addition
                if (!string.IsNullOrEmpty(id)
                    && _stripeEvent.PaymentMethodExists(stripeUserClaim.Value, id))
                {
                    _stripeService.RemovePaymentMethod(id, user);
                
                    // Return
                    _logger.LogInformation($"RemovePaymentMethod: card of ID {id} added to {user.Id}");
                    return Ok("Card successfully removed!");
                }
            }

            return BadRequest("Invalid card token!");
        }
    }
}