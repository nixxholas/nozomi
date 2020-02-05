using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
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
        private readonly string _productId;
        private readonly UserManager<Base.Auth.Models.User> _userManager;

        public StripeEvent(ILogger<StripeEvent> logger, IUnitOfWork<AuthDbContext> unitOfWork, UserManager<Base.Auth.Models.User> userManager,
            IOptions<StripeOptions> stripeConfiguration) 
            : base(logger, unitOfWork)
        {
            // apiKey = Secret Key
            StripeConfiguration.ApiKey = stripeConfiguration.Value.SecretKey;
            _productId = stripeConfiguration.Value.ProductId;
            
            var productService = new ProductService();
            _stripeProduct = productService.Get(stripeConfiguration.Value.ProductId);
            _userManager = userManager;
        }

        public Plan Plan(string planId)
        {
            if (string.IsNullOrEmpty(planId))
                return null; // Invalid plan id
            
            var planService = new PlanService();
            var planListOptions = new PlanListOptions
            {
                Active = true,
                Product = _productId,
            };
            var plans = planService.List(planListOptions);

            return plans.Data.FirstOrDefault(p => p.Id.Equals(planId));
        }

        public bool PlanExists(string planId)
        {
            if (string.IsNullOrEmpty(planId))
                return false; // Invalid plan id...
            
            var planService = new PlanService();
            var planListOptions = new PlanListOptions
            {
                Active = true,
                Product = _productId,
            };
            var plans = planService.List(planListOptions);

            return plans.Data.Any(p => p.Id.Equals(planId));
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

                return userClaims.Where(uc => uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerPaymentMethodId)).Select(uc => new Card { Id = uc.Value });
            }
            throw new NullReferenceException($"{_eventName} cards: Unable to get cards, user is not null.");
        }

        public bool CardExists(string stripeUserId, string cardId)
        {
            if (!string.IsNullOrEmpty(cardId))
            {
                var cardService = new CardService();
                // Let stripe handle the card ownership checks
                var card = cardService.Get(stripeUserId, cardId);

                // TODO: Expiration checks, not sure if Stripe covers this.
                if (card != null && card.Deleted == null)
                {
                    return true;
                }
                
                _logger.LogWarning($"{_eventName} CardExists: StripeUserId {stripeUserId} is attempting " +
                                   $"to access a card he/she does not own {cardId}.");
                return false;
            }
            
            _logger.LogWarning($"{_eventName} CardExists: Null card id.");
            return false;
        }

        public async Task<IEnumerable<PaymentMethod>> ListPaymentMethods(string stripeUserId, 
            string paymentMethodType = "card")
        {
            if (!string.IsNullOrEmpty(stripeUserId))
            {
                // Obtain the list of payment methods via Stripe
                var options = new PaymentMethodListOptions
                {
                    Customer = stripeUserId,
                    Type = paymentMethodType
                };
                var paymentMethodService = new PaymentMethodService();
                var paymentMethods = await paymentMethodService.ListAsync(options);

                if (paymentMethods.StripeResponse.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogWarning($"{_eventName} ListPaymentMethods: Unable to load, there was a " +
                                       $"problem in attempting to retrieve the payment methods for {stripeUserId} " +
                                       $"from Stripe.");
                    throw new NullReferenceException($"{_eventName} ListPaymentMethods: Unable to load, there was a " +
                                                     $"problem in attempting to retrieve the payment methods for {stripeUserId} " +
                                                     $"from Stripe.");
                }

                return paymentMethods.Data;
            }

            throw new NullReferenceException($"{_eventName} ListPaymentMethods: stripeUserId parameter is null.");
        }

        public bool PaymentMethodExists(string stripeUserId, string paymentMethodId)
        {
            if (!string.IsNullOrEmpty(paymentMethodId))
            {
                var paymentMethodService = new PaymentMethodService();
                // Let stripe handle the card ownership checks
                var paymentMethod = paymentMethodService.Get(paymentMethodId);

                if (paymentMethod != null && !string.IsNullOrEmpty(paymentMethod.CustomerId) 
                                          && paymentMethod.CustomerId.Equals(stripeUserId))
                {
                    return true;
                }
                
                _logger.LogInformation($"{_eventName} PaymentMethodExists: StripeUserId {stripeUserId} is attempting " +
                                   $"to access a card he/she does not own {paymentMethodId}.");
                return false;
            }
            
            _logger.LogWarning($"{_eventName} PaymentMethodExists: Null payment method id.");
            return false;
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