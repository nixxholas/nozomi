using Microsoft.Extensions.DependencyInjection;
using Nozomi.Infra.Admin.Service.Events;
using Nozomi.Infra.Admin.Service.Events.Interfaces;
using Nozomi.Infra.Analysis.Service.Events;
using Nozomi.Infra.Analysis.Service.Events.Interfaces;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Analysis;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Analysis.StartupExtensions
{
    public static class StartupEvents
    {
        public static void ConfigureEvents(this IServiceCollection services)
        {
            services.AddScoped<IComponentTypeEvent, ComponentTypeEvent>();
            services.AddScoped<ICurrencyEvent, CurrencyEvent>();
            services.AddScoped<ICurrencyPairEvent, CurrencyPairEvent>();
            services.AddScoped<ICurrencyTypeAdminEvent, CurrencyTypeAdminEvent>();
            services.AddScoped<IRequestEvent, RequestEvent>();
            services.AddScoped<IRequestComponentEvent, RequestComponentEvent>();
            services.AddScoped<ISourceEvent, SourceEvent>();
            services.AddScoped<ITickerEvent, TickerEvent>();
            
            // Nozomi.Analysis event injections
            services.AddScoped<IAnalysedComponentEvent, AnalysedComponentEvent>();
            services.AddScoped<IAnalysedHistoricItemEvent, AnalysedHistoricItemEvent>();
            services.AddScoped<ICurrencyConversionEvent, CurrencyConversionEvent>();
            services.AddScoped<IXAnalysedComponentEvent, XAnalysedComponentEvent>();
        }
    }
}