using System;

namespace Nozomi.Data.ViewModels.Source
{
    public class SourceViewModel
    {
        public Guid Guid { get; set; }
        
        public string Abbreviation { get; set; }
        
        public string Name { get; set; }
        
        public string ApiDocsUrl { get; set; }
        
        public string SourceTypeGuid { get; set; }
    }
}