using System;

namespace Nozomi.Data.ResponseModels.Source
{
    [Obsolete]
    public class LegacySourceViewModel
    {
        public long Id { get; set; }
        
        public string Abbreviation { get; set; }
        
        public string Name { get; set; }
        
        public string ApiDocsUrl { get; set; }
        
        public long SourceTypeId { get; set; }
    }
}