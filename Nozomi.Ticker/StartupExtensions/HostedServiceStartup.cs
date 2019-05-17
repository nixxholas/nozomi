using Microsoft.Extensions.DependencyInjection;
using Nozomi.Infra.Analysis.Service.HostedServices;
using Nozomi.Infra.Analysis.Service.HostedServices.RequestTypes;
using Nozomi.Service.HostedServices;
using Nozomi.Service.HostedServices.StaticUpdater;
using Nozomi.Service.Identity.HostedServices;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class HostedServiceStartup
    {
        public static void ConfigureHostedServices(this IServiceCollection services)
        {
            // Auth Hosted Services
            services.AddHostedService<ApiTokenCachingService>();
            
            services.AddHostedService<NozomiStreamHubHostedService>();
            services.AddHostedService<SourceSyncingService>();
        }
    }
}