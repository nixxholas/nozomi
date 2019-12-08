using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nozomi.Infra.Admin.Service.Events;
using Nozomi.Infra.Admin.Service.Events.Interfaces;
using Nozomi.Infra.Admin.Service.Services;
using Nozomi.Infra.Admin.Service.Services.Interfaces;
using Nozomi.Infra.Analysis.Service.Services;
using Nozomi.Infra.Analysis.Service.Services.Interfaces;
using Nozomi.Preprocessing.Events;
using Nozomi.Preprocessing.Events.Interfaces;
using Nozomi.Service.Events;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Enumerators;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;
using AnalysedComponentService = Nozomi.Infra.Admin.Service.Services.AnalysedComponentService;

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
            services.AddTransient<ICurrencyTypeService, CurrencyTypeService>();
            services.AddTransient<IComponentService, ComponentService>();
            services.AddTransient<Nozomi.Infra.Admin.Service.Services.Interfaces.IRequestService, 
                Nozomi.Infra.Admin.Service.Services.RequestService>();
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