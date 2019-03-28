using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.ViewModels.AreaModels.v1.ApiToken;
using Nozomi.Data;
using Nozomi.Service.Identity.Events.Auth.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Identity.Services.Interfaces;
using StackExchange.Redis;

namespace Nozomi.Ticker.Areas.v1.ApiToken
{
    [Authorize]
    public class ApiTokenController : BaseController<ApiTokenController>, IApiTokenController
    {
        private readonly IApiTokenEvent _apiTokenEvent;
        private readonly IApiTokenService _apiTokenService;
        
        public ApiTokenController(ILogger<ApiTokenController> logger, NozomiUserManager userManager,
            IApiTokenEvent apiTokenEvent, IApiTokenService apiTokenService)
            : base(logger, userManager)
        {
            _apiTokenEvent = apiTokenEvent;
            _apiTokenService = apiTokenService;
        }

        [HttpGet]
        public async Task<NozomiResult<ICollection<ApiTokenResult>>> ApiTokens()
        {
            var user = await GetCurrentUserAsync();
            
            if (user == null) return new NozomiResult<ICollection<ApiTokenResult>>(NozomiResultType.Failed, 
                "You are not authorized to perform this action.");

            var res = await _apiTokenEvent.ApiTokensByUserId(user.Id, true);

            ICollection<ApiTokenResult> apiTokenResults = null;

            if (res != null)
            {
                apiTokenResults = new List<ApiTokenResult>(res.Select(token => token.ToApiTokenResult()));
            }
            
            return new NozomiResult<ICollection<ApiTokenResult>>(apiTokenResults);
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
                apiTokenRes = res.ToApiTokenResult(true);
            }
            
            // TODO: RE-ENABLE THIS
            return new NozomiResult<ApiTokenResult>(null);
            
            return new NozomiResult<ApiTokenResult>(apiTokenRes);
        }

        [HttpPost]
        public async Task<NozomiResult<ApiTokenRevocationResult>> RevokeToken(string tokenGuid)
        {
            if (string.IsNullOrEmpty(tokenGuid)) return new NozomiResult<ApiTokenRevocationResult>(NozomiResultType.Failed, 
                "Invalid API token.");
            
            var user = await GetCurrentUserAsync();
            
            if (user == null) return new NozomiResult<ApiTokenRevocationResult>(NozomiResultType.Failed, 
                "You are not authorized to perform this action.");

            var res = await _apiTokenService.RevokeTokenAsync(Guid.Parse(tokenGuid), user.Id);
            
            return new NozomiResult<ApiTokenRevocationResult>()
            {
                ResultType = res ? NozomiResultType.Success : NozomiResultType.Failed
            };
        }
    }
}