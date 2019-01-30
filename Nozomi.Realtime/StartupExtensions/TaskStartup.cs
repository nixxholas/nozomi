using Microsoft.Extensions.DependencyInjection;
using Nozomi.Realtime.Infra.Service.HostedServices;

namespace Nozomi.Realtime.StartupExtensions
{
    public static class TaskStartup
    {
        public static void ConfigureTasks(this IServiceCollection services)
        {
            services.AddHostedService<CPDSyncingService>();
            services.AddHostedService<CSSSyncingService>();
            services.AddHostedService<TSDSyncingService>();
        }
    }
}