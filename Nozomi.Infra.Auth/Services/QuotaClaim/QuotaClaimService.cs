using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;

namespace Nozomi.Infra.Auth.Services.QuotaClaim
{
    public class QuotaClaimService : BaseService<QuotaClaimService, AuthDbContext>, IQuotaClaimService
    {
        public QuotaClaimService(ILogger<QuotaClaimService> logger, AuthDbContext authDbContext) 
            : base(logger, authDbContext)
        {
        }

        public void SetQuota(string userId, int quotaAmt)
        {
            const string methodName = "SetQuota";
            const string claimType = NozomiJwtClaimTypes.UserQuota;

            var quotaClaim = GetUserClaim(userId, claimType);
            
            if (quotaClaim == null)
            {
                CreateUserClaim(userId, claimType, quotaAmt.ToString(), methodName);
            }
            else
            {
                quotaClaim.ClaimValue = quotaAmt.ToString();
                UpdateUserClaim(userId, quotaClaim, methodName);
            }
        }

        public void AddUsage(string userId, int usageAmt = 1)
        {
            const string methodName = "AddUsage";
            const string claimType = NozomiJwtClaimTypes.UserUsage;

            var usageClaim = GetUserClaim(userId, claimType);

            if (usageClaim == null)
            {
                CreateUserClaim(userId, claimType, usageAmt.ToString(), methodName);
            }
            else
            {
                var updatedUsage = ParseStringToInt(usageClaim.ClaimValue, methodName) + usageAmt;
                usageClaim.ClaimValue = updatedUsage.ToString();

                UpdateUserClaim(userId, usageClaim, methodName);
            }
        }

        public void ResetUsage(string userId, int usageAmt = 0)
        {
            const string methodName = "ResetUsage";
            const string claimType = NozomiJwtClaimTypes.UserUsage;

            var usageClaim = GetUserClaim(userId, claimType);
            
            if (usageClaim == null)
            {
                CreateUserClaim(userId, claimType, usageAmt.ToString(), methodName);
            }
            else
            {
                usageClaim.ClaimValue = usageAmt.ToString();
                UpdateUserClaim(userId, usageClaim, methodName);
            }
        }

        private UserClaim GetUserClaim(string userId, string claimType)
        {
            return _context.UserClaims.AsTracking().SingleOrDefault(claim => claim.ClaimType.Equals(claimType) && claim.UserId.Equals(userId));
        }

        private void CreateUserClaim(string userId, string claimType, string claimValue, string methodName)
        {
            var userClaim = new UserClaim
            {
                ClaimType = claimType,
                ClaimValue = claimValue,
                UserId = userId
            };
            
            _context.UserClaims.Add(userClaim);
            if(_context.SaveChanges(userId) != 1)
                throw new InvalidOperationException($"{_serviceName} {methodName}: Failed to create user claim");
        }

        private void UpdateUserClaim(string userId, UserClaim userClaim, string methodName)
        {
            _context.UserClaims.Update(userClaim);
            if (_context.SaveChanges(userId) != 1)
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