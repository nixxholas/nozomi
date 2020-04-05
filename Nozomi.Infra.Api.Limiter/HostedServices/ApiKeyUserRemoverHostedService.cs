using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Infra.Api.Limiter.Services.Interfaces;
using Nozomi.Infra.Auth.Events.UserEvent;
using Nozomi.Infra.Auth.Services.QuotaClaim;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.Statics;
using Nozomi.Repo.Auth.Data;

namespace Nozomi.Infra.Api.Limiter.HostedServices
{
    /// <summary>
    /// ApiKeyUserRemoverHostedService
    ///
    /// Maintains the removing logic for the ApiKeyUser Redis cache.
    /// 1. Removes a key's value if it has reached its quota
    /// </summary>
    public class ApiKeyUserRemoverHostedService : BaseHostedService<ApiKeyUserRemoverHostedService>
    {
        public ApiKeyUserRemoverHostedService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{_hostedServiceName} is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var authDbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

                        // Obtain all users who have API keys
                        var users = authDbContext.Users.AsNoTracking()
                            .Include(u => u.UserClaims)
                            .Include(u => u.ApiKeys)
                            .Where(u => u.ApiKeys.Any());

                        // Iterate every user
                        foreach (var user in users)
                        {
                            var redisService =
                                scope.ServiceProvider.GetRequiredService<INozomiRedisService>();
                            var userEvent = scope.ServiceProvider.GetRequiredService<IUserEvent>();

                            // Ensure the user is not a staff member
                            if (!userEvent.IsInRoles(user.Id,
                                NozomiPermissions.AllowAllStaffRoles.Split(", ")))
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

                                // ======================== SCENARIO 1: NO USAGE OR QUOTA CLAIMS ======================== //
                                // If the user has invalid quota or usage counts,
                                if (quotaClaim == null || usageClaim == null)
                                {
                                    _logger.LogInformation($"{_hostedServiceName} User does not have a " +
                                                           $"proper quota and/or usage claim!");

                                    foreach (var userApiKey in user.ApiKeys) // BAN!!
                                    {
                                        redisService.Remove(RedisDatabases.ApiKeyUser, userApiKey.Value);
                                        _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: " +
                                                               $" API Key {userApiKey.Value} removed for " +
                                                               $"user {user.Id} in Redis.");
                                    }
                                }
                                // ======================== SCENARIO 2: EXCEEDED USAGE ======================== //
                                else if (quotaClaim != null && usageClaim != null // If the claims ain't null 
                                                            && long.TryParse(quotaClaim.ClaimValue,
                                                                out var quotaCount)
                                                            && long.TryParse(usageClaim.ClaimValue,
                                                                out var usageCount)
                                                            // But the usage exceeds the quota
                                                            && usageCount >= quotaCount)
                                {
                                    _logger.LogInformation(
                                        $"{_hostedServiceName} ExecuteAsync: User {user.Id}" +
                                        $" has exceeded his usage by {usageCount - quotaCount}. Ban time!");

                                    foreach (var userApiKey in user.ApiKeys) // BAN!!
                                    {
                                        redisService.Remove(RedisDatabases.ApiKeyUser, userApiKey.Value);
                                        _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: " +
                                                               $" API Key {userApiKey.Value} removed for " +
                                                               $"user {user.Id} in Redis.");
                                    }
                                }
                                // ======================== SCENARIO 3: ALL IN THE CLEAR ======================== //
                                else
                                {
                                    // User is still valid for API consumption
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"{_hostedServiceName} Critical! : {ex}");
                }

                await Task.Delay(100, stoppingToken);
            }

            _logger.LogWarning($"{_hostedServiceName} has broken out of its loop.");
        }
    }
}