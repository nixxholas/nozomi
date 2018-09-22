using Nozomi.Data.WebModels;
using Nozomi.Service.HostedServices.RequestTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nozomi.Data.HubModels.Interfaces;
using Nozomi.Service.Hubs;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.HostedServices.RequestTypes
{
    public class HttpPostCurrencyPairRequestSyncingService : BaseHostedService<HttpPostCurrencyPairRequestSyncingService>, 
        IHttpPostCurrencyPairRequestSyncingService, IHostedService, IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ICurrencyPairRequestService _currencyPairRequestService;
        private readonly IHubContext<TickerHub, ITickerHubClient> _tickerHub;
        
        public HttpPostCurrencyPairRequestSyncingService(IServiceProvider serviceProvider,
            IHubContext<TickerHub, ITickerHubClient> tickerHub) : base(serviceProvider)
        {
            _currencyPairRequestService = _scope.ServiceProvider.GetRequiredService<ICurrencyPairRequestService>();
            _tickerHub = tickerHub;
        }

        public Task<bool> Process(CurrencyPairRequest req)
        {
            throw new NotImplementedException();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("HttpGetCurrencyPairRequestSyncingService is starting.");

            stoppingToken.Register(() => _logger.LogInformation("HttpGetCurrencyPairRequestSyncingService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                // We will need to resync the Request collection to make sure we're polling only the ones we want to poll
                var getBasedRequests = _currencyPairRequestService.GetAllActive(r => r.IsEnabled && r.DeletedAt == null
                                                                                                 && r.RequestType.Equals(RequestType
                                                                                                     .HttpGet), true);

                // Iterate the requests
                // NOTE: Let's not call a parallel loop since HttpClients might tend to result in memory leaks.
                foreach (var rq in getBasedRequests)
                {
                    // Process the request
                    if (await Process(rq))
                    {
                        // Since its successful, broadcast its success
                        await _tickerHub.Clients.All.BroadcastData(rq.ObscureToPublicJson());
                    }
                }
                
                // No naps taken
                //await Task.Delay(0, stoppingToken);
            }

            _logger.LogWarning("HttpGetCurrencyPairRequestSyncingService background task is stopping.");
        }
    }
}
