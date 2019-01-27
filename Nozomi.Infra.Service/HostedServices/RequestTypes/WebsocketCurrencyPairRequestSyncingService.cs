using System;
using System.Collections.Generic;
using System.Globalization;
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
using Nozomi.Base.Core.Helpers.Exponent;
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
    public class WebsocketCurrencyPairRequestSyncingService :
        BaseHostedService<WebsocketCurrencyPairRequestSyncingService>,
        IWebsocketCurrencyPairRequestSyncingService, IHostedService, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <key>The Id of the WebsocketRequest</key≥
        private readonly Dictionary<string, WebSocket> _wsrWebsockets;

        private readonly IWebsocketRequestEvent _websocketRequestEvent;
        private readonly ICurrencyPairComponentService _currencyPairComponentService;

        public WebsocketCurrencyPairRequestSyncingService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _wsrWebsockets = new Dictionary<string, WebSocket>();
            _currencyPairComponentService = _scope.ServiceProvider.GetRequiredService<ICurrencyPairComponentService>();
            _websocketRequestEvent = _scope.ServiceProvider.GetRequiredService<IWebsocketRequestEvent>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("WebsocketCurrencyPairRequestSyncingService is starting.");

            stoppingToken.Register(() =>
                _logger.LogInformation("WebsocketCurrencyPairRequestSyncingService is stopping."));

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
                                    try
                                    {
                                        if (await Process(dataEndpoint.Value, args.Data))
                                        {
                                            _logger.LogInformation($"[WebsocketCurrencyPairRequestSyncingService] " +
                                                                   $"RequestId: {dataEndpointItem.DataPath} successfully updated");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogCritical(
                                            $"[WebsocketCurrencyPairRequestSyncingService] OnMessage: " + 
                                            ex);
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
                    }
                    else
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
            var processingToken = token;

            // For each component we're checking
            foreach (var component in requestComponents)
            {
                // Always reset
                processingToken = token;

                // Identifier processing
                if (!string.IsNullOrEmpty(component.Identifier))
                {
                    var identifierArr = component.Identifier.Split("/"); // Split the string if its nesting
                    var lastIdentifier = identifierArr.LastOrDefault(); // get the last to identify if its the last

                    foreach (var identifierEl in identifierArr)
                    {
                        if (identifierEl != null)
                        {
                            // CHECK CURRENT TYPE
                            // Identify if its an array or an object
                            if (processingToken is JArray)
                            {
                                try
                                {
                                    // Is it the last?
                                    if (identifierEl != lastIdentifier)
                                    {
                                        // Parse the identifierEl to an integer for index access
                                        if (int.TryParse(identifierEl, out int index))
                                        {
                                            // Pump in the array, treat it as anonymous.
                                            var dataList = processingToken.ToObject<List<JObject>>();

                                            // let's work it out
                                            // update the token
                                            if (index >= 0 && index < dataList.Count)
                                            {
                                                // Traverse the array
                                                processingToken =
                                                    JToken.Parse(JsonConvert.SerializeObject(dataList[index]));
                                            }
                                        }
                                    }
                                    // Yes its the last
                                    else
                                    {
                                        // See if there's any property we need to refer to.
                                        var comArrElArr = identifierEl.Split("=>");

                                        // An array of objects. Let's find the key comArrElArr[0] where
                                        // the value equals comArrElArr[1]
                                        if (comArrElArr.Length == 2)
                                        {
                                            // https://stackoverflow.com/questions/7216917/json-net-has-key-method
                                            var correctEl = (string) processingToken.Children()
                                                .FirstOrDefault(tok => tok[comArrElArr[0]] != null
                                                                       && (bool) tok[comArrElArr[0].Equals(comArrElArr[1])]);

                                            // Null check
                                            if (correctEl == null)
                                            {
                                                _logger.LogError("[WebsocketCurrencyPairRequestSyncingService] " +
                                                                 $"Invalid key value pair {identifierEl}");
                                                return false;
                                            }
                                            else
                                            {
                                                // We found it
                                                processingToken = correctEl;
                                            }
                                        }
                                        // A standard array
                                        else if (comArrElArr.Length == 1)
                                        {
                                            // Parse the identifierEl to an integer for index access
                                            if (int.TryParse(identifierEl, out int index))
                                            {
                                                // Pump in the array, treat it as anonymous.
                                                var dataList = processingToken.ToObject<List<JObject>>();

                                                // let's work it out
                                                // update the token
                                                if (index >= 0 && index < dataList.Count)
                                                {
                                                    // Traverse the array and move up to the parent
                                                    processingToken =
                                                        JToken.Parse(JsonConvert.SerializeObject(dataList[index])).Parent;
                                                }
                                            }
                                            else
                                            {
                                                _logger.LogError("[WebsocketCurrencyPairRequestSyncingService]" +
                                                                 $" Update: Invalid array element {identifierEl}");
                                                return false;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                }
                            }
                            else if (processingToken is JObject)
                            {
                                // Pump in the object
                                JObject obj = processingToken.ToObject<JObject>();

                                // Is it the last?
                                if (identifierEl != lastIdentifier)
                                {
                                    // let's work it out
                                    // update the token
                                    processingToken = obj.SelectToken(identifierEl);
                                }
                                // Yes its the last
                                else
                                {
                                    // let's work it out
                                    // update the token
                                    processingToken = obj.SelectToken(identifierEl).Parent;
                                }
                            }
                            // iterate JValue like a JObject
                            else if (processingToken is JValue)
                            {
                                // Pump in the object
                                JObject obj = processingToken.ToObject<JObject>();

                                // Is it the last?
                                if (identifierEl != lastIdentifier)
                                {
                                    // let's work it out
                                    // update the token
                                    processingToken = obj.SelectToken(identifierEl);
                                }
                                // Yes its the last
                                else
                                {
                                    // let's work it out
                                    // update the token
                                    processingToken = obj.SelectToken(identifierEl).Parent;
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                var comArr = component.QueryComponent.Split("/"); // Split the string if its nesting
                var last = comArr.LastOrDefault(); // get the last to identify if its the last

                // Iterate the queryComponent Array
                foreach (var comArrEl in comArr)
                {
                    // Null check
                    if (comArrEl != null)
                    {
                        // CHECK CURRENT TYPE
                        // Identify if its an array or an object
                        if (processingToken is JArray)
                        {
                            try
                            {
                                // Is it the last?
                                if (comArrEl != last)
                                {
                                    // Parse the comArrEl to an integer for index access
                                    if (int.TryParse(comArrEl, out int index))
                                    {
                                        // Pump in the array, treat it as anonymous.
                                        var dataList = processingToken.ToObject<List<JObject>>();

                                        // let's work it out
                                        // update the token
                                        if (index >= 0 && index < dataList.Count)
                                        {
                                            // Traverse the array
                                            processingToken =
                                                JToken.Parse(JsonConvert.SerializeObject(dataList[index]));
                                        }
                                    }
                                }
                                // Yes its the last
                                else
                                {
                                    // See if theres any property we need to refer to.
                                    var comArrElArr = comArrEl.Split("=>");

                                    if (int.TryParse(comArrElArr[0], out var index))
                                    {
                                        // Traverse first
                                        var rawData = processingToken.ToObject<List<JToken>>()[index];

                                        // if its 1, we assume its just an array of a primitive type
                                        if (comArrElArr.Length == 1)
                                        {
                                            // Retrieve the value.
                                            var rawVal = rawData.ToString();

                                            // https://stackoverflow.com/questions/23131414/culture-invariant-decimal-tryparse
                                            var style = NumberStyles.Any;
                                            if (ExponentHelper.IsExponentialFormat(rawVal))
                                            {
                                                style = NumberStyles.Float;
                                            }

                                            // If it is an exponent
                                            if (decimal.TryParse(rawVal, style, CultureInfo.InvariantCulture,
                                                out var val))
                                            {
                                                if (val > 0)
                                                {
                                                    // Update it
                                                    _currencyPairComponentService.UpdatePairValue(component.Id, val);
                                                }
                                            }
                                        }
                                        // Oh no.. non-primitive...
                                        else if (comArrElArr.Length == 2)
                                        {
                                            // Object-ify
                                            var rawObj = JObject.Parse(rawData.ToString());

                                            // Obtain the desired value
                                            var rawVal = rawObj[comArrElArr[1]].ToString();

                                            // As usual, update it
                                            // https://stackoverflow.com/questions/23131414/culture-invariant-decimal-tryparse
                                            var style = NumberStyles.Any;
                                            if (ExponentHelper.IsExponentialFormat(rawVal))
                                            {
                                                style = NumberStyles.Float;
                                            }

                                            // If it is an exponent
                                            if (decimal.TryParse(rawVal, style, CultureInfo.InvariantCulture,
                                                out var val))
                                            {
                                                if (val > 0)
                                                {
                                                    // Update it
                                                    _currencyPairComponentService.UpdatePairValue(component.Id, val);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // Invalid
                                            return false;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        else if (processingToken is JObject)
                        {
                            // Pump in the object
                            JObject obj = processingToken.ToObject<JObject>();

                            // Is it the last?
                            if (comArrEl != last)
                            {
                                // let's work it out
                                // update the token
                                processingToken = obj.SelectToken(comArrEl);
                            }
                            // Yes its the last
                            else
                            {
                                // See if theres any property we need to refer to.
                                var comArrElArr = comArrEl.Split("=>");

                                // Traverse first
                                var rawData = (string) obj.SelectToken(comArrElArr[0]);

                                if (rawData != null)
                                {
                                    // if its 1, we assume its just an array of a primitive type
                                    if (comArrElArr.Length == 1)
                                    {
                                        // Retrieve the value.
                                        var rawVal = rawData.ToString();

                                        // https://stackoverflow.com/questions/23131414/culture-invariant-decimal-tryparse
                                        var style = NumberStyles.Any;
                                        if (ExponentHelper.IsExponentialFormat(rawVal))
                                        {
                                            style = NumberStyles.Float;
                                        }

                                        // If it is an exponent
                                        if (decimal.TryParse(rawVal, style, CultureInfo.InvariantCulture,
                                            out var val))
                                        {
                                            if (val > 0)
                                            {
                                                // Update it
                                                _currencyPairComponentService.UpdatePairValue(component.Id, val);
                                            }
                                        }
                                    }
                                    // Oh no.. non-primitive...
                                    else if (comArrElArr.Length == 2)
                                    {
                                        // Object-ify
                                        var rawObj = JObject.Parse(rawData.ToString());

                                        // Obtain the desired value
                                        var rawVal = rawObj[comArrElArr[1]].ToString();

                                        // As usual, update it
                                        // https://stackoverflow.com/questions/23131414/culture-invariant-decimal-tryparse
                                        var style = NumberStyles.Any;
                                        if (ExponentHelper.IsExponentialFormat(rawVal))
                                        {
                                            style = NumberStyles.Float;
                                        }

                                        // If it is an exponent
                                        if (decimal.TryParse(rawVal, style, CultureInfo.InvariantCulture,
                                            out var val))
                                        {
                                            if (val > 0)
                                            {
                                                // Update it
                                                _currencyPairComponentService.UpdatePairValue(component.Id, val);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // Invalid
                                        return false;
                                    }
                                }
                            }
                        }
                        // iterate JValue like a JObject
                        else if (processingToken is JValue)
                        {
                            // Pump in the object
                            JObject obj = processingToken.ToObject<JObject>();

                            // Is it the last?
                            if (comArrEl != last)
                            {
                                // let's work it out
                                // update the token
                                processingToken = obj.SelectToken(comArrEl);
                            }
                            // Yes its the last
                            else
                            {
                                var rawData = (string) obj.SelectToken(component.QueryComponent);

                                if (rawData != null)
                                {
                                    // https://stackoverflow.com/questions/23131414/culture-invariant-decimal-tryparse
                                    var style = NumberStyles.Any;
                                    if (ExponentHelper.IsExponentialFormat(rawData))
                                    {
                                        style = NumberStyles.Float;
                                    }

                                    // If it is an exponent
                                    if (decimal.TryParse(rawData, style, CultureInfo.InvariantCulture,
                                        out decimal val))
                                    {
                                        if (val > 0)
                                        {
                                            // Update it
                                            _currencyPairComponentService.UpdatePairValue(component.Id, val);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Something bad happened
                        return false;
                    }
                }
            }

            return true;
        }
    }
}