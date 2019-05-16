using Microsoft.Extensions.DependencyInjection;
using Nozomi.Infra.Analysis.Service.HostedServices;
using Nozomi.Service.HostedServices;
using Nozomi.Service.HostedServices.RequestTypes;
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
            
            // Hosted Services
            services.AddHostedService<HttpGetRequestSyncingService>();
            services.AddHostedService<HttpPostCurrencyPairRequestSyncingService>();
            services.AddHostedService<NozomiStreamHubHostedService>();
            services.AddHostedService<WebsocketCurrencyPairRequestSyncingService>();
            services.AddHostedService<SourceSyncingService>();
            
            // TODO: Microservice.
            // Nozomi.Analysis Hosted Services
            services.AddHostedService<ComponentAnalysisService>();
        }
    }
}