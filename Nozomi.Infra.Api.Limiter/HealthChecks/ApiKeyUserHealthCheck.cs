using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Nozomi.Infra.Api.Limiter.HealthChecks
{
    public class ApiKeyUserHealthCheck : IHealthCheck
    {
        private volatile bool _startupTaskCompleted = false;

        public string Name => "slow_dependency_check";

        public bool StartupTaskCompleted
        {
            get => _startupTaskCompleted;
            set => _startupTaskCompleted = value;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (StartupTaskCompleted)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("ApiKeyUserHostedService is complete."));
            }

            return Task.FromResult(
                HealthCheckResult.Unhealthy("ApiKeyUserHostedService is still running."));
        }
    }
}