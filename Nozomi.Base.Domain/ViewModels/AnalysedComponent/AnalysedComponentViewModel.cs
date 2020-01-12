using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ViewModels.AnalysedHistoricItem;

namespace Nozomi.Data.ViewModels.AnalysedComponent
{
    public class AnalysedComponentViewModel : CreateAnalysedComponentViewModel
    {
        public Guid Guid { get; set; }
        
        public string Value { get; set; }
        
        public new string CurrencyPairId { get; set; }
        
        public new string CurrencyTypeId { get; set; }
        
        public IEnumerable<AnalysedHistoricItemViewModel> History { get; set; }
    }
}