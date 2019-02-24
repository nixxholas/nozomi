using Microsoft.Extensions.DependencyInjection;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Preprocessing.Events;
using Nozomi.Preprocessing.Events.Interfaces;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Analysis;
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

            services.AddScoped<IAnalysedComponentEvent, AnalysedComponentEvent>();
            services.AddScoped<ICurrencyEvent, CurrencyEvent>();
            services.AddScoped<ICurrencyTypeEvent, CurrencyTypeEvent>();
            services.AddScoped<IHistoricalDataEvent, HistoricalDataEvent>();
            services.AddScoped<ISourceEvent, SourceEvent>();
            services.AddScoped<ITickerEvent, TickerEvent>();
            services.AddScoped<IWebsocketRequestEvent, WebsocketRequestEvent>();
        }
    }
}