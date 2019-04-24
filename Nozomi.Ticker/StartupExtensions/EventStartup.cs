using Microsoft.Extensions.DependencyInjection;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Analysis.Service.Events.Analysis;
using Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces;
using Nozomi.Preprocessing.Events;
using Nozomi.Preprocessing.Events.Interfaces;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Events.Memory;
using Nozomi.Service.Events.Memory.Interfaces;
using Nozomi.Service.Events.Websocket;
using Nozomi.Service.Events.Websocket.Interfaces;
using Nozomi.Service.Identity.Events;
using Nozomi.Service.Identity.Events.Auth;
using Nozomi.Service.Identity.Events.Auth.Interfaces;
using Nozomi.Service.Identity.Events.Interfaces;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class EventStartup
    {
        public static void ConfigureEvents(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SmsSender>();
            services.AddTransient<IStripeEvent, StripeEvent>();

            services.AddScoped<IApiTokenEvent, ApiTokenEvent>();
            
            services.AddScoped<ICurrencyEvent, CurrencyEvent>();
            services.AddScoped<ICurrencyPairEvent, CurrencyPairEvent>();
            services.AddScoped<ICurrencyRequestEvent, CurrencyRequestEvent>();
            services.AddScoped<ICurrencyTypeEvent, CurrencyTypeEvent>();
            services.AddScoped<IHistoricalDataEvent, HistoricalDataEvent>();
            services.AddScoped<ICurrencyCurrencyPairEvent, CurrencyCurrencyPairEvent>();
            services.AddScoped<IRequestEvent, RequestEvent>();
            services.AddScoped<IRequestComponentEvent, RequestComponentEvent>();
            services.AddScoped<ISourceEvent, SourceEvent>();
            services.AddScoped<ITickerEvent, TickerEvent>();
            services.AddScoped<IWebsocketRequestEvent, WebsocketRequestEvent>();
            
            // TODO: Microservices
            // Nozomi.Analysis event injections
            services.AddScoped<IAnalysedComponentEvent, AnalysedComponentEvent>();
            services.AddScoped<IAnalysedHistoricItemEvent, AnalysedHistoricItemEvent>();
            services.AddScoped<IAnalysedResponseEvent, AnalysedResponseEvent>();
            services.AddScoped<ICurrencyConversionEvent, CurrencyConversionEvent>();
        }
    }
}