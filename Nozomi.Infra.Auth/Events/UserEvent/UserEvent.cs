using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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

        public bool Exists(string userId)
        {
            return _context.Users.AsNoTracking().Any(u => u.Id.Equals(userId));
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
            
            var paymentMethodClaim = GetUserClaims(userId, claimType).SingleOrDefault(uc => uc.ClaimValue.Equals(paymentMethodId));

            return paymentMethodClaim != null;
        }

        public bool IsInRoles(string userId, ICollection<string> roleNames)
        {
            return _context.UserRoles.AsNoTracking()
                .Where(ur => ur.UserId.Equals(userId))
                .Include(ur => ur.Role)
                .AsEnumerable()
                .Any(ur => roleNames.Contains(ur.Role.Name));
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

            if (users.Count > 1)
                throw new InvalidOperationException($"{_eventName} GetUserByCustomerId: More than one user is" +
                                                    $"binded to the same stripe customer id.");
            else if (!users.Any())
                throw new InvalidOperationException($"{_eventName} GetUserByCustomerId: No user found.");

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