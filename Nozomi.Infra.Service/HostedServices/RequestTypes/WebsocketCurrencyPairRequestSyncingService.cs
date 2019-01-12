using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nozomi.Data.WebModels;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Service.HostedServices.RequestTypes.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.HostedServices.RequestTypes
{
    public class WebsocketCurrencyPairRequestSyncingService : BaseHostedService<WebsocketCurrencyPairRequestSyncingService>, 
        IWebsocketCurrencyPairRequestSyncingService, IHostedService, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <key>The Id of the CurrencyPairRequest</key≥
        private readonly Dictionary<long, ClientWebSocket> _cprWebsockets;
        private readonly ICurrencyPairRequestService _currencyPairRequestService;
        
        public WebsocketCurrencyPairRequestSyncingService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _currencyPairRequestService = _scope.ServiceProvider.GetRequiredService<ICurrencyPairRequestService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("WebsocketCurrencyPairRequestSyncingService is starting.");

            stoppingToken.Register(() => _logger.LogInformation("WebsocketCurrencyPairRequestSyncingService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                //============================= Update Sockets to keep =============================// 
                
                // We will need to resync the Request collection to make sure we're polling only the ones we want to poll
                var requests = _currencyPairRequestService.GetAllByRequestType(RequestType.HttpPost);

                // Iterate the requests
                foreach (var rq in requests)
                {
                    // Remove old crap
                    if (_cprWebsockets.ContainsKey(rq.Id) && (!rq.IsEnabled || rq.DeletedAt != null))
                    {
                        _logger.LogInformation("[WebsocketCurrencyPairRequestSyncingService] Removing Request: " + rq.Id);
                        
                        // Stop the websocket from polling
                        
                        // Remove the websocket from the dictionary
                        if (!_cprWebsockets.Remove(rq.Id))
                        {
                            _logger.LogInformation("[WebsocketCurrencyPairRequestSyncingService] Error Removing Request: " + rq.Id);
                        }
                        else
                        {
                            _logger.LogInformation("[WebsocketCurrencyPairRequestSyncingService] Removed Request: " + rq.Id);
                        }
                    }
                    
                    // Add new crap
                    if (!_cprWebsockets.ContainsKey(rq.Id))
                    {
                        // Start the websockets here
                    }
                }
                
                //============================= End of Update Sockets to keep =============================//
                
                //============================= Check and update new data =============================// 
                
                
                
                //============================= End of check and update new data =============================// 
                
                // No naps taken
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogWarning("WebsocketCurrencyPairRequestSyncingService background task is stopping.");
        }

        public bool IsRequestNeeded(CurrencyPairRequest cpr)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Process(CurrencyPairRequest cpr)
        {
            throw new NotImplementedException();
        }
    }
}