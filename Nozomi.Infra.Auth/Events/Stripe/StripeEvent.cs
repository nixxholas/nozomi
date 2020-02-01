using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.Auth.Global;
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
        private readonly UserManager<Base.Auth.Models.User> _userManager;

        public StripeEvent(ILogger<StripeEvent> logger, IUnitOfWork<AuthDbContext> unitOfWork, UserManager<Base.Auth.Models.User> userManager,
            IOptions<StripeOptions> stripeConfiguration) 
            : base(logger, unitOfWork)
        {
            // apiKey = Secret Key
            StripeConfiguration.ApiKey = stripeConfiguration.Value.SecretKey;

            var productService = new ProductService();
            _stripeProduct = productService.Get(stripeConfiguration.Value.ProductId);
            _userManager = userManager;
        }

        public async Task<IEnumerable<Card>> Cards(Base.Auth.Models.User user)
        {
            if (user != null) {
                var userClaims = await _userManager.GetClaimsAsync(user);

                // Ensure the user has his/her stripe customer id up
                if (!userClaims.Any() || !userClaims.Any(uc =>
                        uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId)))
                {
                    // This shouldn't happen, but just in case
                    _logger.LogInformation($"{_eventName} addCard: user has yet to bind to stripe");
                    throw new KeyNotFoundException($"{_eventName} addCard: user has yet to bind to stripe");
                }

                return userClaims.Where(uc => uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerCardId)).Select(uc => new Card { Id = uc.Value });
            }
            throw new NullReferenceException($"{_eventName} cards: Unable to get cards, user is not null.");
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