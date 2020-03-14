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
                                // Check if quota and usage has exceeded the limits
                                if (usage > quota) // Limit reached, bar user from usage.
                                {
                                    // =========================== REMOVAL LOGIC FIRST =========================== //

                                    var userApiKeys = authDbContext.UserClaims
                                        .AsNoTracking()
                                        .Where(uc => uc.ClaimType.Equals(NozomiJwtClaimTypes.ApiKeys));

                                    _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: User {user.Id}" +
                                                           $" has exceeded his usage by {usage - quota}. Ban time!");

                                    if (userApiKeys.Any()) // Any API keys?
                                    {
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
                                    else // Nope, warn!!!
                                    {
                                        _logger.LogWarning($"{_hostedServiceName} ExecuteAsync: wait, " +
                                                           $"peculiar event, user {user.Id} has no API keys but has " +
                                                           $"hit his limit of {quota}..");
                                    }
                                }
                                else
                                {
                                    // =========================== ADDITION LOGIC =========================== //

                                    var redisEvent = scope.ServiceProvider.GetRequiredService<INozomiRedisEvent>();
                                    
                                }
                            }
                            else
                            {
                                _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: No usage and/or " +
                                                       $"quota found for user {user.Id}.");
                            }
                            
                            // if (string.IsNullOrEmpty(user.UserId) // Ensure user id is existent
                            //     // Then ensure that the quota value is long-able
                            //     || !long.TryParse(user.ClaimValue, out var userQuota))
                            // {
                            //     
                            // }
                            // else // Else, it falls within the criteria for it to be processed    
                            // {
                            //     // Look for the user's current usage
                            //     var userUsageClaim = users
                            //         .FirstOrDefault(uc => uc.UserId.Equals(user.UserId));
                            // }

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