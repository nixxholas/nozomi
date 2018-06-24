using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nozomi.Data.WebModels;
using Nozomi.Data.WebModels.LoggingModels;
using Nozomi.Service.HostedServices.RequestTypes.Interfaces;
using Nozomi.Service.Services.Requests;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Service.HostedServices.RequestTypes
{
    public class HttpGetSyncingService : BaseHostedService, IHttpGetSyncingService
    {
        private HttpClient _httpClient = new HttpClient();
        private readonly IRequestService _requestService;
        private readonly IRequestLogService _requestLogService;
        private readonly ILogger<HttpGetSyncingService> _logger;
        private List<Request> _requestList;
        
        public HttpGetSyncingService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _requestService = _scope.ServiceProvider.GetRequiredService<RequestService>();
            _requestLogService = _scope.ServiceProvider.GetRequiredService<RequestLogService>();
            
            _logger = _scope.ServiceProvider.GetRequiredService<ILogger<HttpGetSyncingService>>();

            // Initialize the request list for all GET requests
            _requestList = _requestService.GetAllActive(true)
                               .Where(r => r.RequestType.Equals(RequestType.HttpGet))
                               .ToList() ?? new List<Request>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        { 
            _logger.LogInformation("HttpGetSyncingService is starting.");

            stoppingToken.Register(() => _logger.LogInformation("HttpGetSyncingService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                // We will need to resync the Request collection to make sure we're polling only the ones we want to poll
                var getBasedRequests = _requestService.GetAll(r => r.IsEnabled && r.DeletedAt == null
                                                                               && r.RequestType.Equals(RequestType
                                                                                   .HttpGet), true);

                // Iterate the requests
                // NOTE: Let's not call a parallel loop since HttpClients might tend to result in memory leaks.
                foreach (var rq in getBasedRequests)
                {
                    // Process the request
                    if (await Process(rq))
                    {
                        // Since its successful
                    }
                }
            }

            _logger.LogWarning("HttpGetSyncingService background task is stopping.");
        }

        public async Task<bool> Process(Request req)
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

                // Succcessful?
                if (payload.IsSuccessStatusCode)
                {
                    var content = await payload.Content.ReadAsStringAsync();

                    var contentToken = JToken.Parse(content);

                    if (contentToken is JArray)
                    {
                        
                    }
                    
                    // Populate the request components
                    foreach (var reqComp in req.RequestComponents)
                    {
                        
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