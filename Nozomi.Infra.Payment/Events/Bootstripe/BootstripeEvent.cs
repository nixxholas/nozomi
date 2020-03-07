using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Preprocessing.Abstracts;
using Stripe;

namespace Nozomi.Infra.Payment.Events.Bootstripe
{
    public class BootstripeEvent : BaseEvent<BootstripeEvent>, IBootstripeEvent
    {
        private readonly PlanService _planService;
        private readonly IOptions<StripeOptions> _stripeOptions;
        private readonly Product _stripeProduct;
        
        public BootstripeEvent(ILogger<BootstripeEvent> logger, IOptions<StripeOptions> stripeOptions) : base(logger)
        {
            // apiKey = Secret Key
            StripeConfiguration.ApiKey = stripeOptions.Value.SecretKey;
            _stripeOptions = stripeOptions;
            
            var productService = new ProductService();
            _stripeProduct = productService.Get(stripeOptions.Value.ProductId);
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

        public Task<IEnumerable<Card>> Cards(User user)
        {
            throw new System.NotImplementedException();
        }

        public bool CardExists(string stripeUserId, string cardId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<PaymentMethod>> ListPaymentMethods(string stripeUserId, string paymentMethodType = "card")
        {
            throw new System.NotImplementedException();
        }

        public bool PaymentMethodExists(string stripeUserId, string paymentMethodId)
        {
            throw new System.NotImplementedException();
        }
    }
}