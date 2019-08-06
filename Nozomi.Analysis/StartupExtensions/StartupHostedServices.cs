using Microsoft.Extensions.DependencyInjection;
using Nozomi.Infra.Analysis.Service.HostedServices;
using Nozomi.Infra.Analysis.Service.HostedServices.RequestTypes;
using Nozomi.Service.HostedServices;

namespace Nozomi.Analysis.StartupExtensions
{
    public static class StartupHostedServices
    {
        public static void ConfigureHostedServices(this IServiceCollection services)
        {
            // RequestComponent Asyncs
            services.AddHostedService<HttpGetRequestSyncingService>();
            // services.AddHostedService<HttpPostCurrencyPairRequestSyncingService>();
            services.AddHostedService<WebsocketCurrencyPairRequestSyncingService>();
            
            services.AddHostedService<AcAnalysisHostedService>();
        }
    }
}