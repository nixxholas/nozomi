using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Data.ViewModels.AnalysedComponent
{
    public class AnalysedComponentViewModel
    {
        public AnalysedComponentType Type { get; set; }
        
        public string UiFormatting { get; set; }
        
        public string Value { get; set; }
        
        public bool IsDenominated { get; set; }
    }
}