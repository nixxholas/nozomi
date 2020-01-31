using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<Base.Auth.Models.User> _userManager;

        public StripeService(ILogger<StripeService> logger, IUnitOfWork<AuthDbContext> unitOfWork,
            UserManager<Base.Auth.Models.User> userManager, IOptions<StripeOptions> stripeConfiguration)
            : base(logger, unitOfWork)
        {
            if (stripeConfiguration == null || stripeConfiguration.Value == null
                                            || string.IsNullOrEmpty(stripeConfiguration.Value.PublishableKey)
                                            || string.IsNullOrEmpty(stripeConfiguration.Value.SecretKey))
            {
                _logger.LogCritical($"{_serviceName} Potential failure detected. Please configure Stripe!");
            }
            else
            {
                // apiKey = Secret Key
                StripeConfiguration.ApiKey = stripeConfiguration.Value.SecretKey;
            }

            _userManager = userManager;
        }

        public StripeService(IHttpContextAccessor contextAccessor, ILogger<StripeService> logger,
            UserManager<Base.Auth.Models.User> userManager, IUnitOfWork<AuthDbContext> unitOfWork,
            IOptions<StripeOptions> stripeConfiguration) : base(contextAccessor, logger, unitOfWork)
        {
            if (stripeConfiguration == null || stripeConfiguration.Value == null
                                            || string.IsNullOrEmpty(stripeConfiguration.Value.PublishableKey)
                                            || string.IsNullOrEmpty(stripeConfiguration.Value.SecretKey))
            {
                _logger.LogCritical($"{_serviceName} Potential failure detected. Please configure Stripe!");
            }
            else
            {
                // apiKey = Secret Key
                StripeConfiguration.ApiKey = stripeConfiguration.Value.SecretKey;
            }

            _userManager = userManager;
        }


        public async void addCard(string stripeCardId, Base.Auth.Models.User user)
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
    }
}