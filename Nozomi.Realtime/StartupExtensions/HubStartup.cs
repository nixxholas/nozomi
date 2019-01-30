using Microsoft.Extensions.DependencyInjection;
using Nozomi.Realtime.Infra.Service.Hubs.Server;
using Nozomi.Realtime.Infra.Service.Hubs.Server.Interfaces;

namespace Nozomi.Realtime.StartupExtensions
{
    public static class HubStartup
    {
        public static void ConfigureHubs(this IServiceCollection services)
        {
            services.AddSignalR();
            
            services.AddScoped<ITickerHubServer, TickerHubServer>();
        }
    }
}