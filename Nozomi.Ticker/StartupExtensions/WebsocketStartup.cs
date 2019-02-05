using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Infra.Websocket.Extensions;
using Nozomi.Infra.Websocket.Handlers;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class WebsocketStartup
    {
        public static void AddNozomiSockets(this IServiceCollection services)
        {
            services.AddWebSocketManager();
        }

        public static void UseNozomiSockets(this IApplicationBuilder app)
        {
            app.UseWebSockets();

            app.MapWebSocketManager("/tickers", app.ApplicationServices.GetService<BroadcastHandler>());
        }
    }
}