using System;
using System.Collections.Generic;
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

        public void AddPaymentMethod(string userId, string paymentMethodId)
        {
            const string methodName = "AddPaymentMethod";
            const string claimType = NozomiJwtClaimTypes.StripeCustomerPaymentMethodId;
            
            if(string.IsNullOrEmpty(userId))
                throw new NullReferenceException($"{_eventName} {methodName}: user Id is null.");
            
            if(string.IsNullOrEmpty(paymentMethodId))
                throw new NullReferenceException($"{_eventName} {methodName}: Payment method id is null.");

            CreateUserClaim(userId, claimType, paymentMethodId, methodName);
        }

        public void RemovePaymentMethod(string userId, string paymentMethodId)
        {
            const string methodName = "RemovePaymentMethod";
            const string claimType = NozomiJwtClaimTypes.StripeCustomerPaymentMethodId;

            var paymentMethodClaimToRemove = GetUserClaims(userId, claimType)
                .SingleOrDefault(claim => claim.ClaimValue.Equals(paymentMethodId));
            
            DeleteUserClaim(userId, paymentMethodClaimToRemove, methodName);
        }

        public void SetDefaultPaymentMethod(string userId, string paymentMethodId)
        {
            const string methodName = "SetDefaultPaymentMethod";
            const string claimType = NozomiJwtClaimTypes.StripeCustomerDefaultPaymentId;
            if(string.IsNullOrEmpty(userId))
                throw new ArgumentNullException($"{_eventName} {methodName}: User Id is null.");
            if(string.IsNullOrEmpty(paymentMethodId))
                throw  new ArgumentNullException($"{_eventName} {methodName}: Payment Method Id is null.");

            var defaultPaymentMethodClaim = GetUserClaim(userId, claimType);

            if (defaultPaymentMethodClaim == null)
            {
                CreateUserClaim(userId, claimType,paymentMethodId,methodName);
            }
            else
            {
                defaultPaymentMethodClaim.ClaimValue = paymentMethodId;
                UpdateUserClaim(userId, defaultPaymentMethodClaim,methodName);
            }
        }
        
        private UserClaim GetUserClaim(string userId, string claimType)
        {
            return _unitOfWork.GetRepository<UserClaim>().GetQueryable().AsTracking().SingleOrDefault(claim => claim.ClaimType.Equals(claimType) && claim.UserId.Equals(userId));
        }

        private IEnumerable<UserClaim> GetUserClaims(string userId, string claimType)
        {
            return _unitOfWork.GetRepository<UserClaim>().GetQueryable().AsTracking().Where(claim => claim.ClaimType.Equals(claimType) && claim.UserId.Equals(userId));
        }

        private void CreateUserClaim(string userId, string claimType, string claimValue, string methodName)
        {
            var userClaim = new UserClaim
            {
                ClaimType = claimType,
                ClaimValue = claimValue,
                UserId = userId
            };
            
            _unitOfWork.GetRepository<UserClaim>().Add(userClaim);
            if(_unitOfWork.Commit(userId) != 1)
                throw new InvalidOperationException($"{_eventName} {methodName}: Failed to create user claim");
        }

        private void UpdateUserClaim(string userId, UserClaim userClaim, string methodName)
        {
            _unitOfWork.GetRepository<UserClaim>().Update(userClaim);
            if (_unitOfWork.Commit(userId) != 1)
                throw new InvalidOperationException($"{_eventName} {methodName}: Failed to update user claim");
        }

        private void DeleteUserClaim(string userId, UserClaim userClaim, string methodName)
        {
            _unitOfWork.GetRepository<UserClaim>().Delete(userClaim);
            if(_unitOfWork.Commit(userId) != 1)
                throw new InvalidOperationException($"{_eventName} {methodName}: Failed to delete user claim");
        }

    }
}