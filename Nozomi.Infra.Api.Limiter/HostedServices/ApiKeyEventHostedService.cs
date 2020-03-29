using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.Options;
using Nozomi.Preprocessing.Singleton;
using Nozomi.Repo.Auth.Data;
using Npgsql;
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
        private readonly IOptions<NozomiRedisCacheOptions> _options;
        
        public ApiKeyEventHostedService(IServiceScopeFactory scopeFactory, IOptions<NozomiRedisCacheOptions> options) 
            : base(scopeFactory)
        {
            _options = options;
        }

        private void ClearRedisCache(IConnectionMultiplexer connectionMultiplexer)
        {
            var endpoints = connectionMultiplexer.GetEndPoints();

            if (endpoints.Any())
            {
                
            }
            else
            {
                _logger.LogWarning($"{_hostedServiceName} ClearRedisCache: No Redis database endpoints available..");
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{_hostedServiceName} is starting.");

            stoppingToken.Register(() => _logger.LogInformation($"{_hostedServiceName} is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        // Redis connect!
                        var nozomiRedisEvent = scope.ServiceProvider.GetRequiredService<INozomiRedisEvent>();

                        // Iterate all keys
                        foreach (var apiKey in nozomiRedisEvent.AllKeys(RedisDatabases.ApiKeyUser))
                        {
                            var database = nozomiRedisEvent.GetDatabase(RedisDatabases.ApiKeyEvents);
                            // Pop the elements from the left
                            var oldestWeight = database.ListLeftPop(apiKey);

                            // If it is a valid value
                            if (oldestWeight.HasValue && long.TryParse(oldestWeight, out var weight) && weight >= 0)
                            {
                                // First navigate to the user first
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

                                    // But if the user's quota exists and that the value is legit,
                                    if (long.TryParse(userQuota.ClaimValue, out var quotaValue) && quotaValue >= 0)
                                    {
                                        var userUsage = authDbContext.UserClaims
                                            .AsTracking() // Ensure we track to modify directly
                                            .SingleOrDefault(uc => uc.UserId.Equals(userKey)
                                                                   && uc.ClaimType.Equals(NozomiJwtClaimTypes
                                                                       .UserUsage));

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

                                        // Ensure that the usage is a number 
                                        if (long.TryParse(userUsage.ClaimValue, out var usageValue))
                                        {
                                            userUsage.ClaimValue = (usageValue + weight).ToString(); // Update it
                                            authDbContext.UserClaims.Update(userUsage);
                                            await authDbContext.SaveChangesAsync(stoppingToken);

                                            if (usageValue < quotaValue) // Quota is below the usage
                                                _logger.LogWarning($"{_hostedServiceName} ExecuteAsync: Quota of " +
                                                                   $" {quotaValue} has exceeded usage for user " +
                                                                   $"{userUsage.UserId}"); // Bad boy, he gon get removed

                                            _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: User " +
                                                                   $"{userUsage.UserId} with quota count updated to " +
                                                                   $"{userUsage.ClaimValue}");
                                        }
                                    }
                                    else // User quota is bad bad bad, might be below 0...
                                    {
                                        _logger.LogWarning($"{_hostedServiceName} ExecuteAsync: Quota for user " +
                                                           $"{userKey} is bad [VALUE: {userQuota.ClaimValue}].");
                                    }
                                }
                                else // API Key is not linked to a User, bad bad bad
                                {
                                    _logger.LogWarning($"{_hostedServiceName}: ExecuteAsync: Invalid API Key.." +
                                                       $" Key: {apiKey}");
                                }
                            }
                            else // Weight is not a valid integer/long or no value yet.
                            {
                                if (oldestWeight.HasValue && !oldestWeight.IsNullOrEmpty)
                                    _logger.LogWarning($"{_hostedServiceName} ExecuteAsync: Invalid event " +
                                                       $"weight for API Key {apiKey} with a weight of {oldestWeight}");
                            }
                        }
                    }

                    await Task.Delay(250, stoppingToken);
                }
                catch (RedisServerException rse)
                {
                    _logger.LogError($"{_hostedServiceName} ExecuteAsync (REDIS ERROR): {rse}");
                }
                catch (NpgsqlException npgsqlException)
                {
                    _logger.LogCritical($"{_hostedServiceName} PSQL Error: {npgsqlException.Message}");
                }
            }

            _logger.LogCritical($"{_hostedServiceName} has broken out of its loop.");
        }
    }
}