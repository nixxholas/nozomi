using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Counter.SDK.Utils.Numerics;
using CounterCore.Service.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Base.Core.Helpers.Native.Collections;
using Nozomi.Data.HubModels.Interfaces;
using Nozomi.Data.WebModels;
using Nozomi.Data.WebModels.LoggingModels;
using Nozomi.Service.HostedServices.RequestTypes.Interfaces;
using Nozomi.Service.Hubs;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

/*
 * HttpGetCurrencyPairRequestSyncingService
 * Created by Nicholas Chen, 2018
 *
 * This Background service acts as a cron job to process, validate and update currency pair data on the fly,
 * with the ability to support dozens of API headers on the HTTP protocol, omitting the standard way of writing one
 * wrapper for one vendor. Although it is able to handle all kinds of vendors with variants in their API structure,
 * HttpGetCurrencyPairRequestSyncingService assumes that the API has limitless API access and would call as much as it
 * wants when it has to.
 *
 * Further development of writing a feature to prevent this limitless API calling will be done.
 */
namespace Nozomi.Service.HostedServices.RequestTypes
{
    public class HttpGetCurrencyPairRequestSyncingService : BaseHostedService<HttpGetCurrencyPairRequestSyncingService>,
        IHttpGetCurrencyPairRequestSyncingService, IHostedService, IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ICurrencyPairComponentService _currencyPairComponentService;
        private readonly ICurrencyPairRequestService _currencyPairRequestService;
        private readonly IRequestLogService _requestLogService;
        private readonly IHubContext<TickerHub, ITickerHubClient> _tickerHub;

