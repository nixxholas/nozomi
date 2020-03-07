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
using Nozomi.Infra.Auth.Events.Stripe;
using Nozomi.Infra.Auth.Events.UserEvent;
using Nozomi.Infra.Auth.Services.User;
using Nozomi.Preprocessing.Abstracts;
using Stripe;

namespace Nozomi.Infra.Payment.Events.Bootstripe
{
    public class BootstripeEvent : BaseEvent<BootstripeEvent>, IBootstripeEvent
    {
        private readonly PlanService _planService;
        private readonly IOptions<StripeOptions> _stripeOptions;
        private readonly Product _stripeProduct;
        private readonly PaymentMethodService _paymentMethodService;
        private readonly IUserEvent _userEvent;
        
        public BootstripeEvent(ILogger<BootstripeEvent> logger, IOptions<StripeOptions> stripeOptions, IUserEvent userEvent) : base(logger)
        {
            // apiKey = Secret Key
            StripeConfiguration.ApiKey = stripeOptions.Value.SecretKey;
            _stripeOptions = stripeOptions;

            var productService = new ProductService();
            _stripeProduct = productService.Get(stripeOptions.Value.ProductId);
            _paymentMethodService = new PaymentMethodService();
            _userEvent = userEvent;
        }

        public bool IsDefaultPlan(string planId)
        {
            const string methodName = "IsDefaultPlan";
            
            if (string.IsNullOrEmpty(planId))
                throw new ArgumentNullException($"{_eventName} {methodName}: Invalid plan id");
            
            return planId.Equals(_stripeOptions.Value.DefaultPlanId);
        }

        public async Task<IEnumerable<Plan>> GetPlans(bool activeOnly = true)
        {
            if(_stripeProduct == null)
                throw new NullReferenceException($"{_eventName} plans: Unable to load, Product is not configured.");
            
            var planListOptions = new PlanListOptions
            {
                Active = activeOnly,
                Product = _stripeProduct.Id
            };

            var plans = await _planService.ListAsync(planListOptions);
            
            if (plans.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogWarning($"{_eventName} plans: Unable to load, plans cannot be " +
                                   $"retrieved from Stripe.");
                throw new NullReferenceException($"{_eventName} plans: Unable to load, plans cannot be " +
                                                 $"retrieved from Stripe.");
            }
            
            return plans.Data;
        }

        public Plan GetPlan(string planId)
        {
            const string methodName = "Plan";
            if (string.IsNullOrEmpty(planId))
                throw new ArgumentNullException($"{_eventName} {methodName}: Plan ID is null");
            
            var planListOptions = new PlanListOptions
            {
                Active = true,
                Product = _stripeOptions.Value.ProductId,
            };
            var plans = _planService.List(planListOptions);

            return plans.Data.FirstOrDefault(p => p.Id.Equals(planId));
        }

        public bool PlanExists(string planId)
        {
            const string methodName = "PlanExists";
            
            if (string.IsNullOrEmpty(planId))
                throw new ArgumentNullException($"{_eventName} {methodName}: Plan id is null");
            var planListOptions = new PlanListOptions
            {
                Active = true,
                Product = _stripeOptions.Value.ProductId,
            };
            var plans = _planService.List(planListOptions);

            return plans.Data.Any(p => p.Id.Equals(planId));
        }

        public async Task<IEnumerable<PaymentMethod>> ListPaymentMethods(User user, string paymentMethodType = "card")
        {
            const string methodName = "ListPaymentMethods";
            PerformUserPrecheck(user, methodName);
            
            var customerId = _userEvent.GetStripeCustomerId(user.Id);
            
            if(customerId.IsNullOrEmpty())
                throw new InvalidOperationException($"{_eventName} {methodName}: User is not registered for stripe");
            
            var options = new PaymentMethodListOptions
            {
                Customer = customerId,
                Type = paymentMethodType
            };

            var paymentMethods = await _paymentMethodService.ListAsync(options);
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

        public bool PaymentMethodExists(string stripeUserId, string paymentMethodId)
        {
            throw new System.NotImplementedException();
        }
        
        private void PerformUserPrecheck(User user, string methodName)
        {
            if(user == null)
                throw new NullReferenceException($"{_eventName} {methodName}: User is null.");
        }
    }
}