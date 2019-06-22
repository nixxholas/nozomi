using System.ComponentModel;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Base.Admin.Domain.AreaModels.AnalysedComponent
{
    public class UpdateAnalysedComponent
    {
        public long Id { get; set; }
        
        [DefaultValue(AnalysedComponentType.Unknown)]
        public AnalysedComponentType ComponentType { get; set; }

        [DefaultValue(false)]
        public bool IsDenominated { get; set; }

        [DefaultValue(10000)]
        public int Delay { get; set; }
        
        [DefaultValue(false)]
        public bool IsEnabled { get; set; }
        
        public string UIFormatting { get; set; }
    }
}