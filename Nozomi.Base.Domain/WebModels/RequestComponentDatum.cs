namespace Nozomi.Data.WebModels
{
    public class RequestComponentDatum : BaseEntityModel
    {
        public long Id { get; set; }
        
        public long RequestComponentId { get; set; }
        public RequestComponent RequestComponent { get; set; }

        public string Value { get; set; }
    }
}