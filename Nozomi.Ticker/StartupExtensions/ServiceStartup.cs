using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nozomi.Preprocessing.Events;
using Nozomi.Preprocessing.Events.Interfaces;
using Nozomi.Service.HostedServices.RequestTypes;
using Nozomi.Service.Identity.Stores;
using Nozomi.Service.Identity.Stores.Interfaces;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Enumerators;
using Nozomi.Service.Services.Enumerators.Interfaces;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class ServiceStartup
    {
        public static void ConfigureServiceLayer(this IServiceCollection services)
        {
            // Service Injections
            services.AddTransient<ICurrencyService, CurrencyService>();
            services.AddTransient<ICurrencyPairService, CurrencyPairService>();
            services.AddTransient<ICurrencyPairComponentService, CurrencyPairComponentService>();
            services.AddTransient<ICurrencyPairRequestService, CurrencyPairRequestService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<IRequestLogService, RequestLogService>();
            services.AddTransient<ISourceService, SourceService>();
            services.AddTransient<ITickerService, TickerService>();

            // Singleton service injections for in-memory-related processes.
            services.AddScoped<IComponentTypeService, ComponentTypeService>();
            services.AddScoped<ICurrencyPairTypeService, CurrencyPairTypeService>();
            services.AddScoped<IRequestPropertyTypeService, RequestPropertyTypeService>();
            services.AddScoped<IRequestTypeService, RequestTypeService>();

            // Event Sourcing
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SmsSender>();
            services.AddTransient<IStripeEvent, StripeEvent>();
            
            // Identity-related service injections
            services.AddTransient<INozomiUserStore, NozomiUserStore>();
        }
    }
}