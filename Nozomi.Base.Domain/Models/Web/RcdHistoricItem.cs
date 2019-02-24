using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Web
{
    public class RcdHistoricItem : BaseEntityModel
    {
        public long Id { get; set; }

        public string Value { get; set; }
        
        public long RequestComponentDatumId { get; set; }
        public RequestComponentDatum RequestComponentDatum { get; set; }
    }
}