using System.Collections.Generic;

namespace Nozomi.Data.ResponseModels.AnalysedComponent
{
    public class ExtendedAnalysedComponentResponse<T> : AnalysedComponentResponse
    {
        public new string ComponentType { get; set; }
        
        public new ICollection<DateValuePair<T>> Historical { get; set; }
    }
}