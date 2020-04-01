using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Nozomi.Preprocessing.Publishers
{
    public class LivenessPublisher : IHealthCheckPublisher
    {
        private readonly ILogger _logger;

        public LivenessPublisher(ILogger<LivenessPublisher> logger)
        {
            _logger = logger;
        }

        // TODO: More checks?
        // The following example is for demonstration purposes only. Health Checks 
        // Middleware already logs health checks results. A real-world readiness 
        // check in a production app might perform a set of more expensive or 
        // time-consuming checks to determine if other resources are responding 
        // properly.
        public Task PublishAsync(HealthReport report, 
            CancellationToken cancellationToken)
        {
            switch (report.Status)
            {
                case HealthStatus.Healthy:
                    _logger.LogInformation("{Timestamp} Readiness Probe Status: {Result}", 
                        DateTime.UtcNow, report.Status);
                    break;
                case HealthStatus.Degraded:
                    _logger.LogWarning("{Timestamp} Readiness Probe Status: {Result}", 
                        DateTime.UtcNow, report.Status);
                    break;
                case HealthStatus.Unhealthy:
                    _logger.LogError("{Timestamp} Readiness Probe Status: {Result}", 
                        DateTime.UtcNow, report.Status);
                    break;
            }

            cancellationToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;
        }
    }
}