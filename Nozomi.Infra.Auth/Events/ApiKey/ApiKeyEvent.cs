using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;

namespace Nozomi.Infra.Auth.Events.ApiKey
{
    public class ApiKeyEvent : BaseEvent<ApiKeyEvent, AuthDbContext>, IApiKeyEvent
    {
        public ApiKeyEvent(ILogger<ApiKeyEvent> logger, AuthDbContext context) : base(logger, context)
        {
        }

        public bool Exists(string apiKey)
        {
            return _context.UserClaims.AsNoTracking()
                .Any(uc => uc.ClaimType.Equals(NozomiJwtClaimTypes.ApiKeys)
                             && uc.ClaimValue.SequenceEqual(apiKey));
        }

        public string View(string apiKey, string userId = null)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<string> ViewAll(string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}