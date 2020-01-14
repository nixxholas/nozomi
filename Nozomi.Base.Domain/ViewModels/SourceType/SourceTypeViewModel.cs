using System;
using System.Collections.Generic;
using Nozomi.Data.ResponseModels.Source;

namespace Nozomi.Data.ViewModels.SourceType
{
    public class SourceTypeViewModel
    {
        public Guid Guid { get; set; }
        
        public string Name { get; set; }
        
        public string Abbreviation { get; set; }
        
        public IEnumerable<SourceViewModel> Sources { get; set; }
    }
}