using System.ComponentModel;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Base.Admin.Domain.AreaModels.AnalysedComponent
{
    public class CreateAnalysedComponent
    {
        [DefaultValue(AnalysedComponentType.Unknown)]
        public AnalysedComponentType ComponentType { get; set; }

        [DefaultValue(false)]
        public bool IsDenominated { get; set; }

        public int Delay { get; set; }
        
        public string UIFormatting { get; set; }
        
        public long CurrencyId { get; set; }
        
        public long CurrencyPairId { get; set; }
        
        public long CurrencyTypeId { get; set; }
    }
}