using System;
using Nozomi.Data.AreaModels.v1.AnalysedComponent;

namespace Nozomi.Base.Admin.Domain.AreaModels.AnalysedComponents
{
    public class AnalysedComponentDto : AnalysedComponentDTO
    {
        public bool IsEnabled { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public long CreatedBy { get; set; }
        
        public DateTime ModifiedAt { get; set; }
        
        public long ModifiedBy { get; set; }
        
        public DateTime? DeletedAt { get; set; }
        
        public long DeletedBy { get; set; }
    }
}