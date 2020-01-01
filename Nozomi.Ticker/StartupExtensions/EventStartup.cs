using Microsoft.Extensions.DependencyInjection;
using Nozomi.Infra.Admin.Service.Events;
using Nozomi.Infra.Admin.Service.Events.Interfaces;
using Nozomi.Infra.Analysis.Service.Events;
using Nozomi.Infra.Analysis.Service.Events.Interfaces;
using Nozomi.Preprocessing.Events;
using Nozomi.Preprocessing.Events.Interfaces;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Analysis;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Enumerators;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class EventStartup
    {
        public static void ConfigureEvents(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SmsSender>();

            services.AddScoped<IAnalysedComponentEvent, AnalysedComponentEvent>();
            services.AddScoped<IComponentTypeEvent, ComponentTypeEvent>();
            services.AddScoped<ICurrencyEvent, CurrencyEvent>();
            services.AddScoped<ICurrencyPairEvent, CurrencyPairEvent>();
            services.AddScoped<ICurrencyPairTypeEvent, CurrencyPairTypeEvent>();
            services.AddScoped<ICurrencyPropertyAdminEvent, CurrencyPropertyAdminEvent>();
            services.AddScoped<ICurrencyTypeAdminEvent, CurrencyTypeAdminEvent>();
            services.AddScoped<IRequestEvent, RequestEvent>();
            services.AddScoped<IComponentEvent, ComponentEvent>();
            services.AddScoped<IRequestPropertyTypeEvent, RequestPropertyTypeEvent>();
            services.AddScoped<IRequestTypeEvent, RequestTypeEvent>();
            services.AddScoped<ISourceEvent, SourceEvent>();
            services.AddScoped<ITickerEvent, TickerEvent>();
            
            // TODO: Microservices
            // Nozomi.Analysis event injections
            services.AddScoped<IAnalysedComponentEvent, AnalysedComponentEvent>();
            services.AddScoped<IAnalysedHistoricItemEvent, AnalysedHistoricItemEvent>();
            services.AddScoped<ICurrencyConversionEvent, CurrencyConversionEvent>();
            
            // Admin
            services.AddScoped<ICurrencyAdminEvent, CurrencyAdminEvent>();
            services.AddScoped<ICurrencyPairAdminEvent, CurrencyPairAdminEvent>();
            services.AddScoped<ICurrencyPairSourceCurrencyAdminEvent, CurrencyPairSourceCurrencyAdminEvent>();
        }
    }
}