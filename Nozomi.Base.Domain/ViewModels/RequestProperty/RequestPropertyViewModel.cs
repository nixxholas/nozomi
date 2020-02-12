using System;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.ViewModels.RequestProperty
{
    public class RequestPropertyViewModel : CreateRequestPropertyInputModel
    {
        public RequestPropertyViewModel(Guid guid, RequestPropertyType type, string key, string value)
        {
            Guid = guid.ToString();
            Type = type;
            Key = key;
            Value = value;
        }
        
        public string Guid { get; set; }
    }
}