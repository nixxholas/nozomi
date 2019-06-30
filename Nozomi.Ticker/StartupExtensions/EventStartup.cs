using Microsoft.Extensions.DependencyInjection;
using Nozomi.Data.Models.Web.Analytical;
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
            services.AddScoped<ICurrencyPropertyAdminEvent, CurrencyPropertyAdminEvent>();
            services.AddScoped<ICurrencyTypeAdminEvent, CurrencyTypeAdminEvent>();
            services.AddScoped<IRequestEvent, RequestEvent>();
            services.AddScoped<IRequestComponentEvent, RequestComponentEvent>();
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