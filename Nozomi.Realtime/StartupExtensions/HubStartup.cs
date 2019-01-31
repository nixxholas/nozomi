using Microsoft.Extensions.DependencyInjection;

namespace Nozomi.Realtime.StartupExtensions
{
    public static class HubStartup
    {
        public static void ConfigureHubs(this IServiceCollection services)
        {
            services.AddSignalR()
                .AddMessagePackProtocol();
        }
    }
}