using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Data.ViewModels.RequestProperty;

namespace Nozomi.Data.ViewModels.Request
{
    public class RequestViewModel : CreateRequestViewModel
    {
        public RequestViewModel() {}
        
        public RequestViewModel(Guid guid, RequestType requestType, ResponseType responseType, string dataPath,
            int delay, long failureDelay, bool isEnabled, string currencySlug, long? currencyPairId, long? currencyTypeId)
        {
            Guid = guid;
            RequestType = requestType;
            ResponseType = responseType;
            DataPath = dataPath;
            Delay = delay;
            FailureDelay = failureDelay;
            IsEnabled = isEnabled;
            CurrencySlug = currencySlug;
            
            if (currencyPairId != null)
                CurrencyPairId = (long)currencyPairId;
            
            if (currencyTypeId != null)
                CurrencyTypeId = (long)currencyTypeId;
        }
        
        public RequestViewModel(Guid guid, RequestType requestType, ResponseType responseType, string dataPath,
            int delay, long failureDelay, bool isEnabled, string currencySlug, long? currencyPairId, long? currencyTypeId,
            ICollection<ComponentViewModel> components, ICollection<RequestPropertyViewModel> properties)
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

            if (currencyPairId != null)
                CurrencyPairId = (long)currencyPairId;
            
            if (currencyTypeId != null)
                CurrencyTypeId = (long)currencyTypeId;
        }
        
        public Guid Guid { get; set; }
        
        public bool IsEnabled { get; set; }
        
        public long CurrencyPairId { get; set; }
        
        public long CurrencyTypeId { get; set; }
        
        public ICollection<ComponentViewModel> Components { get; set; }
        
        public ICollection<RequestPropertyViewModel> Properties { get; set; }
    }
}