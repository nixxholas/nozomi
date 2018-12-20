using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;

namespace Nozomi.Service.Identity
{
    public class NozomiProfileService : IProfileService
    {
        private readonly ILogger<NozomiProfileService> _logger;
        
        public NozomiProfileService(ILogger<NozomiProfileService> logger)
        {
            _logger = logger;
        }
        
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.LogProfileRequest(_logger);

            var claimsNames = new List<string>();
            claimsNames.AddRange(context.RequestedResources.IdentityResources.SelectMany(r => r.UserClaims));
            claimsNames.AddRange(context.RequestedResources.ApiResources.SelectMany(r => r.UserClaims));

            claimsNames.AddRange(new[]{
                "rootadmin",
                "role",
                "username",
                "nickname",
                ClaimTypes.Role,
                ClaimTypes.Name
            });

            context.RequestedClaimTypes = claimsNames;
            context.AddRequestedClaims(context.Subject.Claims);

            context.LogIssuedClaims(_logger);
            return Task.CompletedTask;
        }
        
        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.FromResult(0);
        }
    }
}