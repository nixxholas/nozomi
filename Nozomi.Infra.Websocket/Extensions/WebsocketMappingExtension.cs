using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Infra.Websocket.Managers;
using Nozomi.Infra.Websocket.Middlewares;

namespace Nozomi.Infra.Websocket.Extensions
{
    public static class WebsocketMappingExtension
    {
        public static void MapWebSocketManager(this IApplicationBuilder app)
        {
            app.UseMiddleware<NozomiSocketMiddleware>();
        }
        
        public static void AddWebSocketManager(this IServiceCollection services)
        {
            services.AddSingleton<NozomiSocketManager>();
        }
    }
}