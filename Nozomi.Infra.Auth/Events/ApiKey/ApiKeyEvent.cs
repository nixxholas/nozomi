using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Data.ViewModels.ApiKey;
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

        public IEnumerable<ApiKeyViewModel> ViewAll(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var query = _context.UserClaims.AsNoTracking()
                    .Where(uc => (uc.ClaimType.Equals(NozomiJwtClaimTypes.ApiKeys) 
                                  || uc.ClaimType.StartsWith(NozomiJwtClaimTypes.ApiKeyLabels)) 
                                 && uc.UserId.Equals(userId));

                var result = new List<ApiKeyViewModel>();
                foreach (var apiKey in query.Where(q =>
                    q.ClaimType.Equals(NozomiJwtClaimTypes.ApiKeys)))
                {
                    var label = query.SingleOrDefault(l => l.ClaimType
                        .Equals(string.Concat(NozomiJwtClaimTypes.ApiKeyLabels, apiKey.ClaimValue)));
                    
                    result.Add(new ApiKeyViewModel
                    {
                        Label = label == null ? string.Empty : label.ClaimValue,
                        ApiKeyMasked = apiKey.ClaimValue // TODO: MASK
                    });
                }

                return result;
            }
            
            throw new KeyNotFoundException("Invalid user ID.");
        }
    }
}