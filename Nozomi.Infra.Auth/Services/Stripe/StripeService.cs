using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Infra.Auth.Events.Stripe;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Repository;
using Stripe;

namespace Nozomi.Infra.Auth.Services.Stripe
{
    public class StripeService : BaseService<StripeService, AuthDbContext>, IStripeService
    {
        private readonly IOptions<StripeOptions> _stripeConfiguration;
        private readonly UserManager<Base.Auth.Models.User> _userManager;
        private readonly IStripeEvent _stripeEvent;

        public StripeService(ILogger<StripeService> logger, IUnitOfWork<AuthDbContext> unitOfWork, 
            IStripeEvent stripeEvent, UserManager<Base.Auth.Models.User> userManager, 
            IOptions<StripeOptions> stripeConfiguration)
            : base(logger, unitOfWork)
        {
            _stripeConfiguration = stripeConfiguration;
            _stripeEvent = stripeEvent;
            _userManager = userManager;
        }

        public StripeService(IHttpContextAccessor contextAccessor, ILogger<StripeService> logger,
            UserManager<Base.Auth.Models.User> userManager, IUnitOfWork<AuthDbContext> unitOfWork, 
            IStripeEvent stripeEvent, IOptions<StripeOptions> stripeConfiguration)
            : base(contextAccessor, logger, unitOfWork)
        {
            _stripeConfiguration = stripeConfiguration;
            _stripeEvent = stripeEvent;
            _userManager = userManager;
        }

        public async Task PropagateCustomer(Base.Auth.Models.User user)
        {
            if (user != null)
            {
                // Check if he/she has stripe up or not first 
                var stripeClaims = (await _userManager.GetClaimsAsync(user))
                    .Where(c => c.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId));

                if (stripeClaims.Any())
                {
                    _logger.LogInformation($"{_serviceName} PropagateCustomer: User {user.Id} already has " +
                                           $"stripe binded.");
                    return;
                }

                var customer = new CustomerCreateOptions
                {
                    Email = user.Email,
                };
                var customerService = new CustomerService();
                var result = await customerService.CreateAsync(customer);

                if (result.StripeResponse.StatusCode == HttpStatusCode.OK)
                {
                    var claim = new Claim(NozomiJwtClaimTypes.StripeCustomerId, result.Id);
                    var addClaimRes = await _userManager.AddClaimAsync(user, claim);

                    if (!addClaimRes.Succeeded)
                    {
                        _logger.LogInformation($"{_serviceName} PropagateCustomer: There was an issue " +
                                               $"pushing the stripe customer id of {result.Id} to user claims of " +
                                               $"user {user.Id}.");
                        throw new WebException($"{_serviceName} PropagateCustomer: There was an issue " +
                                                             $"pushing the stripe customer id of {result.Id} to user claims of " +
                                                             $"user {user.Id}.");
                    }

                    return; // ok!
                }

                _logger.LogInformation($"{_serviceName} PropagateCustomer: There was an issue " +
                                       $"propagating the stripe id for user {user.Id}.");
                throw new StripeException($"{_serviceName} PropagateCustomer: There was an issue " +
                                          $"propagating the stripe id for user {user.Id}.");
            }

            _logger.LogInformation($"{_serviceName} PropagateCustomer: Invalid user.");
            throw new InvalidConstraintException($"{_serviceName} PropagateCustomer: Invalid user.");
        }

        public async Task AddPaymentMethod(string paymentMethodId, Base.Auth.Models.User user)
        {
            if (!string.IsNullOrEmpty(paymentMethodId) && user != null)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);

