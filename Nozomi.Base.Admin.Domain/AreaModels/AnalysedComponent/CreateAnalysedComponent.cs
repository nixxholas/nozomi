using System.ComponentModel;
using Nozomi.Data.Models.Analytical;

namespace Nozomi.Base.Admin.Domain.AreaModels.AnalysedComponent
{
    public class CreateAnalysedComponent
    {
        [DefaultValue(AnalysedComponentType.Unknown)]
        public AnalysedComponentType ComponentType { get; set; }

        [DefaultValue(false)]
        public bool IsDenominated { get; set; }

        public int Delay { get; set; }
        
        public string UiFormatting { get; set; }
        
        public long CurrencyId { get; set; }
        
        public long CurrencyPairId { get; set; }
        
        public long CurrencyTypeId { get; set; }
    }
}