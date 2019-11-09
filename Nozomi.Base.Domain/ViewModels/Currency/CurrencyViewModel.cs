using System;
using System.Collections.Generic;
using Nozomi.Data.ViewModels.AnalysedComponent;
using Nozomi.Data.ViewModels.Component;

namespace Nozomi.Data.ViewModels.Currency
{
    public class CurrencyViewModel : CreateCurrencyViewModel
    {        
        public IEnumerable<AnalysedComponentViewModel> Components { get; set; }
        
        public IEnumerable<ComponentViewModel> RawComponents { get; set; }
    }
}