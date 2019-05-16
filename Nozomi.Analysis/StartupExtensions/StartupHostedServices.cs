using Microsoft.Extensions.DependencyInjection;
using Nozomi.Infra.Analysis.Service.HostedServices;
using Nozomi.Service.HostedServices;

namespace Nozomi.Analysis.StartupExtensions
{
    public static class StartupHostedServices
    {
        public static void ConfigureHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<ComponentAnalysisService>();
        }
    }
}