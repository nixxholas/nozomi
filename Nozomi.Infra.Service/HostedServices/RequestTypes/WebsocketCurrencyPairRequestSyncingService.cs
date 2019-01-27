using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nozomi.Data.WebModels;
using Nozomi.Data.WebModels.WebsocketModels;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Service.Events.Websocket.Interfaces;
using Nozomi.Service.HostedServices.RequestTypes.Interfaces;
using Nozomi.Service.Services.Interfaces;
using WebSocket = WebSocketSharp.WebSocket;

namespace Nozomi.Service.HostedServices.RequestTypes
{
    public class WebsocketCurrencyPairRequestSyncingService : BaseHostedService<WebsocketCurrencyPairRequestSyncingService>, 
        IWebsocketCurrencyPairRequestSyncingService, IHostedService, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <key>The Id of the WebsocketRequest</key≥
        private readonly Dictionary<long, WebSocket> _wsrWebsockets;
        private readonly IWebsocketRequestEvent _websocketRequestEvent;
        
        public WebsocketCurrencyPairRequestSyncingService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _websocketRequestEvent = _scope.ServiceProvider.GetRequiredService<IWebsocketRequestEvent>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("WebsocketCurrencyPairRequestSyncingService is starting.");

            stoppingToken.Register(() => _logger.LogInformation("WebsocketCurrencyPairRequestSyncingService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                //============================= Update Sockets to keep =============================// 
                
                // We will need to resync the Request collection to make sure we're polling only the ones we want to poll
                var requests = _websocketRequestEvent.GetAllByRequestType(RequestType.WebSocket);

                // Iterate the requests
                foreach (var rq in requests)
                {
                    // Remove old crap
                    if (_wsrWebsockets.ContainsKey(rq.Id) && (!rq.IsEnabled || rq.DeletedAt != null))
                    {
                        _logger.LogInformation("[WebsocketCurrencyPairRequestSyncingService] Removing Request: " + rq.Id);
                        
                        // Stop the websocket from polling
                        _wsrWebsockets[rq.Id].Close();
                        
                        // Remove the websocket from the dictionary
                        if (!_wsrWebsockets.Remove(rq.Id))
                        {
                            _logger.LogInformation("[WebsocketCurrencyPairRequestSyncingService] Error Removing Request: " + rq.Id);
                        }
                        else
                        {
                            _logger.LogInformation("[WebsocketCurrencyPairRequestSyncingService] Removed Request: " + rq.Id);
                        }
                    }
                    
                    // Add new crap
                    if (!_wsrWebsockets.ContainsKey(rq.Id) && string.IsNullOrEmpty(rq.DataPath))
                    {
                        // Start the websockets here
                        var newSocket = new WebSocket(rq.DataPath);
                        newSocket.EmitOnPing = true;

                        newSocket.OnOpen += (sender, args) => { };

                        newSocket.OnMessage += async (sender, args) =>
                        {
                            if (args.IsPing)
                            {
                                newSocket.Ping();
                            }
                            
                            // Process the incoming data
                            if (string.IsNullOrEmpty(args.Data))
                            {
                                if (await Process(rq, args.Data))
                                {
                                    _logger.LogInformation($"[WebsocketCurrencyPairRequestSyncingService] " +
                                                           $"RequestId: {rq.Id} successfully updated");
                                }
                            }
                            else
                            {
                                _logger.LogError($"[WebsocketCurrencyPairRequestSyncingService] OnMessage: " +
                                                 $"RequestId:{rq.Id} has an empty payload incoming.");
                            }
                        };

                        newSocket.OnError += (sender, args) => { };
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

        public bool IsRequestNeeded(WebsocketRequest cpr)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Process(WebsocketRequest wsr, string payload)
        {
            throw new NotImplementedException();
        }
    }
}