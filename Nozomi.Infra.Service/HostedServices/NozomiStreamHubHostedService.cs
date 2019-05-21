using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels.AnalysedComponent;
using Nozomi.Data.ResponseModels.CurrencyType;
using Nozomi.Infra.Preprocessing.SignalR;
using Nozomi.Infra.Preprocessing.SignalR.Hubs.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.HostedServices.Interfaces;
using Nozomi.Service.Hubs;

namespace Nozomi.Service.HostedServices
{
    public class NozomiStreamHubHostedService : BaseHostedService<NozomiStreamHubHostedService>, 
        INozomiStreamHubHostedService
    {
        private const string ServiceName = "NozomiStreamHubHostedService";
        private readonly IAnalysedComponentEvent _analysedComponentEvent;
        private readonly ICurrencyEvent _currencyEvent;
        private readonly IHubContext<NozomiStreamHub, INozomiStreamClient> _nozomiStreamHub;
        
        public NozomiStreamHubHostedService(IServiceProvider serviceProvider,
            IHubContext<NozomiStreamHub, INozomiStreamClient> nozomiStreamHub) : base(serviceProvider)
        {
            _analysedComponentEvent = _scope.ServiceProvider.GetRequiredService<IAnalysedComponentEvent>();
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
                    
                    await _nozomiStreamHub.Clients.Group(NozomiSocketGroup.MarketCaps.GetDescription())
                        .MarketCaps(ObtainCurrencyTypeResponses(_analysedComponentEvent
                            .GetAllCurrencyTypeAnalysedComponents(0, true, true)));
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
        
        private ICollection<CurrencyTypeResponse> ObtainCurrencyTypeResponses(ICollection<AnalysedComponent> analysedComponents)
        {
            var res = new List<CurrencyTypeResponse>();
            
            if (analysedComponents != null && analysedComponents.Count > 0)
            {
                foreach (var ac in analysedComponents)
                {
                    // Safetynet
                    if (ac.CurrencyType != null && !string.IsNullOrEmpty(ac.Value))
                    {
                        // If the CTR does not exist yet
                        if (!res.Any(ctr => ctr.Name.Equals(ac.CurrencyType.Name)))
                        {
                            // Create
                            res.Add(new CurrencyTypeResponse
                            {
                                Name = ac.CurrencyType.Name,
                                Components = new List<AnalysedComponentResponse>()
                                {
                                    new AnalysedComponentResponse
                                    {
                                        ComponentType = ac.ComponentType,
                                        Value = ac.Value
                                    }
                                }
                            });
                        }
                        else
                        {
                            // Already exists, populate
                            res.SingleOrDefault(ctr => ctr.Name.Equals(ac.CurrencyType.Name))?.Components
                                .Add(new AnalysedComponentResponse
                                {
                                    ComponentType = ac.ComponentType,
                                    Value = ac.Value
                                });
                        }
                    }
                }
            }

            return res;
        }
    }
}