using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Infra.Auth.Events.UserEvent;
using Nozomi.Infra.Auth.Services.User;
using Nozomi.Preprocessing.Abstracts;
using Stripe;

namespace Nozomi.Infra.Payment.Events.Bootstripe
{
    public class BootstripeEvent : BaseEvent<BootstripeEvent>, IBootstripeEvent
    {
        private readonly IOptions<StripeOptions> _stripeOptions;
        private readonly IUserEvent _userEvent;

        public BootstripeEvent(ILogger<BootstripeEvent> logger, IOptions<StripeOptions> stripeOptions,
            IUserEvent userEvent) : base(logger)
        {
            // apiKey = Secret Key
            StripeConfiguration.ApiKey = stripeOptions.Value.SecretKey;
            _stripeOptions = stripeOptions;
            
            _userEvent = userEvent;
        }

        public bool IsDefaultPlan(string planId)
        {
            const string methodName = "IsDefaultPlan";

            if (string.IsNullOrEmpty(planId))
                throw new ArgumentNullException($"{_eventName} {methodName}: Invalid plan id");

            return planId.Equals(_stripeOptions.Value.DefaultPlanId);
        }

        public async Task<string> GetUserCurrentPlanIdAsync(string userId)
        {
            const string methodName = "GetUserCurrentPlanIdAsync";
            
            // Safetynet
            if (!string.IsNullOrEmpty(userId))
            {
                // Then filter it to look for his/her subscription id just to be sure this person actually has a
                // subscription.
                var userSubId = _userEvent.GetUserActiveSubscriptionId(userId);

                // Safetynet
                if (!string.IsNullOrEmpty(userSubId) &&
                    // Make sure there is a Stripe binding no matter what.
                    !string.IsNullOrEmpty(_userEvent.GetStripeCustomerId(userId)))
                {
                    // Since a subscription to Stripe exists, locate the current subscription if any
                    var subService = new SubscriptionService();
                    var sub = await subService.GetAsync(userSubId);

                    // Make sure this subscription is actually valid, through Stripe to ensure it is really legit
                    if ((sub.EndedAt == null || sub.EndedAt < DateTime.UtcNow) && sub.Plan != null)
                        return sub.Plan.Id; // Return since fulfilled
                }
            }

            _logger.LogWarning($"{_eventName} {methodName}: Invalid user, is there even a " +
                               "subscription?");
            return string.Empty;
        }

        public async Task<IEnumerable<Plan>> GetPlans(bool activeOnly = true)
        {
            if (_stripeOptions == null || string.IsNullOrEmpty(_stripeOptions.Value.ProductId))
                throw new NullReferenceException($"{_eventName} GetPlans: Unable to load, Stripe/ProductId is " +
                                                 "not configured.");
            
            var productService = new ProductService();
            var product = productService.Get(_stripeOptions.Value.ProductId);

            if (product == null)
                throw new NullReferenceException($"{_eventName} GetPlans: Unable to load, Product is " +
                                                 "not configured.");

            var planListOptions = new PlanListOptions
            {
                Active = activeOnly,
                Product = product.Id
            };

            var planService = new PlanService();
            var plans = await planService.ListAsync(planListOptions);

            if (plans.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogWarning($"{_eventName} GetPlans: Unable to load, plans cannot be " +
                                   $"retrieved from Stripe.");
                throw new NullReferenceException($"{_eventName} GetPlans: Unable to load, plans cannot be " +
                                                 $"retrieved from Stripe.");
            }

            return plans.Data;
        }

        public Plan GetPlan(string planId)
        {
            const string methodName = "Plan";
            if (string.IsNullOrEmpty(planId))
                throw new ArgumentNullException($"{_eventName} {methodName}: Plan ID is null");

            var planService = new PlanService();
            var planListOptions = new PlanListOptions
            {
                Active = true,
                Product = _stripeOptions.Value.ProductId,
            };
            var plans = planService.List(planListOptions);

            return plans.Data.FirstOrDefault(p => p.Id.Equals(planId));
        }

        public bool PlanExists(string planId)
        {
            const string methodName = "PlanExists";

            if (string.IsNullOrEmpty(planId))
                throw new ArgumentNullException($"{_eventName} {methodName}: Plan id is null");

            var planService = new PlanService();
            var planListOptions = new PlanListOptions
            {
                Active = true,
                Product = _stripeOptions.Value.ProductId,
            };
            var plans = planService.List(planListOptions);

            return plans.Data.Any(p => p.Id.Equals(planId));
        }

        public async Task<IEnumerable<PaymentMethod>> ListPaymentMethods(User user, string paymentMethodType = "card")
        {
            const string methodName = "ListPaymentMethods";
            PerformUserPrecheck(user, methodName);

            var customerId = _userEvent.GetStripeCustomerId(user.Id);

            if (customerId.IsNullOrEmpty())
                throw new InvalidOperationException($"{_eventName} {methodName}: User is not registered for stripe");

            var options = new PaymentMethodListOptions
            {
                Customer = customerId,
                Type = paymentMethodType
            };

            var paymentMethodService = new PaymentMethodService();
            var paymentMethods = await paymentMethodService.ListAsync(options);
            if (paymentMethods.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogWarning($"{_eventName} {methodName}: Unable to load, there was a " +
                                   $"problem in attempting to retrieve the payment methods for {customerId} " +
                                   $"from Stripe.");
                throw new NullReferenceException($"{_eventName} {methodName}: Unable to load, there was a " +
                                                 $"problem in attempting to retrieve the payment methods for {customerId} " +
                                                 $"from Stripe.");
            }

            return paymentMethods.Data;
        }

        public async Task<bool> PaymentMethodExistsUnderUser(User user, string paymentMethodId)
        {
            const string methodName = "PaymentMethodExists";
            PerformUserPrecheck(user, methodName);

            var customerId = _userEvent.GetStripeCustomerId(user.Id);

            if (paymentMethodId.IsNullOrEmpty())
                throw new ArgumentNullException($"{_eventName} {methodName}: Invalid payment method id");

            var paymentMethodService = new PaymentMethodService();
            var paymentMethod = await paymentMethodService.GetAsync(paymentMethodId);

            return paymentMethod != null && paymentMethod.CustomerId.Equals(customerId);
        }

        private void PerformUserPrecheck(User user, string methodName)
        {
            if (user == null)
                throw new NullReferenceException($"{_eventName} {methodName}: User is null.");
        }
    }
}