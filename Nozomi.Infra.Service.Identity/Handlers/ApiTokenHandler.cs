using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Nozomi.Preprocessing;
using Nozomi.Service.Identity.Requirements;

namespace Nozomi.Service.Identity.Handlers
{
    public class ApiTokenHandler : AuthorizationHandler<ApiTokenRequirement>
    {
        // https://stackoverflow.com/questions/47809437/how-to-access-current-httpcontext-in-asp-net-core-2-custom-policy-based-authoriz
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDistributedCache _cache;
        
        public ApiTokenHandler(IHttpContextAccessor httpContextAccessor, IDistributedCache cache)
        {
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ApiTokenRequirement requirement)
        {
            if (!_httpContextAccessor.HttpContext.Request.Headers
                .Any(h => h.Key.Equals(NozomiServiceConstants.ApiTokenHeaderKey)) &&
                !context.User.Identity.IsAuthenticated)
            {
                return Task.CompletedTask;
            }

            var apiTokenSecret = _httpContextAccessor.HttpContext.Request.Headers
                .SingleOrDefault(h => h.Key.Equals(NozomiServiceConstants.ApiTokenHeaderKey)).Value;
            
            if (string.IsNullOrEmpty(apiTokenSecret))
            {
                // Identity-based
                if (context.User.Identity.IsAuthenticated)
                {
                    context.Succeed(requirement);
                }
            } else
            {
                var cachedToken = _cache.GetString(apiTokenSecret).Any();
                
                if (cachedToken) context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}