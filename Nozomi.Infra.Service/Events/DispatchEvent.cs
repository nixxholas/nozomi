using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nozomi.Base.BCL.Helpers.Enumerator;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Dispatch;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Service.Events.Interfaces;
using sta_websocket_sharp_core;

namespace Nozomi.Service.Events
{
    public class DispatchEvent : BaseEvent<DispatchEvent>, IDispatchEvent
    {
        public DispatchEvent(ILogger<DispatchEvent> logger) : base(logger)
        {
        }

        public async Task<DispatchViewModel> Dispatch(DispatchInputModel dispatchInputModel)
        {
            if (dispatchInputModel != null && dispatchInputModel.IsValid())
            {
                switch (dispatchInputModel.RequestType)
                {
                    case RequestType.HttpGet:
                    case RequestType.HttpPost:
                    case RequestType.HttpPut:
                    case RequestType.HttpPatch:
                    case RequestType.HttpDelete:
                        using (var httpClient = new HttpClient())
                        {
                            // FLUSH
                            httpClient.DefaultRequestHeaders.Clear();
                            string body = string.Empty, customMediaType = string.Empty; // For POST/PUT

                            // Setup the url
                            var uri = new UriBuilder(dispatchInputModel.Endpoint);
                            uri.Port = -1;
                            var urlParams = HttpUtility.ParseQueryString(string.Empty);

                            // Setup the request headers and miscellaneous properties
                            foreach (var reqProp in dispatchInputModel.Properties)
                            {
                                switch (reqProp.Type)
                                {
                                    // Adds a custom header
                                    case RequestPropertyType.HttpHeader:
                                        httpClient.DefaultRequestHeaders.Add(reqProp.Key, reqProp.Value);
                                        break;
                                    // Adds a custom acceptance type
                                    case RequestPropertyType.HttpHeader_Accept:
                                        httpClient.DefaultRequestHeaders.Accept.Add(
                                            new MediaTypeWithQualityHeaderValue(reqProp.Value));
                                        break;
                                    // Adds a custom charset acceptance type
                                    case RequestPropertyType.HttpHeader_AcceptCharset:
                                        httpClient.DefaultRequestHeaders.AcceptCharset.Add(
                                            new StringWithQualityHeaderValue(reqProp.Value));
                                        break;
                                    // Adds a custom encoding acceptance type
                                    case RequestPropertyType.HttpHeader_AcceptEncoding:
                                        httpClient.DefaultRequestHeaders.AcceptEncoding.Add(
                                            new StringWithQualityHeaderValue(reqProp.Value));
                                        break;
                                    // Adds a custom language acceptance type
                                    case RequestPropertyType.HttpHeader_AcceptLanguage:
                                        httpClient.DefaultRequestHeaders.AcceptLanguage.Add(
                                            new StringWithQualityHeaderValue(reqProp.Value));
                                        break;
                                    // Adds a header authorization value
                                    case RequestPropertyType.HttpHeader_Authorization:
                                        httpClient.DefaultRequestHeaders.Authorization =
                                            new AuthenticationHeaderValue(reqProp.Key, reqProp.Value);
                                        break;
                                    // Modifies the cache control header values
                                    case RequestPropertyType.HttpHeader_CacheControl:
                                        var cchv = JsonConvert
                                            .DeserializeObject<CacheControlHeaderValue>(reqProp.Value);

                                        if (cchv != null)
                                        {
                                            httpClient.DefaultRequestHeaders.CacheControl = cchv;
                                        }

                                        break;
                                    // Adds a custom connection header value
                                    case RequestPropertyType.HttpHeader_Connection:
                                        httpClient.DefaultRequestHeaders.Connection.Add(reqProp.Value);
                                        break;
                                    // Declares whether if the data path has a close value
                                    case RequestPropertyType.HttpHeader_ConnectionClose:
                                        if (bool.TryParse(reqProp.Value, out var res))
                                        {
                                            httpClient.DefaultRequestHeaders.ConnectionClose = res;
                                        }

                                        break;
                                    // Declares the datetimeoffset
                                    case RequestPropertyType.HttpHeader_Date:
                                        if (DateTimeOffset.TryParse(reqProp.Value, out var dtoRes))
                                        {
                                            httpClient.DefaultRequestHeaders.Date = dtoRes;
                                        }

                                        break;
                                    // Declares the Expect Header
                                    case RequestPropertyType.HttpHeader_Expect:
                                        httpClient.DefaultRequestHeaders.Expect
                                            .Add(new NameValueWithParametersHeaderValue(reqProp.Key, reqProp.Value));
                                        break;
                                    // Declares the ExpectContinue Header to see if we need to continue or not
                                    // after getting what we're expecting
                                    case RequestPropertyType.HttpHeader_ExpectContinue:
                                        if (bool.TryParse(reqProp.Value, out var ecRes))
                                        {
                                            httpClient.DefaultRequestHeaders.ExpectContinue = ecRes;
                                        }

                                        break;
                                    // Declares the From Header, where this request came from or who.. idk.. whatever the API
                                    // asks to put in the From header.
                                    case RequestPropertyType.HttpHeader_From:
                                        httpClient.DefaultRequestHeaders.From = reqProp.Value;
                                        break;
                                    // Declares the Host Header
                                    case RequestPropertyType.HttpHeader_Host:
                                        httpClient.DefaultRequestHeaders.Host = reqProp.Value;
                                        break;
                                    // Declares the If-Match Header
                                    case RequestPropertyType.HttpHeader_IfMatch:
                                        httpClient.DefaultRequestHeaders.IfMatch
                                            .Add(new EntityTagHeaderValue(reqProp.Value));
                                        break;
                                    // Declares the If-Modified-Since Header
                                    case RequestPropertyType.HttpHeader_IfModifiedSince:
                                        if (DateTimeOffset.TryParse(reqProp.Value, out var imsRes))
                                        {
                                            httpClient.DefaultRequestHeaders.IfModifiedSince = imsRes;
                                        }

                                        break;
                                    // Declares the If-None-Match Header
                                    case RequestPropertyType.HttpHeader_IfNoneMatch:
                                        httpClient.DefaultRequestHeaders.IfNoneMatch
                                            .Add(new EntityTagHeaderValue(reqProp.Value));
                                        break;
                                    // Declares the If-Range Header
                                    case RequestPropertyType.HttpHeader_IfRange:
                                        if (RangeConditionHeaderValue.TryParse(reqProp.Value, out var rchvRes))
                                        {
                                            httpClient.DefaultRequestHeaders.IfRange = rchvRes;
                                        }

                                        break;
                                    // Declares the If-Unmodified-Since Header
                                    case RequestPropertyType.HttpHeader_IfUnmodifiedSince:
                                        if (DateTimeOffset.TryParse(reqProp.Value, out var iumsRes))
                                        {
                                            httpClient.DefaultRequestHeaders.IfUnmodifiedSince = iumsRes;
                                        }

                                        break;
                                    // Declares the Max-Forwards Header, the maximum number of forwarding allowed.
                                    case RequestPropertyType.HttpHeader_MaxForwards:
                                        if (int.TryParse(reqProp.Value, out var mfRes))
                                        {
                                            httpClient.DefaultRequestHeaders.MaxForwards = mfRes;
                                        }

                                        break;
                                    // Declares a Pragma Header
                                    case RequestPropertyType.HttpHeader_Pragma:
                                        httpClient.DefaultRequestHeaders.Pragma
                                            .Add(new NameValueHeaderValue(reqProp.Key, reqProp.Value));
                                        break;
                                    // Declares the Proxy-Authorization Header values
                                    case RequestPropertyType.HttpHeader_ProxyAuthorization:
                                        httpClient.DefaultRequestHeaders.ProxyAuthorization =
                                            new AuthenticationHeaderValue(reqProp.Key, reqProp.Value);

                                        break;
                                    // Declares the Range Header
                                    case RequestPropertyType.HttpHeader_Range:
                                        var rhvRes = JsonConvert.DeserializeObject<RangeHeaderValue>(reqProp.Value);

                                        if (rhvRes != null)
                                        {
                                            httpClient.DefaultRequestHeaders.Range = rhvRes;
                                        }

                                        break;
                                    // Declares the Referrer Header, who referred this request
                                    case RequestPropertyType.HttpHeader_Referrer:
                                        httpClient.DefaultRequestHeaders.Referrer = new Uri(reqProp.Value);
                                        break;
                                    // Declares the TE Header
                                    case RequestPropertyType.HttpHeader_TE:
                                        httpClient.DefaultRequestHeaders.TE
                                            .Add(new TransferCodingWithQualityHeaderValue(reqProp.Value));
                                        break;
                                    // Declares the Trailer Header
                                    case RequestPropertyType.HttpHeader_Trailer:
                                        httpClient.DefaultRequestHeaders.Trailer.Add(reqProp.Value);
                                        break;
                                    // Declares the Transfer-Encoding Header value
                                    case RequestPropertyType.HttpHeader_TransferEncoding:
                                        httpClient.DefaultRequestHeaders.TransferEncoding
                                            .Add(new TransferCodingHeaderValue(reqProp.Value));
                                        break;
                                    // Declares the Transfer-Encoding-Chunked Header value, whether the Transfer-Encoding is
                                    // chunked or not..
                                    case RequestPropertyType.HttpHeader_TransferEncodingChunked:
                                        if (bool.TryParse(reqProp.Value, out var tecRes))
                                        {
                                            httpClient.DefaultRequestHeaders.TransferEncodingChunked = tecRes;
                                        }

                                        break;
                                    // Declares the Upgrade Header
                                    case RequestPropertyType.HttpHeader_Upgrade:
                                        httpClient.DefaultRequestHeaders.Upgrade
                                            .Add(new ProductHeaderValue(reqProp.Key, reqProp.Value));
                                        break;
                                    // Declares the User-Agent Header Values
                                    case RequestPropertyType.HttpHeader_UserAgent:
                                        httpClient.DefaultRequestHeaders.UserAgent
                                            .Add(string.IsNullOrEmpty(reqProp.Key)
                                                ? new ProductInfoHeaderValue(reqProp.Value)
                                                : new ProductInfoHeaderValue(reqProp.Key, reqProp.Value));

                                        break;
                                    // Declares a Via Header Value
                                    case RequestPropertyType.HttpHeader_Via:
                                        httpClient.DefaultRequestHeaders.Via.Add(
                                            new ViaHeaderValue(reqProp.Key, reqProp.Value));
                                        break;
                                    // Declares a Warning Header value
                                    case RequestPropertyType.HttpHeader_Warning:
                                        var wRes = JsonConvert.DeserializeObject<WarningHeaderValue>(reqProp.Value);

                                        if (wRes != null)
                                        {
                                            httpClient.DefaultRequestHeaders.Warning.Add(wRes);
                                        }

                                        break;
                                    // Declares the Http POST/PUT Body
                                    case RequestPropertyType.HttpBody:
                                        if (dispatchInputModel.RequestType.Equals(RequestType.HttpGet))
                                            _logger.LogWarning($"{_eventName} Dispatch: Setting body in a GET " +
                                                               "request..");

                                        body = reqProp.Value;
                                        break;
                                    case RequestPropertyType.HttpHeader_MediaType:
                                        customMediaType = reqProp.Value;
                                        break;
                                    case RequestPropertyType.HttpHeader_Custom:
                                    case RequestPropertyType.HttpQuery:
                                        urlParams.Add(reqProp.Key, reqProp.Value);
                                        break;
                                }
                            }

                            // Query parameters? idk
                            if (urlParams.Count > 0)
                            {
                                // Setup the url
                                uri.Query = urlParams.ToString();
                            }

                            // Pull in the payload
                            HttpResponseMessage payload;
                            switch (dispatchInputModel.RequestType)
                            {
                                case RequestType.HttpGet:
                                    payload = await httpClient.GetAsync(uri.ToString());
                                    break;
                                case RequestType.HttpPost:
                                    payload = await httpClient.PostAsync(uri.ToString(),
                                        new StringContent(body, Encoding.UTF8,
                                            string.IsNullOrEmpty(customMediaType)
                                                ? "application/json"
                                                : customMediaType));
                                    break;
                                case RequestType.HttpPut:
                                    payload = await httpClient.PutAsync(uri.ToString(),
                                        new StringContent(body, Encoding.UTF8,
                                            string.IsNullOrEmpty(customMediaType)
                                                ? "application/json"
                                                : customMediaType));
                                    break;
                                case RequestType.HttpDelete:
                                    payload = await httpClient.DeleteAsync(uri.ToString());
                                    break;
                                case RequestType.HttpPatch:
                                    payload = await httpClient.PatchAsync(uri.ToString(),
                                        new StringContent(body, Encoding.UTF8,
                                            string.IsNullOrEmpty(customMediaType)
                                                ? "application/json"
                                                : customMediaType));
                                    break;
                                default:
                                    throw new InvalidExpressionException("Invalid request type.");
                            }

#if DEBUG
                            var debugContent = await payload.Content.ReadAsStringAsync();
#endif

                            if (!payload.IsSuccessStatusCode)
                                return new DispatchViewModel
                                {
                                    Payload = Utf8Json.JsonSerializer.ToJsonString(payload)
                                };

                            switch (payload.StatusCode)
                            {
                                case HttpStatusCode.OK:
                                    // Return the payload distinctively
                                    switch (dispatchInputModel.ResponseType)
                                    {
                                        case ResponseType.Json:
                                            return new DispatchViewModel
                                            {
                                                Payload = Utf8Json.JsonSerializer
                                                    .ToJsonString(await payload.Content.ReadAsStringAsync()),
                                                Response = payload
                                            };
                                        case ResponseType.XML:
                                            // Old Newtonsoft code.
                                            var xmlDoc = new XmlDocument();
                                            xmlDoc.LoadXml(await payload.Content.ReadAsStringAsync());
                                            return new DispatchViewModel
                                            {
                                                Payload = Utf8Json.JsonSerializer.ToJsonString(xmlDoc),
                                                Response = payload
                                            };
                                        default:
                                            throw new InvalidEnumArgumentException($"{_eventName} Dispatch: " +
                                                                                   $"{dispatchInputModel.Endpoint} invalid" +
                                                                                   $" response type.");
                                    }

                                    ;
                                case HttpStatusCode.TooManyRequests:
                                    _logger.LogWarning($"{_eventName} Dispatch: " +
                                                       $"{dispatchInputModel.Endpoint} Too many requests");

                                    throw new DataException("Too many requests.");
                                default:
                                    throw new InvalidDataException("Invalid HTTP response.");
                            }
                        }
                    case RequestType.WebSocket:
                        // Initialise the websocket first
                        using (var socket = new WebSocketCore(dispatchInputModel.Endpoint)
                        {
                            Compression = CompressionMethod.Deflate,
                            EmitOnPing = true,
                            EnableRedirection = false,
                        })
                        {
                            // Setup the payload
                            var outgoingPayload = new DispatchViewModel
                            {
                                Response = new HttpResponseMessage()
                            };

                            // Initialise timer and datacounter here
                            var stopWatch = new Stopwatch();
                            var dataCounter = 0;

                            var concatPayload =
                                new List<string>(); // Setup the JSON arr which we're going to churn out.
                                // Pre-request processing
                                socket.OnOpen += (sender, args) =>
                                {
                                    // Start the timer if there actually is a killswitch timing
                                    if (dispatchInputModel.SocketKillSwitchDelay > 0 && !stopWatch.IsRunning)
                                        stopWatch.Start(); // OnMessage() will handle the closure

                                    if (dispatchInputModel.WebsocketCommands != null 
                                        && dispatchInputModel.WebsocketCommands.Count > 0)
                                        foreach (var wsCommand in dispatchInputModel.WebsocketCommands)
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
                                socket.OnMessage += async (sender, args) =>
                                {
                                    // If timer hits delay, activate kill switch. Or if the datacounter is beyond the 
                                    // SocketDataCount
                                    if (stopWatch.ElapsedMilliseconds >= dispatchInputModel.SocketKillSwitchDelay
                                        || dataCounter > dispatchInputModel.SocketDataCount)
                                        socket.Close(); // Close this since we hit the trigger

                                    if (args.IsPing) // Just in case the endpoint is a mother trucker
                                    {
                                        socket.Ping();
                                    }
                                    else if (!string.IsNullOrEmpty(args.Data)) // Process the incoming data
                                    {
                                        switch (dispatchInputModel.ResponseType)
                                        {
                                            case ResponseType.Json:
                                                // Push to the payload collection and move on.
                                                concatPayload.Add(Utf8Json.JsonSerializer.ToJsonString(args.Data));
                                                break;
                                            case ResponseType.XML:
                                                // Old Newtonsoft code.
                                                var xmlDoc = new XmlDocument();
                                                xmlDoc.LoadXml(args.Data);
                                                concatPayload.Add(Utf8Json.JsonSerializer.ToJsonString(xmlDoc));
                                                break;
                                            default:
                                                throw new InvalidEnumArgumentException($"{_eventName} Dispatch: " +
                                                                                       $"{dispatchInputModel.Endpoint} invalid" +
                                                                                       $" response type.");
                                        }
                                    }
                                    else
                                    {
                                        _logger.LogError($"{_eventName} Dispatch/OnMessage: Endpoint " +
                                                         $"{dispatchInputModel.Endpoint} has an empty payload incoming.");
                                        socket.Close();
                                    }

                                    // Update the payload as well
                                    outgoingPayload.Payload = Utf8Json.JsonSerializer.ToJsonString(concatPayload);

                                    if (dispatchInputModel.SocketDataCount > 0)
                                        dataCounter++; // Bump data counter
                                    
                                    // If there ain't a stopwatch and if a data count is inexistent
                                    if (!stopWatch.IsRunning && (dispatchInputModel.SocketDataCount == null
                                                                 || dispatchInputModel.SocketDataCount <= 0) &&
                                        dataCounter == 0)
                                        socket.Close();

                                    await Task.Delay(50,
                                        CancellationToken.None); // Always delay by 1ms in case of spam
                                };

                                // Error processing
                                socket.OnError += (sender, args) =>
                                {
                                    _logger.LogError($"{_eventName} Dispatch/OnError:" +
                                                     $" {args.Message}");
                                    outgoingPayload.Response.StatusCode = HttpStatusCode.ExpectationFailed;
                                    outgoingPayload.Response.ReasonPhrase = !string.IsNullOrEmpty(args.Message)
                                        ? args.Message
                                        : "The socket connection has been facing some unexpected problems that may require " +
                                          "your intervention to rectify.";
                                    GC.SuppressFinalize(this);
                                };

                                socket.OnClose += (sender, args) =>
                                {
                                    _logger.LogInformation($"{_eventName} Dispatch/onClose: " +
                                                           $"Closing socket connection for {dispatchInputModel.Endpoint}");
                                    stopWatch.Stop();
                                    GC.SuppressFinalize(this);
                                };

                                socket.Connect();

                            // Always ensure that the sockets move to receive data until closed
                            do
                            {
                            } while (socket != null && socket.IsAlive);

                            return outgoingPayload;
                        }
                    default:
                        throw new InvalidOperationException("Invalid protocol type.");
                }
            }

            throw new NullReferenceException("Invalid payload.");
        }
    }
}