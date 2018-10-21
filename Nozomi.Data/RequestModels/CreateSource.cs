namespace Nozomi.Data.RequestModels
{
    public class CreateSource
    {
        public string Name { get; set; }
        
        public string Abbreviation { get; set; }
        
        public string ApiDocsUrl { get; set; }

        public bool IsValid()
        {
            return string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Abbreviation) && string.IsNullOrEmpty(ApiDocsUrl);
        }
    }
}