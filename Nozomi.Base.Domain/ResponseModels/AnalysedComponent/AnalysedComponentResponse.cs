using System;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.ResponseModels.AnalysedComponent
{
    public class AnalysedComponentResponse
    {
        public ComponentType ComponentType { get; set; }
        
        public string Value { get; set; }
    }
}