using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.BCL.Configurations;
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

        public Task SetQuota(Base.Auth.Models.User user, int quotaAmt)
        {
            throw new NotImplementedException();
        }

        public Task AddUsage(Base.Auth.Models.User user, int usageAmt)
        {
            throw new NotImplementedException();
        }

        public Task RestUsage(Base.Auth.Models.User user, int usageAmt = 0)
        {
            throw new NotImplementedException();
        }

        private void PerformUserPrecheck(Base.Auth.Models.User user, string methodName)
        {
            if(user == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: User is null");
        }

        private async Task<IList<Claim>> GetUserClaims(Base.Auth.Models.User user)
        {
            return await _userManager.GetClaimsAsync(user);
        }
    }
}