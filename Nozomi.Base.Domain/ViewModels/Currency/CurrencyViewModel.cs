using System;
using System.Collections.Generic;
using Nozomi.Data.ViewModels.AnalysedComponent;

namespace Nozomi.Data.ViewModels.Currency
{
    public class CurrencyViewModel
    {
        public Guid CurrencyTypeGuid { get; set; }
        
        public string Abbreviation { get; set; }
        
        public string Slug { get; set; }
        
        public string Name { get;set; }
        
        public string LogoPath { get; set; }
        
        public string Description { get; set; }

        public int Denominations { get; set; } = 0;
        
        public string DenominationName { get; set; }
        
        public IEnumerable<AnalysedComponentViewModel> Components { get; set; }
    }
}