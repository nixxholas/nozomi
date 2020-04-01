using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Nozomi.Infra.Api.Limiter.HealthChecks
{
    public class ApiKeyUserHealthCheck : IHealthCheck
    {
        private volatile bool _apiKeyUserTaskCompleted = false;

        public string Name => "slow_dependency_check";

        public bool ApiKeyUserTaskCompleted
        {
            get => _apiKeyUserTaskCompleted;
            set => _apiKeyUserTaskCompleted = value;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (ApiKeyUserTaskCompleted)
            {
                return Task.FromResult(
                    HealthCheckResult.Unhealthy("ApiKeyUserHostedService has stopped."));
            }

            return Task.FromResult(
                HealthCheckResult.Healthy("ApiKeyUserHostedService is still running."));
        }
    }
}