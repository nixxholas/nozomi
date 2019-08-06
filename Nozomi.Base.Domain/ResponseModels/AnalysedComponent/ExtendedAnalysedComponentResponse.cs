using System.Collections.Generic;

namespace Nozomi.Data.ResponseModels.AnalysedComponent
{
    public class ExtendedAnalysedComponentResponse<T> : AnalysedComponentResponse
    {
        public string ParentName { get; set; }
        
        public new string ComponentType { get; set; }
        
        public new ICollection<T> Historical { get; set; }
    }
}