using System;
using System.Collections.Generic;
using Nozomi.Base.Admin.Domain.AreaModels.AnalysedHistoricItem;

namespace Nozomi.Base.Admin.Domain.AreaModels.AnalysedComponent
{
    public class AnalysedComponentDto : Data.AreaModels.v1.AnalysedComponent.AnalysedComponentDto
    {
        public bool IsEnabled { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public long CreatedBy { get; set; }
        
        public DateTime ModifiedAt { get; set; }
        
        public long ModifiedBy { get; set; }
        
        public DateTime? DeletedAt { get; set; }
        
        public long DeletedBy { get; set; }
        
        public ICollection<AnalysedHistoricItemDto> AnalysedHistoricItems { get; set; }
    }
}