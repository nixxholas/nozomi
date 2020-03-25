using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
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
                switch (dispatchInputModel.Type)
                {
                    case RequestType.HttpGet:
                    case RequestType.HttpPost:
                        var httpClient = new HttpClient();
                        
                        // FLUSH
                        httpClient.DefaultRequestHeaders.Clear();

                        // Setup the url
                        var uri = new UriBuilder(dispatchInputModel.Endpoint);
                        var urlParams = HttpUtility.ParseQueryString(string.Empty);

                        // Setup the request properties
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
                                    var cchv = JsonConvert.DeserializeObject<CacheControlHeaderValue>(reqProp.Value);

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
                        var payload = await httpClient.GetAsync(uri.ToString());

                        switch (payload.StatusCode)
                        {
                            case HttpStatusCode.OK:
                                // Return the payload distinctively
                                return new DispatchViewModel
                                {
                                    Payload = Utf8Json.JsonSerializer.ToJsonString(payload.Content),
                                    Response = payload
                                };
                            case HttpStatusCode.TooManyRequests:
                                _logger.LogWarning($"{_eventName} Dispatch: " +
                                                   $"{dispatchInputModel.Endpoint} Too many requests");

                                throw new DataException("Too many requests.");
                        }
                        
                        break;
                    case RequestType.WebSocket:
                        break;
                    default:
                        throw new InvalidOperationException("Invalid protocol type.");
                }
            }

            throw new NullReferenceException("Invalid payload.");
        }
    }
}