using System.Collections.Generic;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.AnalysedComponent;

namespace Nozomi.Data.ViewModels.CurrencyPair
{
    public class CurrencyPairViewModel
    {
        public CurrencyPairType Type { get; set; }
        
        public string MainTicker { get; set; }
        
        public string CounterTicker { get; set; }
        
        public string SourceGuid { get; set; }
        
        public ICollection<AnalysedComponentViewModel> AnalysedComponents { get; set; }
    }
}