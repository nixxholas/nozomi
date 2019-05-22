using System;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Data.ResponseModels.AnalysedComponent
{
    public class AnalysedComponentResponse
    {
        public AnalysedComponentType ComponentType { get; set; }
        
        public string Value { get; set; }
    }
}