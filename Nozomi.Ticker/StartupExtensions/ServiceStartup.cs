using CounterCore.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nozomi.Service.HostedServices.RequestTypes;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class ServiceStartup
    {
        public static void ConfigureServiceLayer(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                // Service Injections
                services.AddTransient<ICurrencyService, CurrencyService>();
                services.AddTransient<ICurrencyPairService, CurrencyPairService>();
                services.AddTransient<ICurrencyPairComponentService, CurrencyPairComponentService>();
                services.AddTransient<ICurrencyPairRequestService, CurrencyPairRequestService>();
                services.AddTransient<IRequestService, RequestService>();
                services.AddTransient<IRequestLogService, RequestLogService>();
                services.AddTransient<ISourceService, SourceService>();

                // Hosted Services
                services.AddSingleton<IHostedService, HttpGetCurrencyPairRequestSyncingService>();
            });
        }
    }
}