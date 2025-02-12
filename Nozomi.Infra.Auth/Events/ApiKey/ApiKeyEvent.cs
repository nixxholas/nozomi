using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.ViewModels.ApiKey;
using Nozomi.Base.BCL.Extensions;
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
            return _context.ApiKeys.AsNoTracking()
                .Any(e => e.Value.Equals(apiKey));
        }

        public string Reveal(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var apiKey = _context.ApiKeys.AsTracking()
                    .OrderByDescending(e => e.CreatedAt)
                    .FirstOrDefault(e => e.UserId.Equals(userId) && !e.Revealed);

                if (apiKey != null)
                {
                    apiKey.Revealed = true;

                    _context.ApiKeys.Update(apiKey);
                    _context.SaveChanges();

                    return apiKey.Value;
                }

                _logger.LogInformation($"{_eventName} Reveal: Nothing to return for user {userId}.");
                return string.Empty;
            }
            
            throw new ArgumentException("Invalid User.");
        }

        public IEnumerable<ApiKeyViewModel> ViewAll(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                return _context.ApiKeys.AsNoTracking()
                    .Where(e => e.UserId.Equals(userId))
                    .Select(e => new ApiKeyViewModel
                    {
                        Guid = e.Guid,
                        Label = e.Label,
                        ApiKeyMasked = string.Concat("Ending with ", e.Value.Substring(e.Value.Length - 8))
                    });

                // var result = new List<ApiKeyViewModel>();
                // foreach (var apiKey in query)
                // {
                //     var label = query.SingleOrDefault(l => l.ClaimType
                //         .Equals(string.Concat(NozomiJwtClaimTypes.ApiKeyLabels, apiKey.ClaimValue)));
                //
                //     var threeQuarterStringCount = apiKey.ClaimValue.Length * 0.75;
                //     result.Add(new ApiKeyViewModel
                //     {
                //         Label = label == null ? string.Empty : label.ClaimValue,
                //         ApiKeyMasked = string.Concat("".PadLeft(apiKey.ClaimValue.Length, '*'), 
                //             apiKey.ClaimValue.Substring(apiKey.ClaimValue.Length - (int)threeQuarterStringCount))
                //     });
                // }

                // return result;
            }
            
            throw new KeyNotFoundException("Invalid user ID.");
        }
    }
}