                // Ensure the user has his/her stripe customer id up
                if (!userClaims.Any() || !userClaims.Any(uc =>
                        uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId)))
                {
                    // This shouldn't happen, but just in case
                    _logger.LogInformation($"{_serviceName} AddPaymentMethod: user has yet to bind to stripe");
                    throw new KeyNotFoundException($"{_serviceName} AddPaymentMethod: user has yet to bind to stripe");
                }

                // Ensure the user has yet to bind this payment method.
                if (userClaims.Any(uc => uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerPaymentMethodId)
                                         && uc.Value.Equals(paymentMethodId)))
                {
                    // This shouldn't happen, but just in case
                    _logger.LogInformation($"{_serviceName} AddPaymentMethod: user already has the " +
                                           $"payment method...");
                    throw new KeyNotFoundException($"{_serviceName} AddPaymentMethod: user has completed " +
                                                   $"the binding earlier.");
                }

                // Obtain the user's stripe id
                var userStripeCustId = userClaims
                    .SingleOrDefault(uc => uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId))?
                    .Value;

                // Safetynet
                if (string.IsNullOrEmpty(userStripeCustId))
                {
                    // This shouldn't happen, but just in case
                    _logger.LogInformation($"{_serviceName} AddPaymentMethod: user has yet to bind to stripe");
                    throw new KeyNotFoundException($"{_serviceName} AddPaymentMethod: user has yet to bind " +
                                                   $"to stripe");
                }
                
                // Bind it!
                var options = new PaymentMethodAttachOptions
                {
                    Customer = userStripeCustId,
                };
                var paymentMethodService = new PaymentMethodService();
                var paymentMethod = paymentMethodService.Attach(paymentMethodId, options);

                // If the method ain't null, bind it
                if (paymentMethod != null && paymentMethod.StripeResponse != null 
                                          && paymentMethod.StripeResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var paymentMethodUserClaim = new Claim(NozomiJwtClaimTypes.StripeCustomerPaymentMethodId, 
                        paymentMethod.Id);
                    await _userManager.AddClaimAsync(user, paymentMethodUserClaim);

                    _logger.LogInformation($"{_serviceName} AddPaymentMethod: {user.Id} successfully added " +
                                           $"a new payment method tokenized as {paymentMethodUserClaim.Value}");
                    return; // Done!
                }

                _logger.LogInformation($"{_serviceName} AddPaymentMethod: There was an issue related to binding " +
                                       $"paymentMethodId {paymentMethodId} to user {user.Id}.");
                throw new StripeException($"{_serviceName} AddPaymentMethod: There was a problem binding " +
                                          $"the newly created payment method of {paymentMethodId} to {user.Id}");
            }

            _logger.LogInformation($"{_serviceName} AddPaymentMethod: Invalid tokenId or userId.");
            throw new InvalidConstraintException($"{_serviceName} AddPaymentMethod: Invalid tokenId or userId.");
        }

        public async Task RemovePaymentMethod(string paymentMethodId, Base.Auth.Models.User user)
        {
            if (!string.IsNullOrEmpty(paymentMethodId) && user != null)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);

                // Ensure the user has his/her stripe customer id up
                if (!userClaims.Any() || !userClaims.Any(uc =>
                        uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId)))
                {
                    // This shouldn't happen, but just in case
                    _logger.LogInformation($"{_serviceName} RemovePaymentMethod: user has yet to bind " +
                                           $"to stripe");
                    throw new KeyNotFoundException($"{_serviceName} RemovePaymentMethod: user has yet to " +
                                                   $"bind to stripe");
                }

                // Ensure the user is binded to this payment method
                if (!userClaims.Any(uc => uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerPaymentMethodId)
                                          && uc.Value.Equals(paymentMethodId)))
                {
                    // This shouldn't happen, but just in case
                    _logger.LogInformation($"{_serviceName} RemovePaymentMethod: user already has the " +
                                           $"payment method...");
                    throw new KeyNotFoundException($"{_serviceName} RemovePaymentMethod: user has " +
                                                   $"completed binding earlier.");
                }

                // Obtain the user's stripe customer id
                var userStripeCustId = userClaims
                    .SingleOrDefault(uc => uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId))?
                    .Value;

                // Safetynet
                if (string.IsNullOrEmpty(userStripeCustId))
                {
                    // This shouldn't happen, but just in case
                    _logger.LogInformation($"{_serviceName} RemovePaymentMethod: user has yet to bind to " +
                                           $"stripe");
                    throw new KeyNotFoundException($"{_serviceName} RemovePaymentMethod: user has yet to " +
                                                   $"bind to stripe");
                }

                // Obtain the payment method claim directly
                var stripePaymentMethodClaim = userClaims.SingleOrDefault(uc =>
                    uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerPaymentMethodId) 
                    && uc.Value.Equals(paymentMethodId));

                if (stripePaymentMethodClaim == null) // This shouldn't happen, but just in case
                {
                    _logger.LogInformation($"{_serviceName} RemovePaymentMethod: user doesn't own the " +
                                           $"payment method what a bitch");
                    throw new KeyNotFoundException($"{_serviceName} RemovePaymentMethod: user does not own " +
                                                   $"the payment method");
                }
                
                var paymentMethodService = new PaymentMethodService();
                var paymentMethod = paymentMethodService.Detach(stripePaymentMethodClaim.Value);

                // If the card's deleted, delete it on our end
                if (paymentMethod != null && paymentMethod.Customer == null)
                {
                    await _userManager.RemoveClaimAsync(user, stripePaymentMethodClaim);

                    _logger.LogInformation($"{_serviceName} RemovePaymentMethod: {user.Id} successfully " +
                                           $"removed a payment method that was tokenized as {stripePaymentMethodClaim.Value}");
                    return; // Done!
                }

                _logger.LogInformation($"{_serviceName} RemovePaymentMethod: There was an issue deleting " +
                                       $"the payment method {paymentMethodId} from user {user.Id}");
                throw new StripeException($"{_serviceName} RemovePaymentMethod: There was an issue deleting " +
                                               $"{paymentMethodId} from user {user.Id}");
            }

            _logger.LogInformation($"{_serviceName} removeCard: Invalid cardId or userId.");
            throw new InvalidConstraintException($"{_serviceName} removeCard: Invalid cardId or userId.");
        }

        public async Task Subscribe(Plan plan, Base.Auth.Models.User user)
        {
            if (user != null)
            {
                if (plan != null)
                {

                    var userClaims = await _userManager.GetClaimsAsync(user);

                    // Ensure the user has his/her stripe customer id up
                    if (!userClaims.Any() || !userClaims.Any(uc =>
                            uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId)))
                    {
                        // This shouldn't happen, but just in case
                        _logger.LogInformation($"{_serviceName} addsubscribePlanCard: user has yet to bind to stripe");
                        throw new KeyNotFoundException($"{_serviceName} subscribePlan: user has yet to bind to stripe");
                    }

                    var customerIdClaim = userClaims.SingleOrDefault(uc => uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId));
                    if (customerIdClaim != null)
                    {
                        var subscriptionOptions = new SubscriptionCreateOptions
                        {
                            Customer = customerIdClaim.Value,
                            CancelAtPeriodEnd = false,
                            Items = new List<SubscriptionItemOptions> {
                                new SubscriptionItemOptions
                                {
                                    Plan = plan.Id
                                }
                            }
                        };

                        var subscriptionService = new SubscriptionService();
                        var subscription = await subscriptionService.CreateAsync(subscriptionOptions);

                        // If the subscription ain't null, bind it
                        if (subscription != null)
                        {
                            var subscriptionUserClaim = new Claim(NozomiJwtClaimTypes.StripeSubscriptionId, subscription.Id);
                            await _userManager.AddClaimAsync(user, subscriptionUserClaim);

                            _logger.LogInformation($"{_serviceName} subscribePlan: {user.Id} successfully subscribed to new plan " +
                                                    $" tokenized as {subscriptionUserClaim.Value}");
                            return; // Done!
                        }

                        _logger.LogInformation($"{_serviceName} subscribePlan: There was an issue subscribing " +
                                    $"user {user.Id} to {plan.Id}");
                        throw new StripeException($"{_serviceName} subscribePlan: There was an issue subscribing " +
                                                        $"user {user.Id} to {plan.Id}");
                    }
                    throw new NullReferenceException($"{_serviceName} subscribePlan: user has yet to bind to stripe");
                }
                throw new NullReferenceException($"{_serviceName} subscribePlan: plan is null.");
            }
            throw new NullReferenceException($"{_serviceName} subscribePlan: user is null.");
        }

        public async Task Unsubscribe(Base.Auth.Models.User user)
        {
            if (user != null)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);

                // Ensure the user has his/her stripe customer id up
                if (!userClaims.Any() || !userClaims.Any(uc =>
                        uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId)))
                {
                    // This shouldn't happen, but just in case
                    _logger.LogInformation($"{_serviceName} Unsubscribe: user has yet to bind to stripe");
                    throw new KeyNotFoundException($"{_serviceName} Unsubscribe: user has yet to bind to stripe");
                }

                var subscriptionIdClaim = userClaims
                    .SingleOrDefault(uc => uc.Type.Equals(NozomiJwtClaimTypes.StripeSubscriptionId));
                if (subscriptionIdClaim != null)
                {
                    var subscriptionService = new SubscriptionService();
                    
                    // Check the plan of the subscription first
                    var subscription = subscriptionService.Get(subscriptionIdClaim.Value);
                    if (_stripeEvent.IsDefaultPlan(subscription.Plan.Id))
                    {
                        _logger.LogInformation($"{_serviceName} Unsubscribe: There was an issue cancelling " +
                                               $"the subscription {subscriptionIdClaim.Value} of user {user.Id} " +
                                               "because its the default plan.");
                        throw new StripeException($"{_serviceName} Unsubscribe: There was an issue cancelling " +
                                                  $"the subscription {subscriptionIdClaim.Value} of user {user.Id} " +
                                                  "because its the default plan.");
                    }
                    
                    // Since it's not the default, cancel it
                    var cancelOptions = new SubscriptionCancelOptions
                    {
                        InvoiceNow = true,
                        Prorate = false
                    };
                    var subCancellation = await subscriptionService.CancelAsync(subscriptionIdClaim.Value, 
                        cancelOptions);

                    if (subCancellation.CanceledAt != null)
                    {
                        // Add it historically
                        await _userManager.AddClaimAsync(user, new Claim(
                            NozomiJwtClaimTypes.PreviousStripeSubscriptionId, subscriptionIdClaim.Value));

                        // Remove subscription from user claims
                        await _userManager.RemoveClaimAsync(user, subscriptionIdClaim);

                        var userStripeClaim = userClaims.SingleOrDefault(uc =>
                            uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId));
                        if (userStripeClaim == null)
                        {
                            _logger.LogInformation($"{_serviceName} Unsubscribe: There was an issue binding " +
                                                   $"the user {user.Id} back to the default plan in his/her claims.");
                            throw new StripeException($"{_serviceName} Unsubscribe: There was an issue " +
                                                      $"binding the user {user.Id} back to the default plan in " +
                                                      "his/her claims.");
                        }
                        
                        var subCreateOptions = new SubscriptionCreateOptions
                        {
                            Customer = userStripeClaim.Value,
                            CancelAtPeriodEnd = false,
                            Items = new List<SubscriptionItemOptions>
                            {
                                new SubscriptionItemOptions
                                {
                                    Plan = _stripeConfiguration.Value.DefaultPlanId
                                }
                            }
                        };
                        var defaultSubRes = await subscriptionService.CreateAsync(subCreateOptions);

                        if (defaultSubRes == null || defaultSubRes.StripeResponse.StatusCode != HttpStatusCode.OK)
                        {
                            _logger.LogInformation($"{_serviceName} Unsubscribe: There was an issue binding " +
                                                   $"the user {user.Id} back to the default plan on Stripe.");
                            throw new StripeException($"{_serviceName} Unsubscribe: There was an issue " +
                                                      $"binding the user {user.Id} back to the default plan on Stripe.");
                        }

                        _logger.LogInformation($"{_serviceName} Unsubscribe: {user.Id} successfully " +
                                               "cancelled a plan subscription that was tokenized as " +
                                               $"{subscriptionIdClaim.Value}");
                        
                        return;
                    }
                    
                    _logger.LogInformation($"{_serviceName} Unsubscribe: There was an issue cancelling " +
                                    $"subscription {subscriptionIdClaim.Value} of user {user.Id}");
                    throw new StripeException($"{_serviceName} Unsubscribe: There was an issue cancelling " +
                                                    $"subscription {subscriptionIdClaim.Value} of user {user.Id}");
                }
                // This shouldn't happen, but just in case
                _logger.LogInformation($"{_serviceName} Unsubscribe: user is not subscribed to a plan");
                throw new KeyNotFoundException($"{_serviceName} Unsubscribe: user is not subscribed to a plan");
            }

            throw new NullReferenceException($"{_serviceName} Unsubscribe: user is null.");
        }

        public async void ChangeSubscription(Plan plan, Base.Auth.Models.User user)
        {
            if (user != null)
            {
                if (plan != null)
                {
                    var userClaims = await _userManager.GetClaimsAsync(user);

                    // Ensure the user has his/her stripe customer id up
                    if (!userClaims.Any() || !userClaims.Any(uc =>
                            uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId)))
                    {
                        // This shouldn't happen, but just in case
                        _logger.LogInformation($"{_serviceName} changePlan: user has yet to bind to stripe");
                        throw new KeyNotFoundException($"{_serviceName} changePlan: user has yet to bind to stripe");
                    }

                    var subscriptionIdUserClaim = userClaims.SingleOrDefault(uc => uc.Type.Equals(NozomiJwtClaimTypes.StripeSubscriptionId));

                    if (subscriptionIdUserClaim != null)
                    {
                        var subscriptionService = new SubscriptionService();
                        var subscription = await subscriptionService.GetAsync(subscriptionIdUserClaim.Value);

                        if (subscription != null && subscription.CanceledAt != null)
                        {

                            var subscriptionChangeOptions = new SubscriptionUpdateOptions {
                                Items = new List<SubscriptionItemOptions> {
                                    new SubscriptionItemOptions
                                    {
                                        Plan = plan.Id
                                    }
                                }
                            };

                            subscription = await subscriptionService.UpdateAsync(subscription.Id, subscriptionChangeOptions);
                            if (subscription.Plan.Id.Equals(plan.Id)) {
                                _logger.LogInformation($"{_serviceName} changePlan: {user.Id} successfully changed to new plan {subscription.Plan.Id}");
                                return;
                            }
                            _logger.LogInformation($"{_serviceName} changePlan: There was an issue changing " +
                                    $"user {user.Id} plan to {plan.Id}");
                            throw new StripeException($"{_serviceName} changePlan: There was an issue changing " +
                                                            $"user {user.Id} plan to {plan.Id}");
                        }
                        throw new NullReferenceException($"{_serviceName} changePlan: subscription {subscriptionIdUserClaim.Value} does not exist or has been cancelled");
                    }
                    throw new NullReferenceException($"{_serviceName} changePlan: user does not have a subscription");
                }
                throw new NullReferenceException($"{_serviceName} changePlan: plan is null.");
            }
            throw new NullReferenceException($"{_serviceName} changePlan: user is null.");

        }
    }
}