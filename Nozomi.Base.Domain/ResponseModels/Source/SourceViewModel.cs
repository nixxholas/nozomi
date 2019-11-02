namespace Nozomi.Data.ResponseModels.Source
{
    public class SourceViewModel
    {
        public long Id { get; set; }
        
        public string Abbreviation { get; set; }
        
        public string Name { get; set; }
        
        public string ApiDocsUrl { get; set; }
        
        public long SourceTypeId { get; set; }
    }
}