        public HttpGetCurrencyPairRequestSyncingService(IServiceProvider serviceProvider,
            IHubContext<TickerHub, ITickerHubClient> tickerHub) : base(serviceProvider)
        {
            _currencyPairComponentService = _scope.ServiceProvider.GetRequiredService<ICurrencyPairComponentService>();
            _currencyPairRequestService = _scope.ServiceProvider.GetRequiredService<ICurrencyPairRequestService>();
            _requestLogService = _scope.ServiceProvider.GetRequiredService<IRequestLogService>();

            _tickerHub = tickerHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("HttpGetCurrencyPairRequestSyncingService is starting.");

            stoppingToken.Register(
                () => _logger.LogInformation("HttpGetCurrencyPairRequestSyncingService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // We will need to resync the Request collection to make sure we're polling only the ones we want to poll
//                  var getBasedRequests = _currencyPairRequestService.GetAllActive(r => r.IsEnabled && r.DeletedAt == null
//                                            && r.RequestType == RequestType.HttpGet
//                                            && r.RequestComponents.Any(rc => !rc.RequestComponentData.Any() 
//                                            || (DateTime.UtcNow > rc.RequestComponentData.OrderByDescending(rcd => rcd.CreatedAt)
//                                                 .FirstOrDefault().CreatedAt.AddMilliseconds(r.Delay))), true)
//                                            .ToList();
                    var getBasedRequests = _currencyPairRequestService.GetAllByRequestType(RequestType.HttpGet);

                    // Iterate the requests
                    // NOTE: Let's not call a parallel loop since HttpClients might tend to result in memory leaks.
                    foreach (var rq in getBasedRequests)
                    {
                        // Process the request
                        if (await Process(rq))
                        {
                            // Since its successful, broadcast its success
                            // await _tickerHub.Clients.All.BroadcastData(rq.ObscureToPublicJson());
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("[HttpGetCurrencyPairRequestSyncingService]: " + ex);
                }

                // No naps taken
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogWarning("HttpGetCurrencyPairRequestSyncingService background task is stopping.");
        }

        public async Task<bool> Process(CurrencyPairRequest req)
        {
            if (req != null && req.IsValidForPolling())
            {
                Console.WriteLine("HttpGetCurrencyPairRequestSyncingService PROCESSING: " + req.Id);

                // FLUSH
                _httpClient.DefaultRequestHeaders.Clear();
                
                // Setup the url
                var uri = new UriBuilder(req.DataPath);
                var urlParams = HttpUtility.ParseQueryString(string.Empty);

                // Setup the request properties
                foreach (var reqProp in req.RequestProperties)
                {
                    switch (reqProp.RequestPropertyType)
                    {
                        // Adds a custom header
                        case RequestPropertyType.HttpHeader:
                            _httpClient.DefaultRequestHeaders.Add(reqProp.Key, reqProp.Value);
                            break;
                        // Adds a custom acceptance type
                        case RequestPropertyType.HttpHeader_Accept:
                            _httpClient.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue(reqProp.Value));
                            break;
                        // Adds a custom charset acceptance type
                        case RequestPropertyType.HttpHeader_AcceptCharset:
                            _httpClient.DefaultRequestHeaders.AcceptCharset.Add(
                                new StringWithQualityHeaderValue(reqProp.Value));
                            break;
                        // Adds a custom encoding acceptance type
                        case RequestPropertyType.HttpHeader_AcceptEncoding:
                            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(
                                new StringWithQualityHeaderValue(reqProp.Value));
                            break;
                        // Adds a custom language acceptance type
                        case RequestPropertyType.HttpHeader_AcceptLanguage:
                            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(
                                new StringWithQualityHeaderValue(reqProp.Value));
                            break;
                        // Adds a header authorization value
                        case RequestPropertyType.HttpHeader_Authorization:
                            _httpClient.DefaultRequestHeaders.Authorization =
                                new AuthenticationHeaderValue(reqProp.Key, reqProp.Value);
                            break;
                        // Modifies the cache control header values
                        case RequestPropertyType.HttpHeader_CacheControl:
                            var cchv = JsonConvert.DeserializeObject<CacheControlHeaderValue>(reqProp.Value);

                            if (cchv != null)
                            {
                                _httpClient.DefaultRequestHeaders.CacheControl = cchv;
                            }

                            break;
                        // Adds a custom connection header value
                        case RequestPropertyType.HttpHeader_Connection:
                            _httpClient.DefaultRequestHeaders.Connection.Add(reqProp.Value);
                            break;
                        // Declares whether if the data path has a close value
                        case RequestPropertyType.HttpHeader_ConnectionClose:
                            if (bool.TryParse(reqProp.Value, out var res))
                            {
                                _httpClient.DefaultRequestHeaders.ConnectionClose = res;
                            }

                            break;
                        // Declares the datetimeoffset
                        case RequestPropertyType.HttpHeader_Date:
                            if (DateTimeOffset.TryParse(reqProp.Value, out var dtoRes))
                            {
                                _httpClient.DefaultRequestHeaders.Date = dtoRes;
                            }

                            break;
                        // Declares the Expect Header
                        case RequestPropertyType.HttpHeader_Expect:
                            _httpClient.DefaultRequestHeaders.Expect
                                .Add(new NameValueWithParametersHeaderValue(reqProp.Key, reqProp.Value));
                            break;
                        // Declares the ExpectContinue Header to see if we need to continue or not
                        // after getting what we're expecting
                        case RequestPropertyType.HttpHeader_ExpectContinue:
                            if (bool.TryParse(reqProp.Value, out var ecRes))
                            {
                                _httpClient.DefaultRequestHeaders.ExpectContinue = ecRes;
                            }

                            break;
                        // Declares the From Header, where this request came from or who.. idk.. whatever the API
                        // asks to put in the From header.
                        case RequestPropertyType.HttpHeader_From:
                            _httpClient.DefaultRequestHeaders.From = reqProp.Value;
                            break;
                        // Declares the Host Header
                        case RequestPropertyType.HttpHeader_Host:
                            _httpClient.DefaultRequestHeaders.Host = reqProp.Value;
                            break;
                        // Declares the If-Match Header
                        case RequestPropertyType.HttpHeader_IfMatch:
                            _httpClient.DefaultRequestHeaders.IfMatch
                                .Add(new EntityTagHeaderValue(reqProp.Value));
                            break;
                        // Declares the If-Modified-Since Header
                        case RequestPropertyType.HttpHeader_IfModifiedSince:
                            if (DateTimeOffset.TryParse(reqProp.Value, out var imsRes))
                            {
                                _httpClient.DefaultRequestHeaders.IfModifiedSince = imsRes;
                            }

                            break;
                        // Declares the If-None-Match Header
                        case RequestPropertyType.HttpHeader_IfNoneMatch:
                            _httpClient.DefaultRequestHeaders.IfNoneMatch
                                .Add(new EntityTagHeaderValue(reqProp.Value));
                            break;
                        // Declares the If-Range Header
                        case RequestPropertyType.HttpHeader_IfRange:
                            if (RangeConditionHeaderValue.TryParse(reqProp.Value, out var rchvRes))
                            {
                                _httpClient.DefaultRequestHeaders.IfRange = rchvRes;
                            }

                            break;
                        // Declares the If-Unmodified-Since Header
                        case RequestPropertyType.HttpHeader_IfUnmodifiedSince:
                            if (DateTimeOffset.TryParse(reqProp.Value, out var iumsRes))
                            {
                                _httpClient.DefaultRequestHeaders.IfUnmodifiedSince = iumsRes;
                            }

                            break;
                        // Declares the Max-Forwards Header, the maximum number of forwarding allowed.
                        case RequestPropertyType.HttpHeader_MaxForwards:
                            if (int.TryParse(reqProp.Value, out var mfRes))
                            {
                                _httpClient.DefaultRequestHeaders.MaxForwards = mfRes;
                            }

                            break;
                        // Declares a Pragma Header
                        case RequestPropertyType.HttpHeader_Pragma:
                            _httpClient.DefaultRequestHeaders.Pragma
                                .Add(new NameValueHeaderValue(reqProp.Key, reqProp.Value));
                            break;
                        // Declares the Proxy-Authorization Header values
                        case RequestPropertyType.HttpHeader_ProxyAuthorization:
                            _httpClient.DefaultRequestHeaders.ProxyAuthorization =
                                new AuthenticationHeaderValue(reqProp.Key, reqProp.Value);

                            break;
                        // Declares the Range Header
                        case RequestPropertyType.HttpHeader_Range:
                            var rhvRes = JsonConvert.DeserializeObject<RangeHeaderValue>(reqProp.Value);

                            if (rhvRes != null)
                            {
                                _httpClient.DefaultRequestHeaders.Range = rhvRes;
                            }

                            break;
                        // Declares the Referrer Header, who referred this request
                        case RequestPropertyType.HttpHeader_Referrer:
                            _httpClient.DefaultRequestHeaders.Referrer = new Uri(reqProp.Value);
                            break;
                        // Declares the TE Header
                        case RequestPropertyType.HttpHeader_TE:
                            _httpClient.DefaultRequestHeaders.TE
                                .Add(new TransferCodingWithQualityHeaderValue(reqProp.Value));
                            break;
                        // Declares the Trailer Header
                        case RequestPropertyType.HttpHeader_Trailer:
                            _httpClient.DefaultRequestHeaders.Trailer.Add(reqProp.Value);
                            break;
                        // Declares the Transfer-Encoding Header value
                        case RequestPropertyType.HttpHeader_TransferEncoding:
                            _httpClient.DefaultRequestHeaders.TransferEncoding
                                .Add(new TransferCodingHeaderValue(reqProp.Value));
                            break;
                        // Declares the Transfer-Encoding-Chunked Header value, whether the Transfer-Encoding is
                        // chunked or not..
                        case RequestPropertyType.HttpHeader_TransferEncodingChunked:
                            if (bool.TryParse(reqProp.Value, out var tecRes))
                            {
                                _httpClient.DefaultRequestHeaders.TransferEncodingChunked = tecRes;
                            }

                            break;
                        // Declares the Upgrade Header
                        case RequestPropertyType.HttpHeader_Upgrade:
                            _httpClient.DefaultRequestHeaders.Upgrade
                                .Add(new ProductHeaderValue(reqProp.Key, reqProp.Value));
                            break;
                        // Declares the User-Agent Header Values
                        case RequestPropertyType.HttpHeader_UserAgent:
                            _httpClient.DefaultRequestHeaders.UserAgent
                                .Add(string.IsNullOrEmpty(reqProp.Key)
                                    ? new ProductInfoHeaderValue(reqProp.Value)
                                    : new ProductInfoHeaderValue(reqProp.Key, reqProp.Value));

                            break;
                        // Declares a Via Header Value
                        case RequestPropertyType.HttpHeader_Via:
                            _httpClient.DefaultRequestHeaders.Via.Add(new ViaHeaderValue(reqProp.Key, reqProp.Value));
                            break;
                        // Declares a Warning Header value
                        case RequestPropertyType.HttpHeader_Warning:
                            var wRes = JsonConvert.DeserializeObject<WarningHeaderValue>(reqProp.Value);

                            if (wRes != null)
                            {
                                _httpClient.DefaultRequestHeaders.Warning.Add(wRes);
                            }

                            break;
                        case RequestPropertyType.HttpQuery:
                            urlParams.Add(reqProp.Key, reqProp.Value);
                            break;
                        default:
                            // Do nothing for now
                            break;
                    }
                }

                if (urlParams.Count > 0)
                {
                    // Setup the url
                    uri.Query = urlParams.ToString();
                }

                // Pull in the payload
                var payload = await _httpClient.GetAsync(uri.ToString());

                // Succcessful? and is there even any Components to update?
                if (payload.IsSuccessStatusCode && req?.RequestComponents != null && req.RequestComponents.Count > 0)
                {
                    // Pull the content
                    var content = await payload.Content.ReadAsStringAsync();
                    var resType = ResponseType.Json;

                    // Pull the components wanted
                    var requestComponents = req.RequestComponents
                        .Where(cpc => cpc.DeletedAt == null && cpc.IsEnabled);

                    // Parse the content
                    if (payload.Content.Headers.ContentType.MediaType.Equals(ResponseType.Json.GetDescription()))
                    {
                        // No action needed
                    }
                    else if (payload.Content.Headers.ContentType.MediaType.Equals(ResponseType.XML.GetDescription()))
                    {
                        // Load the XML
                        //var xmlElement = XElement.Parse(content);
                        resType = ResponseType.XML;
                        var xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(content);
                        content = JsonConvert.SerializeObject(xmlDoc);
                    }

                    var contentToken = JToken.Parse(content);

                    if (Update(contentToken, resType, requestComponents)) return true;

                    // Else error
                }
                else
                {
                    // Log the failure
                    if (_requestLogService.Create(new RequestLog()
                    {
                        Type = RequestLogType.Failure,
                        RawPayload = JsonConvert.SerializeObject(payload),
                        RequestId = req.Id
                    }) <= 0)
                    {
                        // Logging Failure!!!!
                    }
                }
            }

            // Log the failure
            if (_requestLogService.Create(new RequestLog()
            {
                Type = RequestLogType.Failure,
                RawPayload = null,
                RequestId = req?.Id ?? 0
            }) <= 0)
            {
                // Logging Failure!!!!
            }

            return false;
        }

        public bool Update(JToken token, ResponseType resType, IEnumerable<RequestComponent> requestComponents)
        {
            // For each component we're checking
            foreach (var component in requestComponents)
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
                        if (token is JArray)
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
                                        var dataList = token.ToObject<List<JObject>>();

                                        // let's work it out
                                        // update the token
                                        if (index >= 0 && index < dataList.Count)
                                        {
                                            // Traverse the array
                                            token = JToken.Parse(JsonConvert.SerializeObject(dataList[index]));
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
                                        var rawData = token.ToObject<List<JToken>>()[index];

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
                        else if (token is JObject)
                        {
                            // Pump in the object
                            JObject obj = token.ToObject<JObject>();

                            // Is it the last?
                            if (comArrEl != last)
                            {
                                // let's work it out
                                // update the token
                                token = obj.SelectToken(comArrEl);
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
                        else if (token is JValue)
                        {
                            // Pump in the object
                            JObject obj = token.ToObject<JObject>();

                            // Is it the last?
                            if (comArrEl != last)
                            {
                                // let's work it out
                                // update the token
                                token = obj.SelectToken(comArrEl);
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

            return false;
        }
    }
}