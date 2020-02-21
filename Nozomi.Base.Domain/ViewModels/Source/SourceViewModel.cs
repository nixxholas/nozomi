using System;

namespace Nozomi.Data.ViewModels.Source
{
    public class SourceViewModel
    {
        public Guid Guid { get; set; }
        
        public string Abbreviation { get; set; }
        
        public string Name { get; set; }
        
        public string ApiDocsUrl { get; set; }
        
        public Guid SourceTypeGuid { get; set; }
    }
}