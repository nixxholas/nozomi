using Nozomi.Data.WebModels;

namespace Nozomi.Data.AreaModels.v1.RequestProperty
{
    public class CreateRequestProperty
    {
        public RequestPropertyType RequestPropertyType { get; set; }
        
        public string Key { get; set; }
        
        public string Value { get; set; }
    }
}