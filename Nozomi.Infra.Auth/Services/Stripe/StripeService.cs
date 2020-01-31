using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Repository;
using Stripe;

namespace Nozomi.Infra.Auth.Services.Stripe
{
    public class StripeService : BaseService<StripeService, AuthDbContext>, IStripeService
    {
        private readonly StripeClient _stripeClient;
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
                _stripeClient = new StripeClient(stripeConfiguration.Value.SecretKey);
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
                _stripeClient = new StripeClient(stripeConfiguration.Value.SecretKey);
            }

            _userManager = userManager;
        }


        public async void addCard(string stripeCardTokenId, Base.Auth.Models.User user)
        {
            if (!string.IsNullOrEmpty(stripeCardTokenId) && user?.UserClaims != null
                                                         && user.UserClaims.Any(uc =>
                                                             uc.ClaimType.Equals(NozomiJwtClaimTypes.StripeCustomerId))
                                                         && !user.UserClaims.Any(uc =>
                                                             uc.ClaimType.Equals(NozomiJwtClaimTypes
                                                                 .StripeCustomerCardId) 
                                                             && uc.ClaimValue.Equals(stripeCardTokenId)))
            {
                var userStripeCustId = user.UserClaims
                    .SingleOrDefault(uc => uc.ClaimType.Equals(NozomiJwtClaimTypes.StripeCustomerId))?
                    .ClaimValue;

                if (string.IsNullOrEmpty(userStripeCustId))
                {
                    // This shouldn't happen, but just in case
                    _logger.LogInformation($"{_serviceName} addCard: user has yet to bind to stripe");
                    throw new KeyNotFoundException($"{_serviceName} addCard: user has yet to bind to stripe");
                }

                var cardService = new CardService();
                var card = cardService.Get(userStripeCustId, stripeCardTokenId);

                // If the card ain't null, bind it
                if (card != null)
                {
                    var cardUserClaim = new Claim(NozomiJwtClaimTypes.StripeCustomerCardId, card.Id);
                    await _userManager.AddClaimAsync(user, cardUserClaim);

                    _logger.LogInformation($"{_serviceName} addCard: {user.Id} successfully added a new card" +
                                           $" tokenized as {cardUserClaim.Value}");
                    return; // Done!
                }
            }

            _logger.LogInformation($"{_serviceName} addCard: Invalid tokenId or userId.");
            throw new InvalidConstraintException($"{_serviceName} addCard: Invalid tokenId or userId.");
        }
    }
}