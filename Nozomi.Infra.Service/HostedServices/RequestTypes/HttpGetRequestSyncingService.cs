using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Base.Core.Helpers.Exponent;
using Nozomi.Base.Core.Helpers.Native.Collections;
using Nozomi.Data;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Logging;
using Nozomi.Infra.Preprocessing.SignalR.Hubs.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.HostedServices.RequestTypes.Interfaces;
using Nozomi.Service.Hubs;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;
using StackExchange.Redis;
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
    public class HttpGetRequestSyncingService : BaseHostedService<HttpGetRequestSyncingService>,
        IHttpGetRequestSyncingService, IHostedService, IDisposable
    {
        private readonly NozomiDbContext _nozomiDbContext;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ICurrencyRequestEvent _currencyRequestEvent;
        private readonly IRequestComponentService _requestComponentService;
        private readonly ICurrencyPairRequestService _currencyPairRequestService;
        private readonly IRequestService _requestService;
        private readonly IRequestLogService _requestLogService;

        public HttpGetRequestSyncingService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _nozomiDbContext = _scope.ServiceProvider.GetService<NozomiDbContext>();
            _currencyRequestEvent = _scope.ServiceProvider.GetRequiredService<ICurrencyRequestEvent>();
            _requestComponentService = _scope.ServiceProvider.GetRequiredService<IRequestComponentService>();
            _currencyPairRequestService = _scope.ServiceProvider.GetRequiredService<ICurrencyPairRequestService>();
            _requestService = _scope.ServiceProvider.GetRequiredService<IRequestService>();
            _requestLogService = _scope.ServiceProvider.GetRequiredService<IRequestLogService>();
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
                    // We will need to re-synchronize the Request collection to make sure we're polling only
                    // the ones we want to poll
                    var cpRequests = _currencyPairRequestService
                        .GetAllByRequestTypeUniqueToURL(RequestType.HttpGet);
                    
                    // Iterate the requests
                    // NOTE: Let's not call a parallel loop since HttpClients might tend to result in memory leaks.
                    foreach (var cpRequest in cpRequests)
                    {
                        // Process the request
                        if (await ProcessRequest(cpRequest.Value))
                        {
                            // TODO: Broadcasting
                        }
                    }

                    var cRequests = _currencyRequestEvent.GetAllByRequestTypeUniqueToUrl(_nozomiDbContext, 
                        RequestType.HttpGet);

                    foreach (var cRequest in cRequests)
                    {
                        if (await ProcessRequest(cRequest.Value))
                        {
                            // TODO: Broadcasting
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("[HttpGetCurrencyPairRequestSyncingService]: " + ex);
                }

                // No naps taken
                await Task.Delay(10, stoppingToken);
            }

            _logger.LogWarning("HttpGetCurrencyPairRequestSyncingService background task is stopping.");
        }
        
        /// <summary>
        /// Every URL path may have multiple requests attempting to update separate entities.
        /// This method introduces a way of handling the collection of requests bundled together according to
        /// their endpoint paths, allowing us to optimise data polling at a granular level by completely removing
        /// multiple API requests.
        /// </summary>
        /// <param name="requests"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<bool> ProcessRequest<T>(ICollection<T> requests) where T : Request
        {
            if (requests != null && requests.Count > 0)
            {
                // Randomly obtain the first.
                var firstRequest = requests.FirstOrDefault();

                // quit if shit
                if (firstRequest == null) return false;

                _logger.LogInformation($"HttpGetCurrencyPairRequestSyncingService PROCESSING: {firstRequest.DataPath}");

                // FLUSH
                _httpClient.DefaultRequestHeaders.Clear();

                // Setup the url
                var uri = new UriBuilder(firstRequest.DataPath);
                var urlParams = HttpUtility.ParseQueryString(string.Empty);

                // Setup the request properties
                foreach (var reqProp in firstRequest.RequestProperties)
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
                        case RequestPropertyType.HttpHeader_Custom:
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

                switch (payload.StatusCode)
                {
                    case HttpStatusCode.OK:
                        if (requests.Any(r => r.RequestComponents
                            .Any(rc => rc.DeletedAt == null && rc.IsEnabled)))
                        {
                            // Pull the content
                            var content = await payload.Content.ReadAsStringAsync();
                            var resType = ResponseType.Json;

                            // Pull the components wanted
                            var requestComponents = requests
                                .SelectMany(r => r.RequestComponents
                                    .Where(rc => rc.DeletedAt == null && rc.IsEnabled));

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
                        }
                        
                        // Else error
                        return false;
                    case HttpStatusCode.TooManyRequests:
                        // Rate limited. Push back update timings
                        return _requestService.Delay(firstRequest, TimeSpan.FromMilliseconds(firstRequest.FailureDelay));
                    default:
                        // ded
                        return false;
                }
            }

            // Log the failure
            _logger.LogCritical("[HttpGetCurrencyPairRequestSyncingService] CRITICAL FAILURE!");

            return false;
        }

        public bool Update(JToken token, ResponseType resType, IEnumerable<RequestComponent> requestComponents)
        {
            // Save it first
            var currToken = token;
            
            // For each component we're checking
            foreach (var component in requestComponents)
            {
                // Always reset
                currToken = token;
                
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
                        if (currToken is JArray)
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
                                        var dataList = currToken.ToObject<List<JObject>>();

                                        // let's work it out
                                        // update the token
                                        if (index >= 0 && index < dataList.Count)
                                        {
                                            // Traverse the array
                                            currToken = JToken.Parse(JsonConvert.SerializeObject(dataList[index]));
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
                                        var rawData = currToken.ToObject<List<JToken>>()[index];

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
                                                    var res = _requestComponentService.UpdatePairValue(component.Id, val);

                                                    if (res.ResultType.Equals(NozomiResultType.Failed))
                                                    {
                                                        _logger.LogError(res.Message);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                _logger.LogError($"Component: {component.Id}" +
                                                                 $" // Raw Value: {rawVal} | Invalid component value.");
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
                                                    var res = _requestComponentService.UpdatePairValue(component.Id, val);

                                                    if (res.ResultType.Equals(NozomiResultType.Failed))
                                                    {
                                                        _logger.LogError(res.Message);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                _logger.LogError($"Component: {component.Id}" +
                                                                 $" // Raw Value: {rawVal} | Invalid component value.");
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
                        else if (currToken is JObject)
                        {
                            // Pump in the object
                            JObject obj = currToken.ToObject<JObject>();

                            // Is it the last?
                            if (comArrEl != last)
                            {
                                // let's work it out
                                // update the token
                                currToken = obj.SelectToken(comArrEl);
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
                                                var res = _requestComponentService.UpdatePairValue(component.Id, val);

                                                if (res.ResultType.Equals(NozomiResultType.Failed))
                                                {
                                                    _logger.LogError(res.Message);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            _logger.LogError($"Component: {component.Id}" +
                                                             $" // Raw Value: {rawVal} | Invalid component value.");
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
                                                var res = _requestComponentService.UpdatePairValue(component.Id, val);

                                                if (res.ResultType.Equals(NozomiResultType.Failed))
                                                {
                                                    _logger.LogError(res.Message);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            _logger.LogError($"Component: {component.Id}" +
                                                             $" // Raw Value: {rawVal} | Invalid component value.");
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
                        else if (currToken is JValue)
                        {
                            // Pump in the object
                            JObject obj = currToken.ToObject<JObject>();

                            // Is it the last?
                            if (comArrEl != last)
                            {
                                // let's work it out
                                // update the token
                                currToken = obj.SelectToken(comArrEl);
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
                                            var res = _requestComponentService.UpdatePairValue(component.Id, val);

                                            if (res.ResultType.Equals(NozomiResultType.Failed))
                                            {
                                                _logger.LogError(res.Message);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        _logger.LogError($"Component: {component.Id}" +
                                                         $" // Raw Value: {rawData} | Invalid component value.");
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