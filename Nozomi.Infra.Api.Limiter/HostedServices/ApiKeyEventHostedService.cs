using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using StackExchange.Redis;

namespace Nozomi.Infra.Api.Limiter.HostedServices
{
    /// <summary>
    /// UnrecordedApiKeyEventHostedService
    ///
    /// Maintains the UnrecordedApiKeyEvents Redis Cache.
    /// 1. Pops the cached items as soon as possible
    /// 2. Processes the user claims for quota usage with the popped items
    ///
    /// This hostedservices' job is never to 'ban' the user. Its primary job is to increase the usage unconditionally.
    /// Once the usage exceeds the quota, ApiKeyUserHostedService's job is to sort that out.
    /// </summary>
    public class ApiKeyEventHostedService : BaseHostedService<ApiKeyEventHostedService>
    {
        public ApiKeyEventHostedService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
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
                    // Redis connect!
                    var connectionMultiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
                    var endpoints = connectionMultiplexer.GetEndPoints();

                    // Iterate all keys
                    foreach (var apiKey in connectionMultiplexer.GetServer(endpoints[0])
                        .Keys((int) RedisDatabases.ApiKeyEvents))
                    {
                        var database = connectionMultiplexer.GetDatabase((int) RedisDatabases.ApiKeyEvents);
                        // Pop the elements from the left
                        var oldestWeight = database.ListLeftPop(apiKey);

                        // If it is a valid value
                        if (oldestWeight.HasValue && long.TryParse(oldestWeight, out var weight) && weight > 0)
                        {
                            // First navigate to the user first
                            var nozomiRedisEvent = scope.ServiceProvider.GetRequiredService<INozomiRedisEvent>();
                            var userKey = nozomiRedisEvent.GetValue(apiKey,
                                RedisDatabases.ApiKeyUser);

                            // Since we got the user's key,
                            if (!userKey.IsNullOrEmpty)
                            {
                                var authDbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

                                // Obtain the user's quota
                                var userQuota = authDbContext.UserClaims
                                    .AsTracking()
                                    .SingleOrDefault(uc => uc.UserId.Equals(userKey)
                                                           && uc.ClaimType.Equals(NozomiJwtClaimTypes.UserQuota));

                                // Since quota does not exist for the current user,
                                if (userQuota == null)
                                {
                                    // Create the quota claims for the current user
                                    userQuota = new UserClaim
                                    {
                                        UserId = userKey,
                                        ClaimType = NozomiJwtClaimTypes.UserQuota,
                                        ClaimValue = "0"
                                    };
                                    authDbContext.UserClaims.Add(userQuota);
                                    authDbContext.SaveChanges();
                                    
                                    // Let logs know as well
                                    _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: 0 value " +
                                                           $"quota created for user {userKey}");
                                }
                                
                                // Ensure quota exists and that 
                                else if (long.TryParse(userQuota.ClaimValue, out var quotaValue) && quotaValue > 0)
                                {
                                    var userUsage = authDbContext.UserClaims
                                        .AsTracking() // Ensure we track to modify directly
                                        .SingleOrDefault(uc => uc.UserId.Equals(userKey)
                                                               && uc.ClaimType.Equals(NozomiJwtClaimTypes.UserUsage));

                                    if (userUsage == null) // Since it's null, we'll populate a new one for him
                                    {
                                        userUsage = new UserClaim
                                        {
                                            UserId = userKey,
                                            ClaimType = NozomiJwtClaimTypes.UserUsage,
                                            ClaimValue = "0"
                                        };
                                        authDbContext.UserClaims.Add(userUsage);
                                        authDbContext.SaveChanges();
                                    }

                                    // Ensure that the quota is above the usage 
                                    if (long.TryParse(userUsage.ClaimValue, out var usageValue) 
                                        && usageValue < quotaValue)
                                    {
                                        userUsage.ClaimValue = (usageValue + weight).ToString(); // Update it
                                        authDbContext.UserClaims.Update(userUsage);
                                        await authDbContext.SaveChangesAsync(stoppingToken);

                                        _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: User " +
                                                               $"{userUsage.UserId} with quota count updated to " +
                                                               $"{userUsage.ClaimValue}");
                                    }
                                    else // Usage is bad
                                    {
                                        _logger.LogWarning($"{_hostedServiceName} ExecuteAsync: Quota of " +
                                                           $" {quotaValue} has exceeded usage for user {userUsage.UserId}");
                                    }
                                }
                                else // User quota not found, bad bad bad
                                {
                                    _logger.LogWarning($"{_hostedServiceName} ExecuteAsync: Quota for user " +
                                                       $"{userKey} not found.");
                                }
                            }
                            else // API Key is not linked to a User, bad bad bad
                            {
                                _logger.LogWarning($"{_hostedServiceName}: ExecuteAsync: Invalid API Key.." +
                                                   $" Key: {apiKey}");
                            }
                        }
                        else // Weight is not a valid integer/long..
                        {
                            _logger.LogWarning($"{_hostedServiceName} ExecuteAsync: Invalid event weight " +
                                               $"for API Key {apiKey} with a weight of {oldestWeight}");
                        }
                    }
                }

                await Task.Delay(250, stoppingToken);
            }

            _logger.LogCritical($"{_hostedServiceName} has broken out of its loop.");
        }
    }
}