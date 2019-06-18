using System;
using Nozomi.Data.AreaModels.v1.AnalysedComponent;

namespace Nozomi.Base.Admin.Domain.AreaModels.AnalysedComponent
{
    public class AnalysedComponentDto : AnalysedComponentDTO
    {
        public bool IsEnabled { get; set; }
        
        public long? DeletedBy { get; set; }
        
        public DateTime? DeletedAt { get; set; }
    }
}