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
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Repository;
using Stripe;

namespace Nozomi.Infra.Auth.Services.Stripe
{
    public class StripeService : BaseService<StripeService, AuthDbContext>, IStripeService
    {
        private readonly SubscriptionService _subscriptionService;
        private readonly UserManager<Base.Auth.Models.User> _userManager;

        public StripeService(ILogger<StripeService> logger, IUnitOfWork<AuthDbContext> unitOfWork, SubscriptionService subscriptionService,
            UserManager<Base.Auth.Models.User> userManager)
            : base(logger, unitOfWork)
        {
            _userManager = userManager;
        }

        public StripeService(IHttpContextAccessor contextAccessor, ILogger<StripeService> logger,
            UserManager<Base.Auth.Models.User> userManager, IUnitOfWork<AuthDbContext> unitOfWork) 
            : base(contextAccessor, logger, unitOfWork)
        {
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

        public async Task addCard(string stripeCardId, Base.Auth.Models.User user)
        {
            if (!string.IsNullOrEmpty(stripeCardId) && user != null)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);

                // Ensure the user has his/her stripe customer id up
                if (!userClaims.Any() || !userClaims.Any(uc =>
                        uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId)))
                {
                    // This shouldn't happen, but just in case
                    _logger.LogInformation($"{_serviceName} addCard: user has yet to bind to stripe");
                    throw new KeyNotFoundException($"{_serviceName} addCard: user has yet to bind to stripe");
                }

                // Ensure the user has yet to bind this card.
                if (userClaims.Any(uc => uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerCardId)
                                         && uc.Value.Equals(stripeCardId)))
                {
                    // This shouldn't happen, but just in case
                    _logger.LogInformation($"{_serviceName} addCard: user already has the card...");
                    throw new KeyNotFoundException($"{_serviceName} addCard: user has completed binding earlier.");
                }

                var userStripeCustId = userClaims
                    .SingleOrDefault(uc => uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId))?
                    .Value;

                if (string.IsNullOrEmpty(userStripeCustId))
                {
                    // This shouldn't happen, but just in case
                    _logger.LogInformation($"{_serviceName} addCard: user has yet to bind to stripe");
                    throw new KeyNotFoundException($"{_serviceName} addCard: user has yet to bind to stripe");
                }

                var cardService = new CardService();
                var card = cardService.Get(userStripeCustId, stripeCardId);

                // If the card ain't null, bind it
                if (card != null)
                {
                    var cardUserClaim = new Claim(NozomiJwtClaimTypes.StripeCustomerCardId, card.Id);
                    await _userManager.AddClaimAsync(user, cardUserClaim);

                    _logger.LogInformation($"{_serviceName} addCard: {user.Id} successfully added a new card" +
                                           $" tokenized as {cardUserClaim.Value}");
                    return; // Done!
                }
                
                _logger.LogInformation($"{_serviceName} addCard: There was an issue related to binding cardId" +
                                       $" {stripeCardId} to user {user.Id}.");
                throw new StripeException($"{_serviceName} addCard: There was a problem binding the newly" +
                                          $" created card of {stripeCardId} to {user.Id}");
            }
            
            _logger.LogInformation($"{_serviceName} addCard: Invalid tokenId or userId.");
            throw new InvalidConstraintException($"{_serviceName} addCard: Invalid tokenId or userId.");
        }

        public async void removeCard(string stripeCardId, Base.Auth.Models.User user)
        {
            if (!string.IsNullOrEmpty(stripeCardId) && user != null)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);

                // Ensure the user has his/her stripe customer id up
                if (!userClaims.Any() || !userClaims.Any(uc =>
                        uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId)))
                {
                    // This shouldn't happen, but just in case
                    _logger.LogInformation($"{_serviceName} addCard: user has yet to bind to stripe");
                    throw new KeyNotFoundException($"{_serviceName} addCard: user has yet to bind to stripe");
                }

                // Ensure the user is binded to this card
                if (!userClaims.Any(uc => uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerCardId)
                                          && uc.Value.Equals(stripeCardId)))
                {
                    // This shouldn't happen, but just in case
                    _logger.LogInformation($"{_serviceName} addCard: user already has the card...");
                    throw new KeyNotFoundException($"{_serviceName} addCard: user has completed binding earlier.");
                }

                var userStripeCustId = userClaims
                    .SingleOrDefault(uc => uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerId))?
                    .Value;

                if (string.IsNullOrEmpty(userStripeCustId))
                {
                    // This shouldn't happen, but just in case
                    _logger.LogInformation($"{_serviceName} removeCard: user has yet to bind to stripe");
                    throw new KeyNotFoundException($"{_serviceName} removeCard: user has yet to bind to stripe");
                }

                // Obtain the card claim directly
                var stripeCardClaim = userClaims.SingleOrDefault(uc => 
                    uc.Type.Equals(NozomiJwtClaimTypes.StripeCustomerCardId) && uc.Value.Equals(stripeCardId));

                if (stripeCardClaim == null) // This shouldn't happen, but just in case
                {
                    _logger.LogInformation($"{_serviceName} removeCard: user doesn't own the card what a bitch");
                    throw new KeyNotFoundException($"{_serviceName} removeCard: user does not own the card");
                }

                var cardService = new CardService();
                var card = cardService.Delete(userStripeCustId, stripeCardId);

                // If the card's deleted, delete it on our end
                if (card.Deleted != null && (bool) card.Deleted)
                {
                    await _userManager.RemoveClaimAsync(user, stripeCardClaim);

                    _logger.LogInformation($"{_serviceName} removeCard: {user.Id} successfully removed a card" +
                                           $" that was tokenized as {stripeCardClaim.Value}");
                    return; // Done!
                }
                
                _logger.LogInformation($"{_serviceName} removeCard: There was an issue deleting the card " +
                                       $"{stripeCardId} from user {user.Id}");
                throw new StripeException($"{_serviceName} removeCard: There was an issue deleting " +
                                               $"{stripeCardId} from user {user.Id}");
            }

            _logger.LogInformation($"{_serviceName} removeCard: Invalid cardId or userId.");
            throw new InvalidConstraintException($"{_serviceName} removeCard: Invalid cardId or userId.");
        }

        public async void subscribePlan(Plan plan, Base.Auth.Models.User user) {
            if (user != null) {
                if (plan != null) {

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
                    if (customerIdClaim != null) {

                        var subscriptionOptions = new SubscriptionCreateOptions
                        {
                            Customer = customerIdClaim.Value,
                            Items = new List<SubscriptionItemOptions> {
                                new SubscriptionItemOptions
                                {
                                    Plan = plan.Id
                                }
                            }
                        };

                        var subscription = _subscriptionService.Create(subscriptionOptions);

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

        public void cancelPlan(Base.Auth.Models.User user)
        {
            throw new NotImplementedException();
        }

        public void changePlan(Plan plan, Base.Auth.Models.User user)
        {
            throw new NotImplementedException();
        }
    }
}