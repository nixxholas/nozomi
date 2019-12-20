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

            services.AddHttpContextAccessor();

            // Auth Events
            services.AddScoped<IValidatingEvent, ValidatingEvent>();

            services.AddScoped<IAnalysedComponentEvent, AnalysedComponentEvent>();
            services.AddScoped<IAnalysedComponentTypeEvent, AnalysedComponentTypeEvent>();
            services.AddScoped<IAnalysedHistoricItemEvent, AnalysedHistoricItemEvent>();
            services.AddScoped<IComponentTypeEvent, ComponentTypeEvent>();
            services.AddScoped<ICurrencyEvent, CurrencyEvent>();
            services.AddScoped<ICurrencyPairEvent, CurrencyPairEvent>();
            services.AddScoped<ICurrencyPairTypeEvent, CurrencyPairTypeEvent>();
            services.AddScoped<ICurrencyTypeEvent, CurrencyTypeEvent>();
            services.AddScoped<IRequestEvent, RequestEvent>();
            services.AddScoped<IComponentEvent, ComponentEvent>();
            services.AddScoped<IRequestPropertyTypeEvent, RequestPropertyTypeEvent>();
            services.AddScoped<IRequestTypeEvent, RequestTypeEvent>();
            services.AddScoped<ISourceEvent, SourceEvent>();
            services.AddScoped<ISourceTypeEvent, SourceTypeEvent>();
            services.AddScoped<ITickerEvent, TickerEvent>();

            // Services
            services.AddScoped<IAnalysedComponentService, AnalysedComponentService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IComponentService, ComponentService>();
            services.AddScoped<ICurrencyPairService, CurrencyPairService>();
            services.AddScoped<ICurrencySourceService, CurrencySourceService>();
            services.AddScoped<IRcdHistoricItemService, RcdHistoricItemService>();
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<ISourceTypeService, SourceTypeService>();
        }
    }
}
