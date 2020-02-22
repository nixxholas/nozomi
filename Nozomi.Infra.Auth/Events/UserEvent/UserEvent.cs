using System;
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
            throw new System.NotImplementedException();
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

        private void UpdateUserClaim(Base.Auth.Models.User user, UserClaim userClaim, string methodName)
        {
            _unitOfWork.GetRepository<UserClaim>().Update(userClaim);
            if (_unitOfWork.Commit(user.Id) != 1)
                throw new InvalidOperationException($"{_eventName} {methodName}: Failed to update user claim");
        }

    }
}