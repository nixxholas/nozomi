using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nozomi.Base.BCL.Helpers.Exponent;
using Nozomi.Data;
using Nozomi.Data.Models.Web;
using Nozomi.Infra.Syncing.HostedServices.RequestTypes.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests.Interfaces;
using sta_websocket_sharp_core;

namespace Nozomi.Infra.Syncing.HostedServices.RequestTypes
{
    public class WsRequestSyncingService :
        BaseProcessingService<WsRequestSyncingService>,
        IWsRequestSyncingService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <key>The Id of the WebsocketRequest</key≥
        private readonly Dictionary<string, WebSocketCore> _webSockets;

        public WsRequestSyncingService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            _webSockets = new Dictionary<string, WebSocketCore>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: Starting...");

            stoppingToken.Register(() =>
                _logger.LogInformation($"{_hostedServiceName} ExecuteAsync: Stopping..."));

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var websocketRequestEvent = scope.ServiceProvider.GetRequiredService<IRequestEvent>();
                    //============================= Update Sockets to keep =============================// 

                    // We will need to resync the Request collection to make sure we're polling only the ones we want to poll
                    var dataEndpoints = websocketRequestEvent
                        .GetAllByRequestTypeUniqueToURL(RequestType.WebSocket);

                    if (dataEndpoints.Count > 0)
                    {
                        // Iterate the requests
                        foreach (var dataEndpoint in dataEndpoints)
                        {
                            var dataEndpointItem = dataEndpoint.Value.FirstOrDefault();

                            if (dataEndpointItem != null && dataEndpointItem.DeletedAt == null
                                                         && dataEndpointItem.IsEnabled)
                            {
                                // Add new crap
                                if (!_webSockets.ContainsKey(dataEndpointItem.DataPath)
                                    && !string.IsNullOrEmpty(dataEndpointItem.DataPath))
                                {
                                    // Start the websockets here
                                    var newSocket = new WebSocketCore(dataEndpointItem.DataPath)
                                    {
                                        Compression = CompressionMethod.Deflate,
                                        EmitOnPing = true,
                                        EnableRedirection = false,
                                    };

                                    // Pre-request processing
                                    newSocket.OnOpen += (sender, args) =>
                                    {
                                        var wsCommands = dataEndpoint.Value
                                            .SelectMany(de => de.WebsocketCommands).ToList();

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
                                        else if (!string.IsNullOrEmpty(args.Data)) // Process the incoming data
                                        {
                                            try
                                            {
                                                if (await Process(dataEndpoint.Value, args.Data))
                                                {
                                                    _logger.LogInformation(
                                                        $"{_hostedServiceName} " +
                                                        $"RequestId: {dataEndpointItem.DataPath} successfully updated");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                _logger.LogCritical(
                                                    $"{_hostedServiceName} OnMessage: " +
                                                    ex);
                                                newSocket.Close();
                                            }
                                        }
                                        else
                                        {
                                            _logger.LogError($"{_hostedServiceName} OnMessage: " +
                                                             $"RequestId:{dataEndpointItem.DataPath} has an empty payload incoming.");
                                            newSocket.Close();
                                        }

                                        await Task.Delay(50,
                                            CancellationToken.None); // Always delay by 1ms in case of spam
                                    };

                                    // Error processing
                                    newSocket.OnError += async (sender, args) =>
                                    {
                                        _logger.LogError($"{_hostedServiceName} OnError:" +
                                                         $" {args.Message}");
                                        if (_webSockets.ContainsKey(dataEndpointItem.DataPath))
                                            _webSockets.Remove(dataEndpointItem.DataPath);
                                        GC.SuppressFinalize(this);
                                    };

                                    newSocket.OnClose += (sender, args) =>
                                    {
                                        _logger.LogInformation($"{_hostedServiceName} onClose: " +
                                                               $"Closing socket connection for {dataEndpointItem.DataPath}");
                                        if (_webSockets.ContainsKey(dataEndpointItem.DataPath))
                                            _webSockets.Remove(dataEndpointItem.DataPath);
                                        GC.SuppressFinalize(this);
                                    };

                                    newSocket.Connect();
                                    _webSockets.Add(dataEndpointItem.DataPath, newSocket);
                                }
                            }
                            else
                                // Remove old crap
                            if (dataEndpointItem != null && (!dataEndpointItem.IsEnabled ||
                                                             dataEndpointItem.DeletedAt != null)
                                                         && _webSockets.ContainsKey(dataEndpointItem.DataPath))
                            {
                                _logger.LogInformation($"{_hostedServiceName} Removing " +
                                                       "Request: " + dataEndpointItem.Id);

                                // Stop the websocket from polling
                                _webSockets[dataEndpointItem.DataPath].Close();

                                // Remove the websocket from the dictionary
                                if (!_webSockets.Remove(dataEndpointItem.DataPath))
                                {
                                    _logger.LogInformation(
                                        $"{_hostedServiceName} Error Removing Request: "
                                        + dataEndpointItem.DataPath);
                                }
                                else
                                {
                                    // WebSocketCloseStatus.Empty, 
                                    // $"Websocket for {dataEndpointItem.DataPath} is out of date! " +
                                    //     "Removing for update.", CancellationToken.None
                                    _logger.LogInformation(
                                        $"{_hostedServiceName} Removed Request: "
                                        + dataEndpointItem.DataPath);
                                }
                            }
                        }
                    }

                    //============================= End of Update Sockets to keep =============================//

                    //============================= Check and update new data =============================// 


                    //============================= End of check and update new data =============================// 

                    await Task.Delay(100, stoppingToken);
                }
            }

            _logger.LogWarning($"{_hostedServiceName}: Background task is stopping.");
        }

        public async Task<bool> Process(ICollection<Request> wsr, string payload)
        {
            // Are we processing anything?
            if (wsr != null && wsr.Any() && !string.IsNullOrEmpty(payload))
            {
                switch (wsr.FirstOrDefault().ResponseType)
                {
                    case ResponseType.Json:
                        // Do nothing
                        break;
                    case ResponseType.XML:
                        // Old Newtonsoft code.
                        var xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(payload);
                        payload = JsonConvert.SerializeObject(xmlDoc);

                        // var xmlDoc = new XmlDocument();
                        // xmlDoc.LoadXml(payload);
                        // payload = JsonSerializer.Serialize(xmlDoc);
                        break;
                }

                // var stream = new MemoryStream(Encoding.UTF8.GetBytes(payload));
                // var token = await JsonDocument.ParseAsync(stream);
                //     
                // var wsrComponents = wsr
                //     .SelectMany(e => e.RequestComponents)
                //     .Where(rc => rc.DeletedAt == null && rc.IsEnabled)
                //     .ToList();
                //     
                // // Are we processing anything?
                // if (wsrComponents.Any())
                // {
                //     if (Update(token, wsr.FirstOrDefault().ResponseType, wsrComponents)
                //         && requestService.HasUpdated(wsr))
                //     {
                //         _logger.LogInformation($"[{_hostedServiceName}] Process: Request object updated!");
                //         return true;
                //     }
                //     else
                //     {
                //         _logger.LogCritical($"[{_hostedServiceName}] Process: Couldn't update the Request object.");
                //     }
                // }
                // else
                // {
                //     _logger.LogWarning("[WebsocketCurrencyPairRequestSyncingService] Process: Empty " +
                //                        $"WebsocketRequestComponents in {wsr.FirstOrDefault().DataPath}.");
                // }

                var payloadToken = JToken.Parse(payload); // Old Newtonsoft code

                // Old Newtonsoft code
                var wsrComponents = wsr
                    .SelectMany(e => e.RequestComponents)
                    .Where(rc => rc.DeletedAt == null && rc.IsEnabled)
                    .ToList();

                // Are we processing anything?
                if (wsrComponents.Any())
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var requestService = scope.ServiceProvider.GetRequiredService<IRequestService>();

                        if (Update(payloadToken, wsr.FirstOrDefault().ResponseType, wsrComponents)
                            && requestService.HasUpdated(wsr))
                        {
                            _logger.LogInformation($"[{_hostedServiceName}] Process: Request object updated!");
                            return true;
                        }
                        else
                        {
                            _logger.LogCritical($"[{_hostedServiceName}] Process: Couldn't update the Request object.");
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("[WebsocketCurrencyPairRequestSyncingService] Process: Empty " +
                                       $"WebsocketRequestComponents in {wsr.FirstOrDefault().DataPath}.");
                }
            }

            return false;
        }

        // [Obsolete]
        public bool Update(JToken token, ResponseType resType, IEnumerable<Component> requestComponents)
        {
            // Null Checks
            if (token == null)
                return false;

            JToken processingToken;

            // For each component we're checking
            foreach (var component in requestComponents)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var componentService = scope.ServiceProvider.GetRequiredService<IComponentService>();

                    // Always reset
                    processingToken = token;

                    // Identifier processing
                    if (!string.IsNullOrEmpty(component.Identifier))
                    {
                        var res = ProcessIdentifier(processingToken, component.Identifier);

                        if (res.ResultType.Equals(NozomiResultType.Success))
                        {
                            processingToken = res.Data;
                        }
                        else
                        {
                            // Failed
                            componentService.Checked(component.Id);
                            processingToken = null; // Set it to fail for the next statement
                        }
                    }

                    // Identifier & Resetting null checks
                    if (processingToken != null)
                    {
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
                                                        var res = componentService
                                                            .UpdatePairValue(component.Id, val);

                                                        if (res.ResultType.Equals(NozomiResultType.Failed))
                                                        {
                                                            _logger.LogError(res.Message);
                                                        }
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
                                                        var res = componentService
                                                            .UpdatePairValue(component.Id, val);

                                                        if (res.ResultType.Equals(NozomiResultType.Failed))
                                                        {
                                                            _logger.LogError(res.Message);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // Invalid
                                                //return false;
                                            }
                                        }
                                    }
                                }
                                else if (processingToken is JObject)
                                {
                                    // Pump in the object
                                    var obj = processingToken.ToObject<JObject>();

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
                                        var rawData = (string) obj.GetValue(comArrElArr[0]);

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
                                                        var res = componentService
                                                            .UpdatePairValue(component.Id, val);

                                                        if (res.ResultType.Equals(NozomiResultType.Failed))
                                                        {
                                                            _logger.LogError(res.Message);
                                                        }
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
                                                        var res = componentService
                                                            .UpdatePairValue(component.Id, val);

                                                        if (res.ResultType.Equals(NozomiResultType.Failed))
                                                        {
                                                            _logger.LogError(res.Message);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // Invalid
                                                //return false;
                                            }
                                        }
                                    }
                                }
                                // iterate JValue like a JObject
                                else if (processingToken is JValue)
                                {
                                    // Pump in the object
                                    //JObject obj = processingToken.ToObject<JObject>();
                                    var obj = processingToken;

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
                                                    var res = componentService
                                                        .UpdatePairValue(component.Id, val);

                                                    if (res.ResultType.Equals(NozomiResultType.Failed))
                                                    {
                                                        _logger.LogError(res.Message);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Something bad happened
                                //return false;
                            }
                        }
                    }
                    else if (string.IsNullOrEmpty(component.Identifier))
                    {
                        _logger.LogInformation($"Marking Request Component as checked: {component.Id}");
                        return componentService.Checked(component.Id);
                    }
                }
            }

            return true;
        }

        // private async Task<bool> Update(JsonDocument jsonDoc, ResponseType resType, IEnumerable<Component> requestComponents)
        // {
        //     var componentService = _scope.ServiceProvider.GetRequiredService<IComponentService>();
        //     // Null Checks
        //     if (jsonDoc == null)
        //         return false;
        //
        //     var processingToken = jsonDoc;
        //
        //     // For each component we're checking
        //     foreach (var component in requestComponents)
        //     {
        //         // Always reset
        //         processingToken = jsonDoc;
        //
        //         // Identifier processing
        //         if (!string.IsNullOrEmpty(component.Identifier))
        //         {
        //             var res = ProcessIdentifier(processingToken, component.Identifier);
        //
        //             if (res.ResultType.Equals(NozomiResultType.Success))
        //             {
        //                 processingToken = res.Data;
        //             }
        //             else
        //             {
        //                 // Failed
        //                 componentService.Checked(component.Id);
        //                 processingToken = null; // Set it to fail for the next statement
        //             }
        //         }
        //
        //         // Identifier & Resetting null checks
        //         if (processingToken != null)
        //         {
        //             var comArr = component.QueryComponent.Split("/"); // Split the string if its nesting
        //             var last = comArr.LastOrDefault(); // get the last to identify if its the last
        //
        //             // Iterate the queryComponent Array
        //             foreach (var comArrEl in comArr)
        //             {
        //                 // Null check
        //                 if (comArrEl != null)
        //                 {
        //                     // CHECK CURRENT TYPE
        //                     // Identify if its an array or an object
        //                     if (processingToken is JArray)
        //                     {
        //                         try
        //                         {
        //                             // Is it the last?
        //                             if (comArrEl != last)
        //                             {
        //                                 // Parse the comArrEl to an integer for index access
        //                                 if (int.TryParse(comArrEl, out int index))
        //                                 {
        //                                     // Pump in the array, treat it as anonymous.
        //                                     var dataList = processingToken.ToObject<List<JObject>>();
        //
        //                                     // let's work it out
        //                                     // update the token
        //                                     if (index >= 0 && index < dataList.Count)
        //                                     {
        //                                         // Traverse the array
        //                                         processingToken =
        //                                             JToken.Parse(JsonConvert.SerializeObject(dataList[index]));
        //                                     }
        //                                 }
        //                             }
        //                             // Yes its the last
        //                             else
        //                             {
        //                                 // See if theres any property we need to refer to.
        //                                 var comArrElArr = comArrEl.Split("=>");
        //
        //                                 if (int.TryParse(comArrElArr[0], out var index))
        //                                 {
        //                                     // Traverse first
        //                                     var rawData = processingToken.ToObject<List<JToken>>()[index];
        //
        //                                     // if its 1, we assume its just an array of a primitive type
        //                                     if (comArrElArr.Length == 1)
        //                                     {
        //                                         // Retrieve the value.
        //                                         var rawVal = rawData.ToString();
        //
        //                                         // https://stackoverflow.com/questions/23131414/culture-invariant-decimal-tryparse
        //                                         var style = NumberStyles.Any;
        //                                         if (ExponentHelper.IsExponentialFormat(rawVal))
        //                                         {
        //                                             style = NumberStyles.Float;
        //                                         }
        //
        //                                         // If it is an exponent
        //                                         if (decimal.TryParse(rawVal, style, CultureInfo.InvariantCulture,
        //                                             out var val))
        //                                         {
        //                                             if (val > 0)
        //                                             {
        //                                                 // Update it
        //                                                 componentService.UpdatePairValue(component.Id, val);
        //                                             }
        //                                         }
        //                                     }
        //                                     // Oh no.. non-primitive...
        //                                     else if (comArrElArr.Length == 2)
        //                                     {
        //                                         // Object-ify
        //                                         var rawObj = JObject.Parse(rawData.ToString());
        //
        //                                         // Obtain the desired value
        //                                         var rawVal = rawObj[comArrElArr[1]].ToString();
        //
        //                                         // As usual, update it
        //                                         // https://stackoverflow.com/questions/23131414/culture-invariant-decimal-tryparse
        //                                         var style = NumberStyles.Any;
        //                                         if (ExponentHelper.IsExponentialFormat(rawVal))
        //                                         {
        //                                             style = NumberStyles.Float;
        //                                         }
        //
        //                                         // If it is an exponent
        //                                         if (decimal.TryParse(rawVal, style, CultureInfo.InvariantCulture,
        //                                             out var val))
        //                                         {
        //                                             if (val > 0)
        //                                             {
        //                                                 // Update it
        //                                                 componentService.UpdatePairValue(component.Id, val);
        //                                             }
        //                                         }
        //                                     }
        //                                     else
        //                                     {
        //                                         // Invalid
        //                                         //return false;
        //                                     }
        //                                 }
        //                             }
        //                         }
        //                         catch (Exception ex)
        //                         {
        //                             Console.WriteLine(ex);
        //                         }
        //                     }
        //                     else if (processingToken is JObject)
        //                     {
        //                         // Pump in the object
        //                         var obj = processingToken.ToObject<JObject>();
        //
        //                         // Is it the last?
        //                         if (comArrEl != last)
        //                         {
        //                             // let's work it out
        //                             // update the token
        //                             processingToken = obj.SelectToken(comArrEl);
        //                         }
        //                         // Yes its the last
        //                         else
        //                         {
        //                             // See if theres any property we need to refer to.
        //                             var comArrElArr = comArrEl.Split("=>");
        //
        //                             // Traverse first
        //                             var rawData = (string) obj.GetValue(comArrElArr[0]);
        //
        //                             if (rawData != null)
        //                             {
        //                                 // if its 1, we assume its just an array of a primitive type
        //                                 if (comArrElArr.Length == 1)
        //                                 {
        //                                     // Retrieve the value.
        //                                     var rawVal = rawData.ToString();
        //
        //                                     // https://stackoverflow.com/questions/23131414/culture-invariant-decimal-tryparse
        //                                     var style = NumberStyles.Any;
        //                                     if (ExponentHelper.IsExponentialFormat(rawVal))
        //                                     {
        //                                         style = NumberStyles.Float;
        //                                     }
        //
        //                                     // If it is an exponent
        //                                     if (decimal.TryParse(rawVal, style, CultureInfo.InvariantCulture,
        //                                         out var val))
        //                                     {
        //                                         if (val > 0)
        //                                         {
        //                                             // Update it
        //                                             componentService.UpdatePairValue(component.Id, val);
        //                                         }
        //                                     }
        //                                 }
        //                                 // Oh no.. non-primitive...
        //                                 else if (comArrElArr.Length == 2)
        //                                 {
        //                                     // Object-ify
        //                                     var rawObj = JObject.Parse(rawData.ToString());
        //
        //                                     // Obtain the desired value
        //                                     var rawVal = rawObj[comArrElArr[1]].ToString();
        //
        //                                     // As usual, update it
        //                                     // https://stackoverflow.com/questions/23131414/culture-invariant-decimal-tryparse
        //                                     var style = NumberStyles.Any;
        //                                     if (ExponentHelper.IsExponentialFormat(rawVal))
        //                                     {
        //                                         style = NumberStyles.Float;
        //                                     }
        //
        //                                     // If it is an exponent
        //                                     if (decimal.TryParse(rawVal, style, CultureInfo.InvariantCulture,
        //                                         out var val))
        //                                     {
        //                                         if (val > 0)
        //                                         {
        //                                             // Update it
        //                                             componentService.UpdatePairValue(component.Id, val);
        //                                         }
        //                                     }
        //                                 }
        //                                 else
        //                                 {
        //                                     // Invalid
        //                                     //return false;
        //                                 }
        //                             }
        //                         }
        //                     }
        //                     // iterate JValue like a JObject
        //                     else if (processingToken is JValue)
        //                     {
        //                         // Pump in the object
        //                         //JObject obj = processingToken.ToObject<JObject>();
        //                         var obj = processingToken;
        //
        //                         // Is it the last?
        //                         if (comArrEl != last)
        //                         {
        //                             // let's work it out
        //                             // update the token
        //                             processingToken = obj.SelectToken(comArrEl);
        //                         }
        //                         // Yes its the last
        //                         else
        //                         {
        //                             var rawData = (string) obj.SelectToken(component.QueryComponent);
        //
        //                             if (rawData != null)
        //                             {
        //                                 // https://stackoverflow.com/questions/23131414/culture-invariant-decimal-tryparse
        //                                 var style = NumberStyles.Any;
        //                                 if (ExponentHelper.IsExponentialFormat(rawData))
        //                                 {
        //                                     style = NumberStyles.Float;
        //                                 }
        //
        //                                 // If it is an exponent
        //                                 if (decimal.TryParse(rawData, style, CultureInfo.InvariantCulture,
        //                                     out decimal val))
        //                                 {
        //                                     if (val > 0)
        //                                     {
        //                                         // Update it
        //                                         componentService.UpdatePairValue(component.Id, val);
        //                                     }
        //                                 }
        //                             }
        //                         }
        //                     }
        //                 }
        //                 else
        //                 {
        //                     // Something bad happened
        //                     //return false;
        //                 }
        //             }
        //         }
        //         else if (string.IsNullOrEmpty(component.Identifier))
        //         {
        //             _logger.LogInformation($"Marking Request Component as checked: {component.Id}");
        //             return componentService.Checked(component.Id);
        //         }
        //     }
        //
        //     return true;
        // }
    }
}