using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Infra.Analysis.Service.Services;
using Nozomi.Infra.Analysis.Service.Services.Interfaces;
using Nozomi.Service.Identity.Handlers;
using Nozomi.Service.Identity.Services;
using Nozomi.Service.Identity.Services.Interfaces;
using Nozomi.Service.Identity.Stores;
using Nozomi.Service.Identity.Stores.Interfaces;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Enumerators;
using Nozomi.Service.Services.Enumerators.Interfaces;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Analysis.StartupExtensions
{
    public static class StartupServices
    {
        public static void ConfigureServiceLayer(this IServiceCollection services)
        {
            // Service Injections
            services.AddTransient<IApiTokenService, ApiTokenService>();
            services.AddTransient<ICurrencyService, CurrencyService>();
            services.AddTransient<ICurrencyPairService, CurrencyPairService>();
            services.AddTransient<IRequestComponentService, RequestComponentService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<IRcdHistoricItemService, RcdHistoricItemService>();
            services.AddTransient<IRequestLogService, RequestLogService>();
            services.AddTransient<ISourceService, SourceService>();
            services.AddTransient<ITickerService, TickerService>();
            services.AddTransient<ICurrencySourceService, CurrencySourceService>();

            // Singleton service injections for in-memory-related processes.
            services.AddScoped<IComponentTypeService, ComponentTypeService>();
            services.AddScoped<ICurrencyPairTypeService, CurrencyPairTypeService>();
            services.AddScoped<IRequestPropertyTypeService, RequestPropertyTypeService>();
            services.AddScoped<IRequestTypeService, RequestTypeService>();
            services.AddScoped<IStripeService, StripeService>();
            
            // Identity-related service injections
            services.AddTransient<INozomiUserStore, NozomiUserStore>();
            
            // Nozomi.Analysis Service injections
            services.AddTransient<IAnalysedComponentService, AnalysedComponentService>();
            services.AddTransient<IAnalysedHistoricItemService, AnalysedHistoricItemService>();
        }
    }
}