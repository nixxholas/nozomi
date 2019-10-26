using Nozomi.Data.Models.Web;

namespace Nozomi.Data.AreaModels.v1.RequestProperty
{
    public class RequestPropertyDTO
    {
        public long Id { get; set; }

        public RequestPropertyType RequestPropertyType { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}