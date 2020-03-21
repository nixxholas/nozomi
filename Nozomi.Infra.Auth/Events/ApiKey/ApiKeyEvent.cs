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
                             && uc.ClaimValue.Equals(apiKey));
        }

        public IEnumerable<ApiKeyViewModel> ViewAll(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                // TODO: Optimize for Nicholas, .AsEnumerable() causes error, temp fix .ToList()
                var query = _context.UserClaims.AsNoTracking()
                    .Where(uc => (uc.ClaimType.Equals(NozomiJwtClaimTypes.ApiKeys) 
                                  || uc.ClaimType.StartsWith(NozomiJwtClaimTypes.ApiKeyLabels)) 
                                 && uc.UserId.Equals(userId))
                    .ToList();

                var result = new List<ApiKeyViewModel>();
                foreach (var apiKey in query)
                {
                    var label = query.SingleOrDefault(l => l.ClaimType
                        .Equals(string.Concat(NozomiJwtClaimTypes.ApiKeyLabels, apiKey.ClaimValue)));

                    var threeQuarterStringCount = apiKey.ClaimValue.Length * 0.75;
                    result.Add(new ApiKeyViewModel
                    {
                        Label = label == null ? string.Empty : label.ClaimValue,
                        ApiKeyMasked = string.Concat("".PadLeft(apiKey.ClaimValue.Length, '*'), 
                            apiKey.ClaimValue.Substring(apiKey.ClaimValue.Length - (int)threeQuarterStringCount))
                    });
                }

                return result;
            }
            
            throw new KeyNotFoundException("Invalid user ID.");
        }
    }
}