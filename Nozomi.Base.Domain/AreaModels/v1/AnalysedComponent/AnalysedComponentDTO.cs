using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Data.AreaModels.v1.AnalysedComponent
{
    public class AnalysedComponentDto
    {
        public long Id { get; set; }
        
        public AnalysedComponentType ComponentType { get; set; }
        
        public string Value { get; set; }

        public bool IsDenominated { get; set; } = false;
        
        public int Delay { get; set; }
    }
}