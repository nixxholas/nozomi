using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Data.ViewModels.RequestProperty;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Data.ViewModels.Request
{
    public class RequestViewModel : CreateRequestViewModel
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
        
        public Guid Guid { get; set; }
        
        public bool IsEnabled { get; set; }
        
        public ICollection<ComponentViewModel> Components { get; set; }
        
        public ICollection<RequestPropertyViewModel> Properties { get; set; }

        public class RequestViewModelExample : IExamplesProvider<RequestViewModel>
        {
            public RequestViewModel GetExamples()
            {
                return new RequestViewModel
                {
                    Guid = Guid.NewGuid(),
                    IsEnabled = true,
                    RequestType = RequestType.HttpGet,
                    ResponseType = ResponseType.Json,
                    DataPath = "",
                    Delay = 5000,
                    FailureDelay = 600000,
                    Components = new List<ComponentViewModel>(),
                    Properties = new List<RequestPropertyViewModel>(),
                    CurrencySlug = "BTC",
                    CurrencyPairGuid = Guid.NewGuid().ToString(),
                    CurrencyTypeGuid = Guid.NewGuid().ToString()
                };
            }
        }
    }
}