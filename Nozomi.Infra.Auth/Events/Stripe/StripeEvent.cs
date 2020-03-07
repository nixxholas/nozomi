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
using Nozomi.Base.Auth.Models;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Repository;
using Stripe;

namespace Nozomi.Infra.Auth.Events.Stripe
{
    public class StripeEvent : BaseEvent<StripeEvent, AuthDbContext>, IStripeEvent
    {
        private readonly IOptions<StripeOptions> _stripeOptions;
        private readonly Product _stripeProduct;
        private readonly UserManager<Base.Auth.Models.User> _userManager;

        public StripeEvent(ILogger<StripeEvent> logger, IUnitOfWork<AuthDbContext> unitOfWork, 
            UserManager<Base.Auth.Models.User> userManager, IOptions<StripeOptions> stripeOptions) 
            : base(logger, unitOfWork)
        {
            // apiKey = Secret Key
            StripeConfiguration.ApiKey = stripeOptions.Value.SecretKey;
            _stripeOptions = stripeOptions;
            
            var productService = new ProductService();
            _stripeProduct = productService.Get(stripeOptions.Value.ProductId);
            _userManager = userManager;
        }

        public async Task<Base.Auth.Models.User> GetUserByCustomerId(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new NullReferenceException($"{_eventName} GetUserByCustomerId: Customer Id is null.");

            var customerIdClaim = new UserClaim
            {
                ClaimType = NozomiJwtClaimTypes.StripeCustomerId,
                ClaimValue = id
            };

            var users = await _userManager.GetUsersForClaimAsync(customerIdClaim.ToClaim());

            if (users.Count > 1 || users.Count < 1)
                throw new InvalidOperationException($"{_eventName} GetUserByCustomerId: More than one user binded to the same stripe customer id.");

            return users.First();
        }
    }
}