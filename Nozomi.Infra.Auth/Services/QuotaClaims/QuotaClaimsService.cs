using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
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

        public bool SetQuota(Base.Auth.Models.User user, int quotaAmt)
        {
            var methodName = "SetQuota";
            var claimType = NozomiJwtClaimTypes.UserQuota;
            PerformUserPrecheck(user, methodName);

            var quotaClaim = GetUserClaim(user.Id, claimType);
            
            if (quotaClaim == null)
            {
                return CreateUserClaim(user, claimType, quotaAmt.ToString());
            }
            
            quotaClaim.ClaimValue = quotaAmt.ToString();
            return UpdateUserClaim(user, quotaClaim);
        }

        public bool AddUsage(Base.Auth.Models.User user, int usageAmt = 1)
        {
            throw new NotImplementedException();
        }

        public bool RestUsage(Base.Auth.Models.User user, int usageAmt = 0)
        {
            throw new NotImplementedException();
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

        private bool CreateUserClaim(Base.Auth.Models.User user, string claimType, string claimValue)
        {
            var userClaim = new UserClaim
            {
                ClaimType = claimType,
                ClaimValue = claimValue,
                UserId = user.Id
            };
            
            _unitOfWork.GetRepository<UserClaim>().Add(userClaim);
            return (_unitOfWork.Commit(user.Id) == 1);
        }

        private bool UpdateUserClaim(Base.Auth.Models.User user, UserClaim userClaim)
        {
            _unitOfWork.GetRepository<UserClaim>().Update(userClaim);
            return (_unitOfWork.Commit(user.Id) == 1);
        }


    }
}