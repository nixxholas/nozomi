using System;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.ViewModels.RequestProperty
{
    public class RequestPropertyViewModel : CreateRequestPropertyInputModel
    {
        public RequestPropertyViewModel() {}
        
        public RequestPropertyViewModel(Guid guid, RequestPropertyType type, string key, string value)
        {
            Guid = guid;
            Type = type;
            Key = key;
            Value = value;
        }
        
        /// <summary>
        /// The unique identifier of the request property.
        /// </summary>
        public Guid Guid { get; set; }
    }
}