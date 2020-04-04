using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Data.ViewModels.RequestProperty;
using Nozomi.Data.ViewModels.WebsocketCommand;

namespace Nozomi.Data.ViewModels.Request
{
    public class RequestInputModel : CreateRequestInputModel
    {
        public RequestInputModel() {}
        
        public RequestInputModel(Guid guid, RequestType requestType, ResponseType responseType, string dataPath,
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
        
        public RequestInputModel(Guid guid, RequestType requestType, ResponseType responseType, string dataPath,
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
        
        public new ICollection<UpdateWebsocketCommandInputModel> WebsocketCommands { get; set; }
    }
}