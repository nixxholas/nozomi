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
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.Auth.ViewModels.Payment;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Infra.Auth.Events.UserEvent;
using Nozomi.Infra.Auth.Services.User;
using Nozomi.Infra.Payment.Events.Bootstripe;
using Nozomi.Infra.Payment.Services.Bootstripe;
using Nozomi.Infra.Payment.Services.SubscriptionHandling;
using Stripe;

namespace Nozomi.Auth.Controllers.Payment
{
    [SecurityHeaders]
    public class PaymentController : BaseController<PaymentController>
    {
        private readonly IOptions<StripeOptions> _stripeOptions;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IUserEvent _userEvent;
        private readonly IBootstripeService _bootstripeService;
        private readonly ISubscriptionsHandlingService _subscriptionsHandlingService;
        private readonly IBootstripeEvent _bootstripeEvent;
        
        public PaymentController(ILogger<PaymentController> logger, IWebHostEnvironment webHostEnvironment,
            IOptions<StripeOptions> stripeOptions, UserManager<User> userManager,
            IBootstripeService bootstripeService, IBootstripeEvent bootstripeEvent,
            ISubscriptionsHandlingService subscriptionsHandlingService,
            IUserService userService, IUserEvent userEvent) 
            : base(logger, webHostEnvironment)
        {
            _stripeOptions = stripeOptions;
            _userManager = userManager;
            _userService = userService;
            _userEvent = userEvent;
            _bootstripeService = bootstripeService;
            _bootstripeEvent = bootstripeEvent;
            _subscriptionsHandlingService = subscriptionsHandlingService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> CurrentPlan()
        {
            // Validate
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Safetynet
            if (user == null)
                return BadRequest("Please reauthenticate again!");
                
            return Ok(await _bootstripeEvent.GetUserCurrentPlanIdAsync(user.Id));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Plans()
        {
            return Ok(await _bootstripeEvent.GetPlans());
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

            if (user == null || vm == null)
                return BadRequest("Invalid card token!");

            if (!_userEvent.HasStripe(user.Id))
                return BadRequest("User does not have stripe enabled");

            if (string.IsNullOrEmpty(vm.PaymentMethodId))
                return BadRequest("Invalid card token!");

            if (_userEvent.HasPaymentMethod(user.Id, vm.PaymentMethodId))
                return BadRequest("Payment method already added");

            if (!await _bootstripeEvent.PaymentMethodExists(vm.PaymentMethodId))
                return BadRequest("Payment method does not exist!");
            
            await _bootstripeService.AddPaymentMethod(vm.PaymentMethodId, user);
            
            // Return
            _logger.LogInformation($"AddCard: card of ID {vm.PaymentMethodId} added to {user.Id}");
            return Ok("Card successfully added!");
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

                return Ok(await _bootstripeEvent.ListPaymentMethods(user));
            }

            return BadRequest("Invalid user!");
        }
        
        // DELETE: /Payment/RemovePaymentMethod/{id}
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
                
                // Obtain the user's payment methods
                var paymentMethods = await _bootstripeEvent.ListPaymentMethods(user);
                
                // Ensure he/she has more than one payment method currently
                if (paymentMethods != null && paymentMethods.Count() > 1 
                                           // Ensure the user owns this payment method
                                           && paymentMethods.Any(pm => pm.Id.Equals(id)))
                {
                    // Process card removal
                    if (!string.IsNullOrEmpty(id)
                        && await _bootstripeEvent.PaymentMethodBelongsToUser(user, id))
                    {
                        await _bootstripeService.RemovePaymentMethod(id, user);
                
                        // Return
                        _logger.LogInformation($"RemovePaymentMethod: card of ID {id} removed from {user.Id}");
                        return Ok("Card successfully removed!");
                    }
                }
                
                // Log failure
                _logger.LogInformation("RemovePaymentMethod: An attempt to remove was made for card of " +
                                       $"ID {id} by {user.Id}.");
                return BadRequest("You can't delete this!");
            }

            return BadRequest("Invalid card token!");
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeSubscription(string id)
        {
            // Validate
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Safetynet
            if (user != null && !string.IsNullOrEmpty(id)
                             // Ensure the plan in question exists and is enabled
                             && _bootstripeEvent.PlanExists(id))
            {
                // Since the user has no existing subscriptions, proceed.
                await _subscriptionsHandlingService.ChangePlan(id, user);
                
                // Return
                _logger.LogInformation($"Subscribe: plan of ID {id} added to {user.Id}");
                return Ok("Plan has successfully been subscribed!");
            }

            return BadRequest("Invalid plan!");
        }

        /// <summary>
        /// Plan Subscription API
        ///
        /// Enables the caller to subscribe to a specified plan if he/she is unsubscribed.
        /// </summary>
        /// <param name="id">the ID of the plan</param>
        /// <returns>HttpResult of the subscription execution.</returns>
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("{id}")]
        public async Task<IActionResult> Subscribe(string id)
        {
            // Validate
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Safetynet
            if (user != null && !string.IsNullOrEmpty(id)
                             // Ensure the plan in question exists and is enabled
                             && _bootstripeEvent.PlanExists(id))
            {
                // Since the user has no existing subscriptions, proceed.
                await _subscriptionsHandlingService.Subscribe(id, user);
                
                // Return
                _logger.LogInformation($"Subscribe: plan of ID {id} added to {user.Id}");
                return Ok("Plan has successfully been subscribed!");
            }

            return BadRequest("Invalid plan!");
        }

        /// <summary>
        /// Plan Unsubscription API
        ///
        /// Enables the caller to subscribe to a specified plan if he/she is unsubscribed.
        /// </summary>
        /// <param name="id">the ID of the subscription</param>
        /// <returns>HttpResult of the subscription deletion.</returns>
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> Unsubscribe()
        {
            // Validate
            var user = await _userManager.FindByIdAsync(((ClaimsIdentity) User.Identity)
                .Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject)
                                            || c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);
            
            // Safetynet
            if (user != null)
            {
                // Since the user has no existing subscriptions, proceed.
                await _subscriptionsHandlingService.UnsubscribeUser(user);
                
                // Return
                _logger.LogInformation($"Unsubscribe: Subscription removed from {user.Id}");
                return Ok("Plan has successfully been unsubscribed!");
            }

            return BadRequest("Invalid plan!");
        }
    }
}