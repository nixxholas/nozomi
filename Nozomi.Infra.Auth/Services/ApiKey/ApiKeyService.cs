using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.BCL.Helpers.Crypto;
using Nozomi.Infra.Auth.Events.ApiKey;
using Nozomi.Infra.Auth.Events.UserEvent;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;

namespace Nozomi.Infra.Auth.Services.ApiKey
{
    public class ApiKeyService : BaseService<ApiKeyService, AuthDbContext>, IApiKeyService
    {
        private readonly IApiKeyEvent _apiKeyEvent;
        private readonly IUserEvent _userEvent;
        
        public ApiKeyService(ILogger<ApiKeyService> logger, AuthDbContext context,
            IApiKeyEvent apiKeyEvent, IUserEvent userEvent) : base(logger, context)
        {
            _apiKeyEvent = apiKeyEvent;
            _userEvent = userEvent;
        }

        public ApiKeyService(IHttpContextAccessor contextAccessor, ILogger<ApiKeyService> logger, 
            AuthDbContext context, IApiKeyEvent apiKeyEvent, IUserEvent userEvent) 
            : base(contextAccessor, logger, context)
        {
            _apiKeyEvent = apiKeyEvent;
            _userEvent = userEvent;
        }

        public void GenerateApiKey(string userId)
        {
            var newKey = Randomizer.GenerateRandomCryptographicKey(64);
            while (_apiKeyEvent.Exists(newKey)) // Ensure truly random
            {
                newKey = Randomizer.GenerateRandomCryptographicKey(64);
            }

            if (_userEvent.Exists(userId))
            {
                _context.UserClaims.Add(new UserClaim
                {
                    ClaimType = NozomiJwtClaimTypes.ApiKeys,
                    ClaimValue = newKey,
                    UserId = userId
                });
            }
        }
    }
}