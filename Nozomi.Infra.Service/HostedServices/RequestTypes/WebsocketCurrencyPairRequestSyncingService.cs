using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nozomi.Data.WebModels;
using Nozomi.Data.WebModels.WebsocketModels;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Service.Events.Websocket.Interfaces;
using Nozomi.Service.HostedServices.RequestTypes.Interfaces;
using Nozomi.Service.Services.Interfaces;
using WebSocketSharp;
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
        private readonly Dictionary<string, WebSocket> _wsrWebsockets;
        private readonly IWebsocketRequestEvent _websocketRequestEvent;
        
        public WebsocketCurrencyPairRequestSyncingService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _wsrWebsockets = new Dictionary<string, WebSocket>();
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
                var dataEndpoints = _websocketRequestEvent.GetAllByRequestTypeUniqueToURL(RequestType.WebSocket);

                // Iterate the requests
                foreach (var dataEndpoint in dataEndpoints)
                {
                    var dataEndpointItem = dataEndpoint.Value.FirstOrDefault();

                    if (dataEndpointItem != null && dataEndpointItem.DeletedAt == null
                        && dataEndpointItem.IsEnabled)
                    {
                    // Add new crap
                    if (!_wsrWebsockets.ContainsKey(dataEndpointItem.DataPath) 
                        && !string.IsNullOrEmpty(dataEndpointItem.DataPath))
                    {
                        // Start the websockets here
                        var newSocket = new WebSocket(dataEndpointItem.DataPath)
                        {
                            Compression = CompressionMethod.Deflate,
                            EmitOnPing = true,
                            EnableRedirection = false
                        };

                        // Pre-request processing
                        newSocket.OnOpen += (sender, args) =>
                        {
                            var wsCommands = dataEndpoint.Value.SelectMany(de => de.WebsocketCommands).ToList();

                                foreach (var wsCommand in wsCommands)
                                {
                                    if (wsCommand.Delay.Equals(0))
                                    {
                                        // One-time command
                                    }
                                    else
                                    {
                                        // Run a repeated task
                                    }
                                }
                        };

                        // Incoming processing
                        newSocket.OnMessage += async (sender, args) =>
                        {
                            if (args.IsPing)
                            {
                                newSocket.Ping();
                            }
                            
                            // Process the incoming data
                            if (!string.IsNullOrEmpty(args.Data))
                            {
                                if (await Process(dataEndpoint.Value, args.Data))
                                {
                                    _logger.LogInformation($"[WebsocketCurrencyPairRequestSyncingService] " +
                                                           $"RequestId: {dataEndpointItem.DataPath} successfully updated");
                                }
                            }
                            else
                            {
                                _logger.LogError($"[WebsocketCurrencyPairRequestSyncingService] OnMessage: " +
                                                 $"RequestId:{dataEndpointItem.DataPath} has an empty payload incoming.");
                            }
                        };

                        // Error processing
                        newSocket.OnError += (sender, args) =>
                        {
                            _logger.LogError($"[WebsocketCurrencyPairRequestSyncingService] OnError:" +
                                             $" {args.Message}");
                        };
                        
                        newSocket.Connect();
                        _wsrWebsockets.Add(dataEndpointItem.DataPath, newSocket);
                    }
                    } else 
                        // Remove old crap
                    if (dataEndpointItem != null && (!dataEndpointItem.IsEnabled || dataEndpointItem.DeletedAt != null) 
                        && _wsrWebsockets.ContainsKey(dataEndpointItem.DataPath))
                    {
                        _logger.LogInformation("[WebsocketCurrencyPairRequestSyncingService] Removing " +
                                               "Request: " + dataEndpointItem.Id);
                        
                        // Stop the websocket from polling
                        _wsrWebsockets[dataEndpointItem.DataPath].Close();
                        
                        // Remove the websocket from the dictionary
                        if (!_wsrWebsockets.Remove(dataEndpointItem.DataPath))
                        {
                            _logger.LogInformation(
                                "[WebsocketCurrencyPairRequestSyncingService] Error Removing Request: " 
                                + dataEndpointItem.DataPath);
                        }
                        else
                        {
                            _logger.LogInformation(
                                "[WebsocketCurrencyPairRequestSyncingService] Removed Request: " 
                                + dataEndpointItem.DataPath);
                        }
                    }
                }
                
                //============================= End of Update Sockets to keep =============================//
                
                //============================= Check and update new data =============================// 
                
                
                
                //============================= End of check and update new data =============================// 
                
                // No naps taken
                await Task.Delay(0, stoppingToken);
            }

            _logger.LogWarning("WebsocketCurrencyPairRequestSyncingService background task is stopping.");
        }
        
        public Task<bool> Process(ICollection<WebsocketRequest> wsr, string payload)
        {
            // Are we processing anything?
            if (wsr.Count > 0 && !string.IsNullOrEmpty(payload))
            {
                switch (wsr.FirstOrDefault().ResponseType)
                {
                    case ResponseType.Json:
                        // Do nothing
                        break;
                    case ResponseType.XML:
                        var xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(payload);
                        payload = JsonConvert.SerializeObject(xmlDoc);
                        break;
                }

                var payloadToken = JToken.Parse(payload);
                
                var wsrComponents = wsr.SelectMany(e => e.RequestComponents)
                    .Where(rc => rc.DeletedAt == null && rc.IsEnabled);

                // Are we processing anything?
                if (wsrComponents.Any())
                {
                    return Task.FromResult(Update(payloadToken, wsr.FirstOrDefault().ResponseType, wsrComponents));
                }
                else
                {
                    _logger.LogWarning("[WebsocketCurrencyPairRequestSyncingService] Process: Empty " +
                                       $"WebsocketRequestComponents in {wsr.FirstOrDefault().DataPath}.");
                }
            }
            
            return Task.FromResult(false);
        }

        public bool Update(JToken token, ResponseType resType, IEnumerable<RequestComponent> requestComponents)
        {
            throw new NotImplementedException();
        }
    }
}