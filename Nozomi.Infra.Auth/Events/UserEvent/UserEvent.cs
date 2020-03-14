﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using Polly.Retry;

namespace Nozomi.Infra.Auth.Events.UserEvent
{
    public class UserEvent : BaseEvent<UserEvent, AuthDbContext>, IUserEvent
    {
        private readonly UserManager<Base.Auth.Models.User> _userManager;
        public UserEvent(ILogger<UserEvent> logger, UserManager<Base.Auth.Models.User> userManager,
            AuthDbContext authDbContext) 
            : base(logger, authDbContext)
        {
            _userManager = userManager;
        }

        public bool HasStripe(string userId)
        {
            const string methodName = "HasStripe";
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException($"{_eventName} {methodName}: Invalid userId.");

            return _context.Users.AsNoTracking()
                .Where(u => u.Id.Equals(userId))
                .Include(u => u.UserClaims)
                .Any(u => u.UserClaims != null && u.UserClaims.Count > 0
                    && u.UserClaims.Any(uc => uc.ClaimType.Equals(NozomiJwtClaimTypes.StripeCustomerId)));
        }

        public bool HasDefaultPaymentMethod(string userId)
        {
            const string methodName = "HasDefaultPaymentMethod";
            const string claimType = NozomiJwtClaimTypes.StripeCustomerDefaultPaymentId;
            
            if(string.IsNullOrEmpty(userId))
                throw new ArgumentNullException($"{_eventName} {methodName}: Invalid userId.");

            var defaultPaymentIdClaim = GetUserClaim(userId, claimType);

            return defaultPaymentIdClaim != null;
        }

        public bool HasPaymentMethod(string userId, string paymentMethodId)
        {
            const string methodName = "HasDefaultPaymentMethod";
            const string claimType = NozomiJwtClaimTypes.StripeCustomerPaymentMethodId;
            
            if(string.IsNullOrEmpty(userId))
                throw new ArgumentNullException($"{_eventName} {methodName}: Invalid userId.");
            if(string.IsNullOrEmpty(paymentMethodId))
                throw new ArgumentNullException($"{_eventName} {methodName}: Invalid paymentMethodId");
            
            var userClaim = GetUserClaims(userId, claimType).SingleOrDefault(uc => uc.ClaimValue.Equals(paymentMethodId));

            return userClaim != null;
        }

        public string GetStripeCustomerId(string userId)
        {
            const string methodName = "GetStripeCustomerId";
            const string claimType = NozomiJwtClaimTypes.StripeCustomerId;
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException($"{_eventName} {methodName}: Invalid userId.");

            var claim = GetUserClaim(userId, claimType);

            return claim?.ClaimValue;
        }

        public string GetUserActiveSubscriptionId(string userId)
        {
            const string methodName = "GetUserActiveSubscriptionId";
            const string claimType = NozomiJwtClaimTypes.StripeSubscriptionId;
            if(string.IsNullOrEmpty(userId))
                throw new ArgumentNullException($"{_eventName} {methodName}: Invalid userId.");

            var claim = GetUserClaim(userId, claimType);

            return claim?.ClaimValue;
        }

        public IEnumerable<string> GetUserPaymentMethods(string userId)
        {
            const string methodName = "GetUserPaymentMethods";
            const string claimType = NozomiJwtClaimTypes.StripeCustomerPaymentMethodId;

            var userClaims = GetUserClaims(userId, claimType);
            
            return userClaims.Select(uc => uc.ClaimValue);
        }

        public async Task<User> GetUserByCustomerId(string id)
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

        private UserClaim GetUserClaim(string userId, string claimType)
        {
            return _context.UserClaims.AsTracking().SingleOrDefault(claim => claim.ClaimType.Equals(claimType) && claim.UserId.Equals(userId));
        }

        private IEnumerable<UserClaim> GetUserClaims(string userId, string claimType)
        {
            return _context.UserClaims.AsTracking().Where(claim => claim.ClaimType.Equals(claimType) && claim.UserId.Equals(userId));
        }
    }
}