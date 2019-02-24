using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Web.Analytical
{
    public class AnalysedHistoricItem : BaseEntityModel
    {
        public long Id { get; set; }
        
        public long AnalysedComponentId { get; set; }
        
        public AnalysedComponent AnalysedComponent { get; set; }
        
        public string Value { get; set; }
    }
}