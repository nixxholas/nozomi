using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Counter.SDK.Utils.Numerics;
using CounterCore.Service.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nozomi.Data.HubModels.Interfaces;
using Nozomi.Data.WebModels;
using Nozomi.Data.WebModels.LoggingModels;
using Nozomi.Service.HostedServices.RequestTypes.Interfaces;
using Nozomi.Service.Hubs;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Service.HostedServices.RequestTypes
{
    public class HttpGetCurrencyPairRequestSyncingService : BaseHostedService, IHttpGetCurrencyPairRequestSyncingService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ICurrencyPairComponentService _currencyPairComponentService;
        private readonly ICurrencyPairRequestService _currencyPairRequestService;
        private readonly IRequestLogService _requestLogService;
        private readonly ILogger<HttpGetCurrencyPairRequestSyncingService> _logger;
        private List<CurrencyPairRequest> _currencyPairRequestList;
        private readonly IHubContext<TickerHub, ITickerHubClient> _tickerHub;
        
        public HttpGetCurrencyPairRequestSyncingService(IServiceProvider serviceProvider,
            IHubContext<TickerHub, ITickerHubClient> tickerHub) : base(serviceProvider)
        {
            _currencyPairComponentService = _scope.ServiceProvider.GetRequiredService<ICurrencyPairComponentService>();
            _currencyPairRequestService = _scope.ServiceProvider.GetRequiredService<ICurrencyPairRequestService>();
            _requestLogService = _scope.ServiceProvider.GetRequiredService<IRequestLogService>();
            
            _logger = _scope.ServiceProvider.GetRequiredService<ILogger<HttpGetCurrencyPairRequestSyncingService>>();

            // Initialize the request list for all GET requests
            _currencyPairRequestList = _currencyPairRequestService.GetAllActive(true)
                               .Where(r => r.RequestType.Equals(RequestType.HttpGet))
                               .ToList() ?? new List<CurrencyPairRequest>();
            _tickerHub = tickerHub;
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
                        _tickerHub.Clients.All.BroadcastData(rq.ObscureToPublicJson());
                    }
                }
                

                await Task.Delay(5000);
            }

            _logger.LogWarning("HttpGetCurrencyPairRequestSyncingService background task is stopping.");
        }

        public async Task<bool> Process(CurrencyPairRequest req)
        {
            if (req != null && req.IsValidForPolling())
            {
                // FLUSH
                _httpClient.DefaultRequestHeaders.Clear();
                
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
                            _httpClient.DefaultRequestHeaders.Accept.
                                Add(new MediaTypeWithQualityHeaderValue(reqProp.Value)); 
                            break;
                        // Adds a custom charset acceptance type
                        case RequestPropertyType.HttpHeader_AcceptCharset:
                            _httpClient.DefaultRequestHeaders.AcceptCharset.
                                Add(new StringWithQualityHeaderValue(reqProp.Value)); 
                            break;
                        // Adds a custom encoding acceptance type
                        case RequestPropertyType.HttpHeader_AcceptEncoding:
                            _httpClient.DefaultRequestHeaders.AcceptEncoding.
                                Add(new StringWithQualityHeaderValue(reqProp.Value));
                            break;
                        // Adds a custom language acceptance type
                        case RequestPropertyType.HttpHeader_AcceptLanguage:
                            _httpClient.DefaultRequestHeaders.AcceptLanguage.
                                Add(new StringWithQualityHeaderValue(reqProp.Value)); 
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
                        default:
                            // Do nothing for now
                            break;
                    }
                }
                
                // Pull in the payload
                var payload = await _httpClient.GetAsync(req.DataPath);

                // Succcessful? and is there even any Components to update?
                if (payload.IsSuccessStatusCode && req.CurrencyPair?.CurrencyPairComponents != null && req.CurrencyPair.CurrencyPairComponents.Count > 0)
                {
                    // Pull the content
                    var content = await payload.Content.ReadAsStringAsync();
                    // Parse the content
                    var contentToken = JToken.Parse(content);

                    // Pull the components wanted
                    var requestComponents = req.CurrencyPair.CurrencyPairComponents
                        .Where(cpc => cpc.DeletedAt == null && cpc.IsEnabled);

                    if (contentToken is JArray)
                    {
                        // Pump in the array
                        List<string> dataList = contentToken.ToObject<List<string>>();

                        // If the db really hodls a number,
                        foreach (var component in requestComponents)
                        {
                            if (component.QueryComponent != null &&
                                int.TryParse(component.QueryComponent, out int index))
                            {
                                // let's work it out
                                if (index >= 0 && index < dataList.Count)
                                {
                                    // Number checks
                                    // Make sure the datalist element we're targetting contains a proper value.
                                    if (decimal.TryParse(dataList[index], out decimal val))
                                    {
                                        // Update it
                                        _currencyPairComponentService.UpdatePairValue(component.Id, val);
                                    }
                                }
                            }
                        }

                        return true;
                    } else if (contentToken is JObject)
                    {
                        // Pump in the object
                        JObject obj = contentToken.ToObject<JObject>();

                        foreach (var component in requestComponents)
                        {
                            if (component.QueryComponent != null)
                            {
                                var rawData = (string)obj.SelectToken(component.QueryComponent);

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

                        return true;
                    }
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
    }
}