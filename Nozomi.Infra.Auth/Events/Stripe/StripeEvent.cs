using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Repository;
using Stripe;

namespace Nozomi.Infra.Auth.Events.Stripe
{
    public class StripeEvent : BaseEvent<StripeEvent, AuthDbContext>, IStripeEvent
    {
        private readonly Product _stripeProduct;
        
        public StripeEvent(ILogger<StripeEvent> logger, IUnitOfWork<AuthDbContext> unitOfWork,
            IOptions<StripeOptions> stripeConfiguration) 
            : base(logger, unitOfWork)
        {
            // apiKey = Secret Key
            StripeConfiguration.ApiKey = stripeConfiguration.Value.SecretKey;

            var productService = new ProductService();
            _stripeProduct = productService.Get(stripeConfiguration.Value.ProductId);
        }

        public async Task<IEnumerable<Plan>> Plans(bool activeOnly = true)
        {
            if (_stripeProduct != null)
            {
                var planService = new PlanService();
                var planListOptions = new PlanListOptions
                {
                    Active = activeOnly,
                    Product = _stripeProduct.Id
                };
                
                var plans = await planService.ListAsync(planListOptions);
                if (plans.StripeResponse.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogWarning($"{_eventName} plans: Unable to load, plans cannot be " +
                                       $"retrieved from Stripe.");
                    throw new NullReferenceException($"{_eventName} plans: Unable to load, plans cannot be " +
                                                     $"retrieved from Stripe.");
                }

                return plans.Data;
            }

            throw new NullReferenceException($"{_eventName} plans: Unable to load, Product is not configured.");
        }
    }
}