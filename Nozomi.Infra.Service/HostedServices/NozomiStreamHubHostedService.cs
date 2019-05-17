using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Infra.Preprocessing.SignalR;
using Nozomi.Infra.Preprocessing.SignalR.Hubs.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.HostedServices.Interfaces;
using Nozomi.Service.Hubs;

namespace Nozomi.Service.HostedServices
{
    public class NozomiStreamHubHostedService : BaseHostedService<NozomiStreamHubHostedService>, 
        INozomiStreamHubHostedService
    {
        private const string ServiceName = "NozomiStreamHubHostedService";
        private readonly ICurrencyEvent _currencyEvent;
        private readonly IHubContext<NozomiStreamHub, INozomiStreamClient> _nozomiStreamHub;
        
        public NozomiStreamHubHostedService(IServiceProvider serviceProvider,
            IHubContext<NozomiStreamHub, INozomiStreamClient> nozomiStreamHub) : base(serviceProvider)
        {
            _currencyEvent = _scope.ServiceProvider.GetRequiredService<ICurrencyEvent>();
            
            _nozomiStreamHub = nozomiStreamHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} is starting.");

            stoppingToken.Register(() => _logger.LogInformation($"{ServiceName} is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Push the updated currency data
                    await _nozomiStreamHub.Clients.Group(NozomiSocketGroup.Currencies.GetDescription())
                        .Currencies(_currencyEvent.GetAllDetailed());
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[{ServiceName}]" +
                                       " Something bad happened");
                }
            }
            
            _logger.LogError($"{ServiceName} background task is stopping.");
        }

        public bool Broadcast()
        {
            throw new NotImplementedException();
        }
    }
}