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
        private readonly SubscriptionService _subscriptionService;
        private readonly PlanService _planService;
        private readonly CustomerService _customerService;
        private readonly PaymentMethodService _paymentMethodService;

        public StripeService(ILogger<StripeService> logger, IUnitOfWork<AuthDbContext> unitOfWork, 
            IStripeEvent stripeEvent, UserManager<Base.Auth.Models.User> userManager, 
            IOptions<StripeOptions> stripeConfiguration)
            : base(logger, unitOfWork)
        {
            _stripeConfiguration = stripeConfiguration;
            _stripeEvent = stripeEvent;
            _userManager = userManager;
            _subscriptionService = new SubscriptionService();
            _planService = new PlanService();
            _customerService = new CustomerService();
            _paymentMethodService = new PaymentMethodService();
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

        public async Task Subscribe(string planId, Base.Auth.Models.User user)
        {
            if (user != null)
            {
                if (_stripeEvent.PlanExists(planId))
                {
                    var plan = await _planService.GetAsync(planId);

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

                        var subscription = await _subscriptionService.CreateAsync(subscriptionOptions);

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
                throw new NullReferenceException($"{_serviceName} subscribePlan: plan does not exist.");
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
                    
                    // Check the plan of the subscription first
                    var subscription = _subscriptionService.Get(subscriptionIdClaim.Value);
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
                    var subCancellation = await _subscriptionService.CancelAsync(subscriptionIdClaim.Value, 
                        cancelOptions);

                    if (subCancellation.CanceledAt != null)
                    {
                        // Add it historically
                        await _userManager.AddClaimAsync(user, new Claim(
                            NozomiJwtClaimTypes.PreviousStripeSubscriptionId, subscriptionIdClaim.Value));

                        // Remove subscription from user claims
                        await _userManager.RemoveClaimAsync(user, subscriptionIdClaim);

                        // Obtain his/her stripe customer id
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
                        
                        // Prepare to bind on Stripe's end
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
                        var defaultSubRes = await _subscriptionService.CreateAsync(subCreateOptions);

                        if (defaultSubRes == null || defaultSubRes.StripeResponse.StatusCode != HttpStatusCode.OK)
                        {
                            _logger.LogInformation($"{_serviceName} Unsubscribe: There was an issue binding " +
                                                   $"the user {user.Id} back to the default plan on Stripe.");
                            throw new StripeException($"{_serviceName} Unsubscribe: There was an issue " +
                                                      $"binding the user {user.Id} back to the default plan on Stripe.");
                        }
                        
                        // Since binding on Stripe is successful, we'll bind him/her internally
                        await _userManager.AddClaimAsync(user, new Claim(
                            NozomiJwtClaimTypes.StripeSubscriptionId, defaultSubRes.Id));

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

        public async void ChangeSubscription(string planId, Base.Auth.Models.User user)
        {
            if (user != null)
            {
                if (_stripeEvent.PlanExists(planId))
                {
                    var plan = await _planService.GetAsync(planId);
                    var userClaims = await _userManager.GetClaimsAsync(user);

                    // Ensure the user has his/her stripe customer id up
                    if (!userClaims.Any() || !userClaims.Any(uc =>
                            uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId)))
                    {
                        // This shouldn't happen, but just in case
                        _logger.LogInformation($"{_serviceName} ChangeSubscription: user has yet to bind " +
                                               "to stripe");
                        throw new KeyNotFoundException($"{_serviceName} ChangeSubscription: user has yet " +
                                                       "to bind to stripe");
                    }

                    // Obtain the user's current subscription
                    var subscriptionIdUserClaim = userClaims
                        .SingleOrDefault(uc => uc.Type.Equals(NozomiJwtClaimTypes.StripeSubscriptionId));

                    if (subscriptionIdUserClaim != null)
                    {
                        var subscription = await _subscriptionService.GetAsync(subscriptionIdUserClaim.Value);

                        if (subscription != null && subscription.CanceledAt != null
                            && !subscription.Plan.Id.Equals(plan.Id))
                        {

                            var subscriptionChangeOptions = new SubscriptionUpdateOptions {
                                Items = new List<SubscriptionItemOptions> {
                                    new SubscriptionItemOptions
                                    {
                                        Plan = plan.Id
                                    }
                                }
                            };

                            subscription = await _subscriptionService.UpdateAsync(subscription.Id, 
                                subscriptionChangeOptions);
                            if (subscription.Plan.Id.Equals(plan.Id)) {
                                _logger.LogInformation($"{_serviceName} ChangeSubscription: {user.Id} " +
                                                       "successfully changed to new plan {subscription.Plan.Id}");
                                return;
                            }
                            _logger.LogInformation($"{_serviceName} ChangeSubscription: There was an issue " +
                                                   $"changing user {user.Id} plan to {plan.Id}");
                            throw new StripeException($"{_serviceName} ChangeSubscription: There was an " +
                                                      $"issue changing user {user.Id} plan to {plan.Id}");
                        }
                        
                        if (subscription != null && subscription.Plan.Id.Equals(plan.Id))
                            throw new InvalidOperationException($"{_serviceName} ChangeSubscription: " +
                                                                $"subscription {subscriptionIdUserClaim.Value} is the" +
                                                                " same plan as the one requested for.");
                        
                        throw new NullReferenceException($"{_serviceName} ChangeSubscription: subscription " +
                                                         $"{subscriptionIdUserClaim.Value} does not exist or has " +
                                                         "been cancelled");
                    }
                    throw new NullReferenceException($"{_serviceName} ChangeSubscription: user does not have " +
                                                     "a ChangeSubscription");
                }
                throw new NullReferenceException($"{_serviceName} ChangeSubscription: plan is null.");
            }
            throw new NullReferenceException($"{_serviceName} ChangeSubscription: user is null.");

        }
    }
}