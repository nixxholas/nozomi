using Microsoft.Extensions.DependencyInjection;
using Nozomi.Infra.Blockchain.Auth.Events;
using Nozomi.Infra.Blockchain.Auth.Events.Interfaces;
using Nozomi.Preprocessing.Events;
using Nozomi.Preprocessing.Events.Interfaces;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Analysis;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Events.Auth;
using Nozomi.Service.Identity.Events.Auth.Interfaces;
using Nozomi.Service.Services.Enumerators;

namespace Nozomi.Web.StartupExtensions
{
    public static class EventStartup
    {
        public static void ConfigureEvents(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SmsSender>();

            // Auth Events
            services.AddScoped<IValidatingEvent, ValidatingEvent>();

            services.AddScoped<IAnalysedComponentEvent, AnalysedComponentEvent>();
            services.AddScoped<IAnalysedHistoricItemEvent, AnalysedHistoricItemEvent>();
            services.AddScoped<IApiTokenEvent, ApiTokenEvent>();
            services.AddScoped<IComponentTypeEvent, ComponentTypeEvent>();
            services.AddScoped<ICurrencyEvent, CurrencyEvent>();
            services.AddScoped<ICurrencyPairEvent, CurrencyPairEvent>();
            services.AddScoped<ICurrencyPairTypeEvent, CurrencyPairTypeEvent>();
            services.AddScoped<ICurrencyTypeEvent, CurrencyTypeEvent>();
            services.AddScoped<IRequestEvent, RequestEvent>();
            services.AddScoped<IRequestComponentEvent, RequestComponentEvent>();
            services.AddScoped<IRequestPropertyTypeEvent, RequestPropertyTypeEvent>();
            services.AddScoped<IRequestTypeEvent, RequestTypeEvent>();
            services.AddScoped<ISourceEvent, SourceEvent>();
            services.AddScoped<ITickerEvent, TickerEvent>();
        }
    }
}
