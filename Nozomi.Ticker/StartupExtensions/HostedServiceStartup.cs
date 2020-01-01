using Microsoft.Extensions.DependencyInjection;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class HostedServiceStartup
    {
        public static void ConfigureHostedServices(this IServiceCollection services)
        {
            // Auth Hosted Services
            // services.AddHostedService<NozomiStreamHubHostedService>();
            // services.AddHostedService<SourceSyncingService>();
        }
    }
}