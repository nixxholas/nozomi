using Nozomi.Data.Models.Web;

namespace Nozomi.Data.ViewModels.RequestProperty
{
    public class RequestPropertyViewModel
    {
        public RequestPropertyViewModel() {}

        public RequestPropertyViewModel(RequestPropertyType type, string key, string value)
        {
            Type = type;
            Key = key;
            Value = value;
        }
        
        public RequestPropertyType Type { get; set; }
        
        public string Key { get; set; }
        
        public string Value { get; set; }
    }
}