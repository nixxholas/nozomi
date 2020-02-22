using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Data.Models.Currency;
using Nozomi.Infra.Auth.Events.Stripe;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Repository;

namespace Nozomi.Infra.Auth.Services.QuotaClaims
{
    public class QuotaClaimsService : BaseService<QuotaClaimsService, AuthDbContext>, IQuotaClaimsService
    {
        private readonly UserManager<Base.Auth.Models.User> _userManager;
        private readonly IStripeEvent _stripeEvent;
        
        public QuotaClaimsService(ILogger<QuotaClaimsService> logger, IUnitOfWork<AuthDbContext> unitOfWork, 
            IStripeEvent stripeEvent, UserManager<Base.Auth.Models.User> userManager) : base(logger, unitOfWork)
        {
            _userManager = userManager;
            _stripeEvent = stripeEvent;
        }

        public void SetQuota(Base.Auth.Models.User user, int quotaAmt)
        {
            const string methodName = "SetQuota";
            const string claimType = NozomiJwtClaimTypes.UserQuota;
            PerformUserPrecheck(user, methodName);

            var quotaClaim = GetUserClaim(user.Id, claimType);
            
            if (quotaClaim == null)
            {
                CreateUserClaim(user, claimType, quotaAmt.ToString(), methodName);
            }
            else
            {
                quotaClaim.ClaimValue = quotaAmt.ToString();
                UpdateUserClaim(user, quotaClaim, methodName);
            }
        }

        public void AddUsage(Base.Auth.Models.User user, int usageAmt = 1)
        {
            const string methodName = "AddUsage";
            const string claimType = NozomiJwtClaimTypes.UserUsage;
            PerformUserPrecheck(user,methodName);

            var usageClaim = GetUserClaim(user.Id, claimType);

            if (usageClaim == null)
            {
                CreateUserClaim(user, claimType, usageAmt.ToString(), methodName);
            }
            else
            {
                var updatedUsage = ParseStringToInt(usageClaim.ClaimValue, methodName) + usageAmt;
                usageClaim.ClaimValue = updatedUsage.ToString();

                UpdateUserClaim(user, usageClaim, methodName);
            }
        }

        public void RestUsage(Base.Auth.Models.User user, int usageAmt = 0)
        {
            const string methodName = "ResetUsage";
            const string claimType = NozomiJwtClaimTypes.UserUsage;
            PerformUserPrecheck(user, methodName);

            var usageClaim = GetUserClaim(user.Id, claimType);
            
            if (usageClaim == null)
            {
                CreateUserClaim(user, claimType, usageAmt.ToString(), methodName);
            }
            else
            {
                usageClaim.ClaimValue = usageAmt.ToString();
                UpdateUserClaim(user, usageClaim, methodName);
            }
        }

        private void PerformUserPrecheck(Base.Auth.Models.User user, string methodName)
        {
            if(user == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: User is null");
        }

        private UserClaim GetUserClaim(string userId, string claimType)
        {
            return _unitOfWork.GetRepository<UserClaim>().GetQueryable().AsTracking().SingleOrDefault(claim => claim.ClaimType.Equals(claimType) && claim.UserId.Equals(userId));
        }

        private void CreateUserClaim(Base.Auth.Models.User user, string claimType, string claimValue, string methodName)
        {
            var userClaim = new UserClaim
            {
                ClaimType = claimType,
                ClaimValue = claimValue,
                UserId = user.Id
            };
            
            _unitOfWork.GetRepository<UserClaim>().Add(userClaim);
            if(_unitOfWork.Commit(user.Id) != 1)
                throw new InvalidOperationException($"{_serviceName} {methodName}: Failed to create user claim");
        }

        private void UpdateUserClaim(Base.Auth.Models.User user, UserClaim userClaim, string methodName)
        {
            _unitOfWork.GetRepository<UserClaim>().Update(userClaim);
            if (_unitOfWork.Commit(user.Id) != 1)
                throw new InvalidOperationException($"{_serviceName} {methodName}: Failed to update user claim");
        }

        private int ParseStringToInt(string value, string methodName)
        {
            var parseSuccess = int.TryParse(value, out var parsedValue);
            
            if(!parseSuccess)
                throw new InvalidCastException($"{_serviceName} {methodName}: Error parsing {value} to int");

            return parsedValue;
        }

    }
}