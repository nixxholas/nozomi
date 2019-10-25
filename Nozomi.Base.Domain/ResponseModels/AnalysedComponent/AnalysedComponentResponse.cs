using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Analytical;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.ResponseModels.AnalysedComponent
{
    public class AnalysedComponentResponse
    {
        public AnalysedComponentType ComponentType { get; set; }
        
        public ICollection<string> Historical { get; set; }
        
        public string Value { get; set; }
    }
}