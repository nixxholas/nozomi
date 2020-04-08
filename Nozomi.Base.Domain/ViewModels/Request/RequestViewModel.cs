using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Data.ViewModels.RequestProperty;
using Nozomi.Data.ViewModels.WebsocketCommand;

namespace Nozomi.Data.ViewModels.Request
{
    public class RequestViewModel
    {
        public RequestViewModel() {}
        
        public RequestViewModel(Guid guid, RequestType requestType, ResponseType responseType, string dataPath,
            int delay, long failureDelay, bool isEnabled, string currencySlug, string currencyPairGuid, 
            string currencyTypeGuid)
        {
            Guid = guid;
            RequestType = requestType;
            ResponseType = responseType;
            DataPath = dataPath;
            Delay = delay;
            FailureDelay = failureDelay;
            IsEnabled = isEnabled;
            CurrencySlug = currencySlug;
            CurrencyPairGuid = currencyPairGuid;
            CurrencyTypeGuid = currencyTypeGuid;
        }
        
        public RequestViewModel(Guid guid, RequestType requestType, ResponseType responseType, string dataPath,
            int delay, long failureDelay, bool isEnabled, string currencySlug, string currencyPairGuid, 
            string currencyTypeGuid, ICollection<ComponentViewModel> components, 
            ICollection<RequestPropertyViewModel> properties)
        {
            Guid = guid;
            RequestType = requestType;
            ResponseType = responseType;
            DataPath = dataPath;
            Delay = delay;
            FailureDelay = failureDelay;
            IsEnabled = isEnabled;
            Components = components;
            Properties = properties;
            CurrencySlug = currencySlug;
            CurrencyPairGuid = currencyPairGuid;
            CurrencyTypeGuid = currencyTypeGuid;
        }
        
        /// <summary>
        /// The unique identifier of the request.
        /// </summary>
        public Guid Guid { get; set; }
        
        /// <summary>
        /// The protocol type of this request.
        /// </summary>
        public RequestType RequestType { get; set; }

        /// <summary>
        /// The response type (or the payload type) of this request.
        /// </summary>
        public ResponseType ResponseType { get; set; }

        /// <summary>
        /// The URL to the endpoint
        /// </summary>
        public string DataPath { get; set; }

        /// <summary>
        /// The delay between each request, in milliseconds.
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// The delay after a failed request attempt, in milliseconds.
        /// </summary>
        public long FailureDelay { get; set; }

        /// <summary>
        /// This will deduce what type of request this is for
        /// i.e. ItemType, CurrencyPair or Currency.
        /// </summary>
        public CreateRequestInputModel.RequestParentType ParentType { get; set; }

        /// <summary>
        /// The unique slug identifier of the currency linked to this request.
        /// </summary>
        public string CurrencySlug { get; set; }

        /// <summary>
        /// The unique GUID identifier of the currency pair linked to this request.
        /// </summary>
        public string CurrencyPairGuid { get; set; }

        /// <summary>
        /// The unique GUID identifier of the Currency Type linked to this request.
        /// </summary>
        public string CurrencyTypeGuid { get; set; }
        
        /// <summary>
        /// Enabled?
        /// </summary>
        public bool IsEnabled { get; set; }
        
        /// <summary>
        /// The components that are linked to this request.
        /// </summary>
        public ICollection<ComponentViewModel> Components { get; set; }
        
        /// <summary>
        /// The request properties that are linked to this request.
        /// </summary>
        public ICollection<RequestPropertyViewModel> Properties { get; set; }
        
        public ICollection<WebsocketCommandViewModel> WebsocketCommands { get; set; }
    }
}