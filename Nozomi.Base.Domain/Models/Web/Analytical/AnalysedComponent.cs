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
        
        public int Delay { get; set; }
        
        public long? RequestId { get; set; }
        
        public Request Request { get; set; }
        
        public long? CurrencyId { get; set; }
        
        public Currency.Currency Currency { get; set; }
        
        public ICollection<AnalysedHistoricItem> AnalysedHistoricItems { get; set; }
    }
}