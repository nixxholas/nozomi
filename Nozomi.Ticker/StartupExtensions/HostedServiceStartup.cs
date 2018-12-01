using Microsoft.Extensions.DependencyInjection;
using Nozomi.Service.HostedServices.RequestTypes;
using Nozomi.Service.HostedServices.StaticUpdater;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class HostedServiceStartup
    {
        public static void ConfigureHostedServices(this IServiceCollection services)
        {
            // Hosted Services
            services.AddHostedService<HttpGetCurrencyPairRequestSyncingService>();
            services.AddHostedService<HttpPostCurrencyPairRequestSyncingService>();
            services.AddHostedService<CPDSyncingService>();
            services.AddHostedService<CSSSyncingService>();
            services.AddHostedService<TSDSyncingService>();
        }
    }
}