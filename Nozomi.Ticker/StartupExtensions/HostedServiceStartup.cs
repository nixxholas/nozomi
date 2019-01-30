using Microsoft.Extensions.DependencyInjection;
using Nozomi.Service.HostedServices.RequestTypes;
using Nozomi.Service.Identity.HostedServices;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class HostedServiceStartup
    {
        public static void ConfigureHostedServices(this IServiceCollection services)
        {
            // Auth Hosted Services
            services.AddHostedService<ApiTokenCachingService>();
            
            // Hosted Services
            services.AddHostedService<HttpGetCurrencyPairRequestSyncingService>();
            services.AddHostedService<HttpPostCurrencyPairRequestSyncingService>();
            services.AddHostedService<WebsocketCurrencyPairRequestSyncingService>();
        }
    }
}