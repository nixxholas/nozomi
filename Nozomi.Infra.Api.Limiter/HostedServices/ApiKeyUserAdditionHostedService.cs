using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public class ApiKeyUserAdditionHostedService : BaseHostedService<ApiKeyUserAdditionHostedService>
    {
        public ApiKeyUserAdditionHostedService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
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
                            .Include(u => u.ApiKeys)
                            .Where(u => u.ApiKeys.Any());

                            // Iterate every user
                            foreach (var user in users)
                            {
                                var redisEvent =
                                    scope.ServiceProvider.GetRequiredService<INozomiRedisEvent>();
                                var redisService =
                                    scope.ServiceProvider.GetRequiredService<INozomiRedisService>();
                                var quotaClaimService = scope.ServiceProvider
                                    .GetRequiredService<IQuotaClaimService>();

                                // Obtain the user's quota and usage
                                var requiredUserClaims = authDbContext.UserClaims
                                    .AsNoTracking()
                                    .Where(uc => uc.UserId.Equals(user.Id)
                                                 && (uc.ClaimType.Equals(NozomiJwtClaimTypes.UserQuota)
                                                     || uc.ClaimType.Equals(NozomiJwtClaimTypes.UserUsage)));

                                // Quota
                                var quotaClaim = requiredUserClaims.SingleOrDefault(uc =>
                                    uc.ClaimType.Equals(NozomiJwtClaimTypes.UserQuota));
                                
                                if (quotaClaim == null) // Populate the quota claim if it doesn't exist
                                {
                                    // If quota claim is null, set it
                                    quotaClaimService.SetQuota(user.Id, 0);
                                    _logger.LogInformation($"{_hostedServiceName} User: {user.Id} has no " +
                                                           $"quota claim, created one for him/her here.");

                                    quotaClaim = authDbContext.UserClaims
                                        .FirstOrDefault(uc => uc.UserId.Equals(user.Id) 
                                                              && uc.ClaimType.Equals(NozomiJwtClaimTypes.UserQuota));
                                }

                                // Usage
                                var usageClaim = requiredUserClaims.SingleOrDefault(uc =>
                                    uc.ClaimType.Equals(NozomiJwtClaimTypes.UserUsage));

                                if (usageClaim == null) // Populate the usage claim if it doesn't exist
                                {
                                    // If usage claim is null, set it
                                    quotaClaimService.AddUsage(user.Id, 0);
                                    _logger.LogInformation($"{_hostedServiceName} User: {user.Id} has no " +
                                                           $"usage claim, created one for him/her here.");

                                    usageClaim = authDbContext.UserClaims
                                        .FirstOrDefault(uc => uc.UserId.Equals(user.Id) 
                                                              && uc.ClaimType.Equals(NozomiJwtClaimTypes.UserUsage));
                                }

                                // Safety net, has valid quota and usage
                                if (quotaClaim != null && usageClaim != null
                                                       && long.TryParse(quotaClaim.ClaimValue, out var quota)
                                                       && long.TryParse(usageClaim.ClaimValue, out var usage))
                                {
                                    var userEvent = scope.ServiceProvider.GetRequiredService<IUserEvent>();
                                    var userIsStaff = userEvent.IsInRoles(user.Id, // Is user a staff member?
                                        NozomiPermissions.AllowAllStaffRoles.Split(", "));

                                    if (!userIsStaff) // Is user a staff? hopefully no
                                    {
                                        // Check if quota and usage has exceeded the limits
                                        if (usage > quota) // Limit reached, bar user from usage.
                                        {
                                            // =========================== REMOVAL LOGIC FIRST =========================== //

                                            _logger.LogInformation(
                                                $"{_hostedServiceName} ExecuteAsync: User {user.Id}" +
                                                $" has exceeded his usage by {usage - quota}. Ban time!");

                                            foreach (var userApiKey in user.ApiKeys) // BAN!!
                                            {
                                                redisService.Remove(RedisDatabases.ApiKeyUser, userApiKey.Value);
                                                _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: " +
                                                                       $" API Key {userApiKey.Value} removed for " +
                                                                       $"user {user.Id} in Redis.");
                                            }
                                        }
                                        else // Limit not reached, ensure API keys exist
                                        {
                                            // =========================== ADDITION LOGIC =========================== //

                                            // Iterate the user's api keys and populate the cache if needed
                                            foreach (var userApiKey in user.ApiKeys)
                                            {
                                                // If the cache does not contain this api key,
                                                if (!redisEvent.Exists(userApiKey.Value,
                                                    RedisDatabases.ApiKeyUser))
                                                {
                                                    // Add it into the cache
                                                    redisService.Add(RedisDatabases.ApiKeyUser, userApiKey.Value,
                                                        user.Id);
                                                    _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: " +
                                                                           $" Api Key {userApiKey.Value} added with " +
                                                                           $"symlink to user {user.Id}");
                                                }

                                                // Don't refactor this, may need it.. not sure when
                                                // else
                                                // {
                                                //     _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: " +
                                                //                            $" Api Key {userApiKey.Value} already added");
                                                // }
                                            }
                                        }
                                    }
                                    else // User is a staff..
                                    {
                                        // Iterate the user's api keys and populate the cache if needed
                                        foreach (var userApiKey in user.ApiKeys)
                                        {
                                            if (!redisEvent.Exists(userApiKey.Value,
                                                RedisDatabases.ApiKeyUser))
                                            {
                                                // Add it into the cache
                                                redisService.Add(RedisDatabases.ApiKeyUser, userApiKey.Value,
                                                    user.Id);
                                                _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: " +
                                                                       $" Staff Api Key {userApiKey.Value} added with " +
                                                                       $"symlink to Staff member {user.Id}");
                                            }
                                        }
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
                
#if DEBUG
            _logger.LogCritical($"{_hostedServiceName} is out of loop!!!");
#endif

            _logger.LogWarning($"{_hostedServiceName} has broken out of its loop.");
        }
    }
}