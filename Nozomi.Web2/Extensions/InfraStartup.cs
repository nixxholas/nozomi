using Microsoft.Extensions.DependencyInjection;
using Nozomi.Infra.Blockchain.Auth.Events;
using Nozomi.Infra.Blockchain.Auth.Events.Interfaces;
using Nozomi.Preprocessing.Events;
using Nozomi.Preprocessing.Events.Interfaces;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Analysis;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Enumerators;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Web2.Extensions
{
    public static class InfraStartup
    {
        public static void ConfigureInfra(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SmsSender>();

            // Auth Events
            services.AddScoped<IValidatingEvent, ValidatingEvent>();

            services.AddTransient<IAnalysedComponentEvent, AnalysedComponentEvent>();
            services.AddTransient<IAnalysedComponentTypeEvent, AnalysedComponentTypeEvent>();
            services.AddTransient<IAnalysedHistoricItemEvent, AnalysedHistoricItemEvent>();
            services.AddTransient<IComponentEvent, ComponentEvent>();
            services.AddTransient<IComponentTypeEvent, ComponentTypeEvent>();
            services.AddTransient<ICurrencyEvent, CurrencyEvent>();
            services.AddTransient<ICurrencyPairEvent, CurrencyPairEvent>();
            services.AddTransient<ICurrencyPairTypeEvent, CurrencyPairTypeEvent>();
            services.AddTransient<ICurrencySourceEvent, CurrencySourceEvent>();
            services.AddTransient<ICurrencyTypeEvent, CurrencyTypeEvent>();
            services.AddTransient<IRequestEvent, RequestEvent>();
            services.AddTransient<IRequestPropertyTypeEvent, RequestPropertyTypeEvent>();
            services.AddTransient<IRequestTypeEvent, RequestTypeEvent>();
            services.AddTransient<ISourceEvent, SourceEvent>();
            services.AddTransient<ISourceTypeEvent, SourceTypeEvent>();
            services.AddTransient<ITickerEvent, TickerEvent>();
            services.AddTransient<ITrelloEvent, TrelloEvent>();

            // Services
            services.AddScoped<IAnalysedComponentService, AnalysedComponentService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IComponentService, ComponentService>();
            services.AddScoped<ICurrencyPairService, CurrencyPairService>();
            services.AddScoped<ICurrencySourceService, CurrencySourceService>();
            services.AddScoped<ICurrencyTypeService, CurrencyTypeService>();
            services.AddScoped<IRcdHistoricItemService, RcdHistoricItemService>();
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<ISourceTypeService, SourceTypeService>();
        }
    }
}
