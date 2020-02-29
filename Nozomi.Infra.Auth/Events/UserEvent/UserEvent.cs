using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Repository;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Stripe;

namespace Nozomi.Infra.Auth.Events.UserEvent
{
    public class UserEvent : BaseEvent<UserEvent, AuthDbContext>, IUserEvent
    {
        private readonly UserManager<Base.Auth.Models.User> _userManager;
        public UserEvent(ILogger<UserEvent> logger, IUnitOfWork<AuthDbContext> unitOfWork, UserManager<Base.Auth.Models.User> userManager) : base(logger, unitOfWork)
        {
            _userManager = userManager;
        }

        public bool HasStripe(string userId)
        {
            const string methodName = "HasStripe";
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException($"{_eventName} {methodName}: Invalid userId.");

            return _unitOfWork.GetRepository<Base.Auth.Models.User>()
                .GetQueryable()
                .AsNoTracking()
                .Where(u => u.Id.Equals(userId))
                .Include(u => u.UserClaims)
                .Any(u => u.UserClaims != null && u.UserClaims.Count > 0
                    && u.UserClaims.Any(uc => uc.ClaimType.Equals(NozomiJwtClaimTypes.StripeCustomerId)));
        }

        public bool HasDefaultPaymentMethod(string userId)
        {
            const string methodName = "HasDefaultPaymentMethod";
            if(string.IsNullOrEmpty(userId))
                throw new ArgumentNullException($"{_eventName} {methodName}: Invalid userId.");
            
            return _unitOfWork.GetRepository<Base.Auth.Models.User>()
                .GetQueryable()
                .AsNoTracking()
                .Where(u => u.Id.Equals(userId))
                .Include(u => u.UserClaims)
                .Any(u => u.UserClaims != null && u.UserClaims.Count > 0
                                               && u.UserClaims.Any(uc => uc.ClaimType.Equals(NozomiJwtClaimTypes.StripeCustomerDefaultPaymentId)));
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

        private UserClaim GetUserClaim(string userId, string claimType)
        {
            return _unitOfWork.GetRepository<UserClaim>().GetQueryable().AsTracking().SingleOrDefault(claim => claim.ClaimType.Equals(claimType) && claim.UserId.Equals(userId));
        }

        private IEnumerable<UserClaim> GetUserClaims(string userId, string claimType)
        {
            return _unitOfWork.GetRepository<UserClaim>().GetQueryable().AsTracking().Where(claim => claim.ClaimType.Equals(claimType) && claim.UserId.Equals(userId));
        }
    }
}