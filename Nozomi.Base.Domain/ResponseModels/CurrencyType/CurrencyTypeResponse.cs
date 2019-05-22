using System.Collections.Generic;
using Nozomi.Data.ResponseModels.AnalysedComponent;

namespace Nozomi.Data.ResponseModels.CurrencyType
{
    public class CurrencyTypeResponse
    {
        public string Name { get; set; }
        
        public ICollection<AnalysedComponentResponse> Components { get; set; }
    }
}