using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Infra.Auth.Events.Stripe;
using Nozomi.Infra.Auth.Events.UserEvent;
using Nozomi.Infra.Auth.Services.QuotaClaims;
using Nozomi.Infra.Auth.Services.User;
using Nozomi.Infra.Payment.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Stripe;

namespace Nozomi.Infra.Payment.Services.SubscriptionHandling
{
    public class SubscriptionsHandlingService: BaseService<SubscriptionsHandlingService>, ISubscriptionsHandlingService
    {
        private readonly IQuotaClaimsService _quotaClaimsService;
        private readonly IUserEvent _userEvent;
        private readonly IUserService _userService;
        private readonly IStripeEvent _stripeEvent;

        private readonly SubscriptionService _subscriptionService;
        private readonly PlanService _planService;
        
        public SubscriptionsHandlingService(ILogger<SubscriptionsHandlingService> logger, IQuotaClaimsService quotaClaimsService, IUserEvent userEvent, IUserService userService, IStripeEvent stripeEvent) : base(logger)
        {
            _quotaClaimsService = quotaClaimsService;
            _userEvent = userEvent;
            _stripeEvent = stripeEvent;
            _userService = userService;
            
            _subscriptionService = new SubscriptionService();
            _planService = new PlanService();
        }

        public async Task Subscribe(string planId, User user)
        {
            const string methodName = "Subscribe";
            PerformUserPrecheck(user, methodName);

            if (!_userEvent.HasStripe(user.Id))
                throw new InvalidOperationException($"{_serviceName} {methodName}: User is not registered for stripe");

            var stripeCustomerId = _userEvent.GetStripeCustomerId(user.Id);

            var plan = await _planService.GetAsync(planId);

            if (plan == null)
                throw new StripeException($"{_serviceName} {methodName}: Plan does not exist");
            
            var quotaValue = 0;
            var quotaString = plan.Metadata["Quota"];
            if (!int.TryParse(quotaString, out quotaValue))
                throw new FormatException($"{_serviceName} {methodName}: Failed to parse plan quota to int");

            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = stripeCustomerId,
                CancelAtPeriodEnd = false,
                Items = new List<SubscriptionItemOptions> 
                {
                    new SubscriptionItemOptions
                    {
                        Plan = plan.Id
                    }
                }
            };

            var subscription = await _subscriptionService.CreateAsync(subscriptionOptions);
            if (subscription == null)
                throw new StripeException($"{_serviceName} {methodName}: An error occured while trying to create subscription for plan {planId} for {user.Id}");

            _userService.AddSubscription(user.Id, subscription.Id);
            _quotaClaimsService.SetQuota(user.Id, quotaValue);
        }
        
        public async Task ChangePlan(string planId, User user)
        {
            const string methodName = "ChangePlan";
            PerformUserPrecheck(user, methodName);

            var plan = await _planService.GetAsync(planId);
            
            if(plan == null)
                throw new StripeException($"{_serviceName} {methodName}: An error occured while retrieving plan by id {planId}");

            var activeSubscriptionId = _userEvent.GetUserActiveSubscriptionId(user.Id);
            var stripeCustomerId = _userEvent.GetStripeCustomerId(user.Id);
            
            if(activeSubscriptionId == null)
                throw new InvalidOperationException($"{_serviceName} {methodName}: Active subscription Id does not exist");

            var subscription = await _subscriptionService.GetAsync(planId);
            
            if(subscription == null)
                throw new StripeException($"{_serviceName} {methodName}: Could not retrieve subscription by id {activeSubscriptionId}");
            
            if(!subscription.Status.ToLower().Equals("active"))
                throw new InvalidOperationException($"{_serviceName} {methodName}: Subscription id {activeSubscriptionId} is not active");

            if (!subscription.CustomerId.Equals(stripeCustomerId))
                throw new InvalidOperationException($"{_serviceName} {methodName}: Subscription {activeSubscriptionId} does not belong to customer {stripeCustomerId}");

            if (!subscription.Plan.Id.Equals(planId))
                throw new InvalidOperationException($"{_serviceName} {methodName}: No need to update subscription plan");

            var subscriptionChangeOptions = new SubscriptionUpdateOptions {
                Items = new List<SubscriptionItemOptions> {
                    new SubscriptionItemOptions
                    {
                        Plan = planId
                    }
                }
            };
            
            subscription = await _subscriptionService.UpdateAsync(subscription.Id, subscriptionChangeOptions);
            
            if (!subscription.Plan.Id.Equals(planId)) {
                _logger.LogInformation($"{_serviceName} {methodName}: There was an issue " +
                                       $"changing user {user.Id} plan to {planId}");
                throw new StripeException($"{_serviceName} {methodName}: There was an " +
                                          $"issue changing user {user.Id} plan to {planId}");
            }
            
            _logger.LogInformation($"{_serviceName} ChangeSubscription: {user.Id} " +
                                   "successfully changed to new plan {subscription.Plan.Id}");
            
            var quotaValue = 0;
            var quotaString = plan.Metadata["Quota"];
            if (!int.TryParse(quotaString, out quotaValue))
                throw new FormatException($"{_serviceName} {methodName}: Failed to parse plan quota to int");
            
            _quotaClaimsService.SetQuota(user.Id, quotaValue);
            
            _logger.LogInformation($"{_serviceName} {methodName}: Updated user {user.Id} quota to {quotaValue}");
            return;
        }
        
        public async Task SubscriptionCancelled(Subscription subscription)
        {
            const string methodName = "SubscriptionCancelled";
            PerformSubscriptionPrecheck(subscription, methodName);
            
            if(!subscription.Status.ToLower().Equals("canceled "))
                throw new InvalidOperationException($"{_serviceName} {methodName}: ");

            var customerId = subscription.CustomerId;
            var user = await _stripeEvent.GetUserByCustomerId(customerId);

            if (user == null)
                throw new NullReferenceException(
                    $"{_serviceName} {methodName}: Unable to retrieve user based on customer id {customerId}");
            
            //TODO: Get default plan quota
            var quota = 5000;
            _quotaClaimsService.SetQuota(user.Id, quota);
            _userService.RemoveSubscription(user.Id);
        }

        public async Task UnsubscribeUser(User user)
        {
            const string methodName = "UnsubscribeUser";
            PerformUserPrecheck(user, methodName);

            var subscriptionId =  _userEvent.GetUserActiveSubscriptionId(user.Id);
            
            if(string.IsNullOrEmpty(subscriptionId))
                throw new InvalidOperationException($"{_serviceName} {methodName}: User has no active subscription to cancel");

            var subscription = await _subscriptionService.GetAsync(subscriptionId);

            if (_stripeEvent.IsDefaultPlan(subscription.Plan.Id))
                throw new InvalidOperationException($"{_serviceName} {methodName}: Unable to cancel subscription {subscriptionId} as it is the default plan");
            
            var cancelOptions = new SubscriptionCancelOptions
            {
                InvoiceNow = true,
                Prorate = false
            };
            
            subscription = await _subscriptionService.CancelAsync(subscriptionId, 
                cancelOptions);
            
            if(subscription.CanceledAt == null)
                throw new StripeException($"{_serviceName} {methodName}: An error occured while trying to cancel subscription {subscriptionId}");
            
            _userService.RemoveSubscription(user.Id);
        }

        private void PerformUserPrecheck(User user, string methodName)
        {
            if(user == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: User is null.");
        }
        
        private void PerformSubscriptionPrecheck(Subscription subscription, string methodName)
        {
            if(subscription == null)
                throw new ArgumentNullException($"{_serviceName} {methodName}: Subscription is null");
        }
    }
}