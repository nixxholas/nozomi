using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.ApiToken;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Identity.Services.Interfaces;

namespace Nozomi.Ticker.Areas.v1.ApiToken
{
    [Authorize]
    public class ApiTokenController : BaseController<ApiTokenController>, IApiTokenController
    {
        private readonly IApiTokenService _apiTokenService;
        
        public ApiTokenController(ILogger<ApiTokenController> logger, NozomiUserManager userManager,
            IApiTokenService apiTokenService)
            : base(logger, userManager)
        {
            _apiTokenService = apiTokenService;
        }

        [HttpGet]
        public async Task<NozomiResult<ICollection<ApiTokenResult>>> ApiTokens()
        {
            var user = await GetCurrentUserAsync();
            
            if (user == null) return new NozomiResult<ICollection<ApiTokenResult>>(NozomiResultType.Failed, 
                "You are not authorized to perform this action.");

            return new NozomiResult<ICollection<ApiTokenResult>>();
        }
        
        [HttpPost]
        public async Task<NozomiResult<ApiTokenResult>> GenerateToken(string label = null)
        {
            var user = await GetCurrentUserAsync();
            
            if (user == null) return new NozomiResult<ApiTokenResult>(NozomiResultType.Failed, 
                "You are not authorized to perform this action.");

            var res = await _apiTokenService.GenerateTokenAsync(user.Id, label);

            ApiTokenResult apiTokenRes = null;

            if (res != null)
            {
                apiTokenRes = new ApiTokenResult
                {
                    Key = res.Key,
                    Secret = res.Secret,
                    Label = label // For the view
                };
            }
            
            return new NozomiResult<ApiTokenResult>(apiTokenRes);
        }

        [HttpDelete]
        public Task<NozomiResult<ApiTokenRevocationResult>> RevokeToken()
        {
            throw new System.NotImplementedException();
        }
    }
}