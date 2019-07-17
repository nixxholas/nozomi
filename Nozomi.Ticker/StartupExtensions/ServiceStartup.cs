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
using Nozomi.Service.Identity.Events;
using Nozomi.Service.Identity.Events.Interfaces;
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
using AnalysedComponentService = Nozomi.Infra.Admin.Service.Services.AnalysedComponentService;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class ServiceStartup
    {
        public static void ConfigureServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, ApiTokenHandler>();
            
            // Service Injections
            services.AddTransient<IApiTokenService, ApiTokenService>();
            services.AddTransient<ICurrencyService, CurrencyService>();
            services.AddTransient<ICurrencyPairService, CurrencyPairService>();
            services.AddTransient<ICurrencyTypeService, CurrencyTypeService>();
            services.AddTransient<IRequestComponentService, RequestComponentService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<IRcdHistoricItemService, RcdHistoricItemService>();
            services.AddTransient<IRequestLogService, RequestLogService>();
            services.AddTransient<ISourceService, SourceService>();
            services.AddTransient<ITickerService, TickerService>();
            services.AddTransient<ICurrencySourceService, CurrencySourceService>();

            // Singleton service injections for in-memory-related processes.
            services.AddScoped<ICurrencyPairTypeService, CurrencyPairTypeService>();
            services.AddScoped<IRequestPropertyTypeService, RequestPropertyTypeService>();
            services.AddScoped<IRequestTypeService, RequestTypeService>();
            services.AddScoped<IStripeService, StripeService>();
            
            // Admin Service Injections
            services.AddTransient<IAnalysedComponentService, AnalysedComponentService>();
            services.AddTransient<ICurrencyPropertyService, CurrencyPropertyService>();
            
            // Identity-related service injections
            services.AddTransient<INozomiUserStore, NozomiUserStore>();
            
            // TODO: Microservice
            // Nozomi.Analysis Service injections
            services.AddTransient<IProcessAnalysedComponentService, ProcessAnalysedComponentService>();
            services.AddTransient<IAnalysedHistoricItemService, AnalysedHistoricItemService>();
        }
    }
}