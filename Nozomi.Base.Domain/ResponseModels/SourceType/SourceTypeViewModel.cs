using System;
using System.Collections.Generic;
using Nozomi.Data.ResponseModels.Source;

namespace Nozomi.Data.ResponseModels.SourceType
{
    public class SourceTypeViewModel
    {
        public Guid Guid { get; set; }
        
        public string Name { get; set; }
        
        public string Abbreviation { get; set; }
        
        public ICollection<SourceViewModel> Sources { get; set; }
    }
}