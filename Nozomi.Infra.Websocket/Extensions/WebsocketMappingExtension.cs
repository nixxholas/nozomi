using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Infra.Websocket.Handlers;
using Nozomi.Infra.Websocket.Managers;
using Nozomi.Infra.Websocket.Managers.Interfaces;
using Nozomi.Infra.Websocket.Middlewares;

namespace Nozomi.Infra.Websocket.Extensions
{
    public static class WebsocketMappingExtension
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app, 
            PathString path,
            WebSocketHandler handler)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<WebSocketManagerMiddleware>(handler));
        }
        
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<IWebsocketConnectionManager, WebSocketConnectionManager>();

            foreach(var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if(type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }
    }
}