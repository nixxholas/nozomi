using System;
using System.Collections.Generic;
using Nozomi.Data.ViewModels.ComponentHistoricItem;

namespace Nozomi.Data.ViewModels.Component
{
    public class UpdateComponentInputModel
    {
        public long ComponentTypeId { get; set; }

        public string Identifier { get; set; }
        
        public string QueryComponent { get; set; }
        
        public bool IsDenominated { get; set; }
        
        public bool AnomalyIgnorance { get; set; }
        
        public bool StoreHistoricals { get; set; }
        
        public IEnumerable<ComponentHistoricItemViewModel> History { get; set; }
    }
}