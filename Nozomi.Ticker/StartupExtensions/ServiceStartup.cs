using Microsoft.Extensions.DependencyInjection;
using Nozomi.Infra.Admin.Service.Services;
using Nozomi.Infra.Admin.Service.Services.Interfaces;
using Nozomi.Infra.Analysis.Service.Services;
using Nozomi.Infra.Analysis.Service.Services.Interfaces;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class ServiceStartup
    {
        public static void ConfigureServiceLayer(this IServiceCollection services)
        {
            // Service Injections
            services.AddTransient<Nozomi.Infra.Admin.Service.Services.Interfaces.ICurrencyService, 
                Nozomi.Infra.Admin.Service.Services.CurrencyService>();
            services.AddTransient<ICurrencyPairService, CurrencyPairService>();
            services.AddTransient<Nozomi.Infra.Admin.Service.Services.Interfaces.ICurrencyTypeService, 
                Nozomi.Infra.Admin.Service.Services.CurrencyTypeService>();
            services.AddTransient<IComponentService, ComponentService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<IRcdHistoricItemService, RcdHistoricItemService>();
            services.AddScoped<IRequestTypeEvent, RequestTypeEvent>();
            services.AddTransient<ISourceService, SourceService>();
            services.AddTransient<ITickerService, TickerService>();
            services.AddTransient<ICurrencySourceService, CurrencySourceService>();

            // Admin Service Injections
            services.AddTransient<Nozomi.Infra.Admin.Service.Services.Interfaces.IAnalysedComponentService, 
                Nozomi.Infra.Admin.Service.Services.AnalysedComponentService>();
            services.AddTransient<ICurrencyPropertyService, CurrencyPropertyService>();
            
            // TODO: Microservice
            // Nozomi.Analysis Service injections
            services.AddTransient<IProcessAnalysedComponentService, ProcessAnalysedComponentService>();
            services.AddTransient<IAnalysedHistoricItemService, AnalysedHistoricItemService>();
        }
    }
}