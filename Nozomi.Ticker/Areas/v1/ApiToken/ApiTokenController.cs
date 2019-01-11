using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.ApiToken;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Identity.Services.Interfaces;

namespace Nozomi.Ticker.Areas.v1.ApiToken
{
    public class ApiTokenController : BaseController<ApiTokenController>, IApiTokenController
    {
        private readonly IApiTokenService _apiTokenService;
        
        public ApiTokenController(ILogger<ApiTokenController> logger, NozomiUserManager userManager,
            IApiTokenService apiTokenService)
            : base(logger, userManager)
        {
            _apiTokenService = apiTokenService;
        }

        [Authorize]
        public async Task<NozomiResult<ICollection<ApiTokenResult>>> ApiTokens()
        {
            var user = await GetCurrentUserAsync();
            
            if (user == null) return new NozomiResult<ICollection<ApiTokenResult>>(NozomiResultType.Failed, 
                "You are not authorized to perform this action.");

            return new NozomiResult<ICollection<ApiTokenResult>>();
        }

        public Task<NozomiResult<ApiTokenResult>> CreateToken()
        {
            throw new System.NotImplementedException();
        }

        public Task<NozomiResult<ApiTokenRevocationResult>> RevokeToken()
        {
            throw new System.NotImplementedException();
        }
    }
}