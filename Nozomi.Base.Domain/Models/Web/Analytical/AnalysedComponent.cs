using System.Collections.Generic;
using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Web.Analytical
{
    /// <summary>
    /// Component made only in runtime.
    /// </summary>
    public class AnalysedComponent : BaseEntityModel
    {
        public long Id { get; set; }
        
        public AnalysedComponentType ComponentType { get; set; }
        
        public string Value { get; set; }

        public bool IsDenominated { get; set; } = false;
        
        public int Delay { get; set; }

        /// <summary>
        /// Defines the formatting for frontend libs.
        /// 
        /// i.e. for Numeral.js - '(0,0.0000)' to display (10,000.0000)
        /// 
        /// We shouldn't peg this to an AnalysedComponentType because there
        /// may be different variation on the same type
        /// </summary>
        /// <value>The UIF ormatting.</value>
        public string UIFormatting { get; set; }
        
        public long? RequestId { get; set; }
        
        public Request Request { get; set; }
        
        public long? CurrencyId { get; set; }
        
        public Currency.Currency Currency { get; set; }
        
        public ICollection<AnalysedHistoricItem> AnalysedHistoricItems { get; set; }
    }
}