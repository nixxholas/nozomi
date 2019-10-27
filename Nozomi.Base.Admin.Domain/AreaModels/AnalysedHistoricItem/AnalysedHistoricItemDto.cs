using System;

namespace Nozomi.Base.Admin.Domain.AreaModels.AnalysedHistoricItem
{
    public class AnalysedHistoricItemDto
    {
        public long Id { get; set; }
        
        public long AnalysedComponentId { get; set; }
        
        public string Value { get; set; }
        
        public DateTime HistoricDateTime { get; set; }
        
        public bool IsEnabled { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public string DeletedBy { get; set; }
    }
}