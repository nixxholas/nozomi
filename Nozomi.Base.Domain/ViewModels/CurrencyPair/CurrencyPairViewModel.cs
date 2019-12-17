using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.AnalysedComponent;
using Nozomi.Data.ViewModels.Source;

namespace Nozomi.Data.ViewModels.CurrencyPair
{
    public class CurrencyPairViewModel
    {
        public Guid Guid { get; set; }
        
        public CurrencyPairType Type { get; set; }
        
        public string MainTicker { get; set; }
        
        public string CounterTicker { get; set; }
        
        public string SourceGuid { get; set; }
        
        public SourceViewModel Source { get; set; }
        
        public IEnumerable<AnalysedComponentViewModel> AnalysedComponents { get; set; }
    }
}