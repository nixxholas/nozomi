using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
                    var redisEvent = scope.ServiceProvider.GetRequiredService<INozomiRedisEvent>();
                    
                    // Obtain all users with their API keys, quota limit and quota usage.
                    var userClaims = authDbContext.UserClaims
                        .Where(uc => uc.ClaimType.Equals(NozomiJwtClaimTypes.ApiKeys)
                                     || uc.ClaimType.Equals(NozomiJwtClaimTypes.UserQuota)
                                     || uc.ClaimType.Equals(NozomiJwtClaimTypes.UserUsage));
                    
                    // Ensure that there's a userclaim to perform these actions
                    if (userClaims.Any())
                    {
                        // Iterate every quota first.
                        foreach (var userQuotaClaim in userClaims
                            .Where(uc => uc.ClaimType.Equals(NozomiJwtClaimTypes.UserQuota)))
                        {
                            if (string.IsNullOrEmpty(userQuotaClaim.UserId) // Ensure user id is existent
                                // Then ensure that the quota value is long-able
                                || !long.TryParse(userQuotaClaim.ClaimValue, out var userQuota))
                            {
                                
                            }
                            else // Else, it falls within the criteria for it to be processed    
                            {
                                // Look for the user's current usage
                                var userUsageClaim = userClaims
                                    .FirstOrDefault(uc => uc.UserId.Equals(userQuotaClaim.UserId));
                            }

                        }
                    }
                    
                    // Add the new key entries first

                    // Update keys with exceeded quotas

                    // DONE!

                    // // Obtain all banned users
                    // var bannedUsers = redisEvent
                    //     .AllKeys(RedisDatabases.BlockedUserApiKeys)
                    //     .DefaultIfEmpty()
                    //     .ToList();
                    //
                    // if (bannedUsers.Any())
                    // {
                    //     var redisService = scope.ServiceProvider.GetRequiredService<INozomiRedisService>();
                    //     
                    //     // Iterate every banned user
                    //     foreach (var bannedUser in bannedUsers)
                    //     {
                    //         // Remove the token if it exists
                    //         redisService.Remove(RedisDatabases.ApiKeyUser, bannedUser);
                    //     }
                    // }
                    //
                    // // Obtain all currently active user tokens
                    // var userTokens = authDbContext.UserClaims
                    //     .Where(uc => !bannedUsers.Contains(uc.UserId) // Ensure the user is not banned
                    //                  && uc.ClaimType.Equals(NozomiJwtClaimTypes.ApiKeys)); // Obtain only API Keys
                    //
                    // // Iterate every token
                    // foreach (var userToken in userTokens)
                    // {
                    //     // Add the token if it doesn't exist
                    // }
                }
            }
            
            _logger.LogWarning($"{_hostedServiceName} has broken out of its loop.");
        }
    }
}