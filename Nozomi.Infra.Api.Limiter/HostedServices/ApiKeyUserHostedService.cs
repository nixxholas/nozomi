using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Infra.Api.Limiter.Services.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;

namespace Nozomi.Infra.Api.Limiter.HostedServices
{
    /// <summary>
    /// ApiKeyUserRedisHostedService
    ///
    /// Maintains the ApiKeyUser Redis cache.
    /// 1. Removes a key's value if it has reached its quota
    /// 2. Adds new key entries.
    /// </summary>
    public class ApiKeyUserHostedService : BaseHostedService<ApiKeyUserHostedService>
    {
        public ApiKeyUserHostedService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{_hostedServiceName} is starting.");

            stoppingToken.Register(() => _logger.LogInformation($"{_hostedServiceName} is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var authDbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

                    // Obtain all users who have API keys
                    var users = authDbContext.Users
                        .AsNoTracking()
                        .Include(u => u.UserClaims)
                        .Where(u => u.UserClaims
                            .Any(uc => uc.ClaimType.Equals(NozomiJwtClaimTypes.ApiKeys)));

                    // Ensure that there's any users before iterating
                    if (users.Any())
                    {
                        // Iterate every user
                        foreach (var user in users)
                        {
                            // Obtain the user's quota and usage
                            var requiredUserClaims = authDbContext.UserClaims
                                .AsNoTracking()
                                .Where(uc => uc.UserId.Equals(user.Id)
                                             && (uc.ClaimType.Equals(NozomiJwtClaimTypes.UserQuota)
                                                 || uc.ClaimType.Equals(NozomiJwtClaimTypes.UserUsage)));

                            // Quota
                            var quotaClaim = requiredUserClaims.SingleOrDefault(uc =>
                                uc.ClaimType.Equals(NozomiJwtClaimTypes.UserQuota));

                            // Usage
                            var usageClaim = requiredUserClaims.SingleOrDefault(uc =>
                                uc.ClaimType.Equals(NozomiJwtClaimTypes.UserUsage));

                            // Safetynet
                            if (quotaClaim != null && usageClaim != null && long.TryParse(quotaClaim.ClaimValue,
                                out var quota) && long.TryParse(usageClaim.ClaimValue, out var usage))
                            {
                                // Obtain the Api Keys first
                                var userApiKeys = authDbContext.UserClaims
                                    .AsNoTracking()
                                    .Where(uc => uc.ClaimType.Equals(NozomiJwtClaimTypes.ApiKeys));

                                if (userApiKeys.Any()) // Any API keys?
                                {
                                    // Check if quota and usage has exceeded the limits
                                    if (usage > quota) // Limit reached, bar user from usage.
                                    {
                                        // =========================== REMOVAL LOGIC FIRST =========================== //

                                        _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: User {user.Id}" +
                                                               $" has exceeded his usage by {usage - quota}. Ban time!");

                                        var nozomiRedisService =
                                            scope.ServiceProvider.GetRequiredService<INozomiRedisService>();
                                        foreach (var userApiKey in userApiKeys) // BAN!!
                                        {
                                            nozomiRedisService.Remove(RedisDatabases.ApiKeyUser, userApiKey.ClaimValue);
                                            _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: " +
                                                                   $" API Key {userApiKey.ClaimValue} removed for " +
                                                                   $"user {user.Id} in Redis.");
                                        }
                                    }
                                    else // Limit not reached, ensure API keys exist
                                    {
                                        // =========================== ADDITION LOGIC =========================== //

                                        var redisEvent = scope.ServiceProvider.GetRequiredService<INozomiRedisEvent>();
                                        var redisService =
                                            scope.ServiceProvider.GetRequiredService<INozomiRedisService>();

                                        // Iterate the user's api keys and populate the cache if needed
                                        foreach (var userApiKey in userApiKeys)
                                        {
                                            if (redisEvent.Exists(userApiKey.ClaimValue)) continue;
                                            // Add it into the cache
                                            redisService.Add(RedisDatabases.ApiKeyUser, userApiKey.ClaimValue, 
                                                user.Id);
                                            _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: " +
                                                                   $" Api Key {userApiKey.ClaimValue} added with " +
                                                                   $"symlink to user {user.Id}");
                                        }
                                    }
                                }
                                else // Nope, warn!!!
                                {
                                    _logger.LogWarning($"{_hostedServiceName} ExecuteAsync: wait, " +
                                                       $"peculiar event, user {user.Id} has no API keys but has " +
                                                       $"hit his limit of {quota}..");
                                }
                            }
                            else
                            {
                                _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: No usage and/or " +
                                                       $"quota found for user {user.Id}.");
                            }
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: Apparently, no users to " +
                                               "check!");
                    }
                }
            }

            _logger.LogWarning($"{_hostedServiceName} has broken out of its loop.");
        }
    }
}