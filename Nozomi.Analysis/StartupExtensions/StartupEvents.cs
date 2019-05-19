using Microsoft.Extensions.DependencyInjection;
using Nozomi.Infra.Analysis.Service.Events.Analysis;
using Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Events.Websocket;
using Nozomi.Service.Events.Websocket.Interfaces;

namespace Nozomi.Analysis.StartupExtensions
{
    public static class StartupEvents
    {
        public static void ConfigureEvents(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyEvent, CurrencyEvent>();
            services.AddScoped<ICurrencyPairEvent, CurrencyPairEvent>();
            services.AddScoped<ICurrencyRequestEvent, CurrencyRequestEvent>();
            services.AddScoped<ICurrencyTypeEvent, CurrencyTypeEvent>();
            services.AddScoped<IRequestEvent, RequestEvent>();
            services.AddScoped<IRequestComponentEvent, RequestComponentEvent>();
            services.AddScoped<ISourceEvent, SourceEvent>();
            services.AddScoped<ITickerEvent, TickerEvent>();
            services.AddScoped<IWebsocketRequestEvent, WebsocketRequestEvent>();
            
            // Nozomi.Analysis event injections
            services.AddScoped<IAnalysedComponentEvent, AnalysedComponentEvent>();
            services.AddScoped<IAnalysedHistoricItemEvent, AnalysedHistoricItemEvent>();
            services.AddScoped<ICurrencyConversionEvent, CurrencyConversionEvent>();
        }
    }
}