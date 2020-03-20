using System;
using System.ComponentModel;

namespace Nozomi.Data.ViewModels.AnalysedHistoricItem
{
    public class AnalysedHistoricItemViewModel
    {
        [Description("The time this value was generated.")]
        public DateTime Timestamp { get; set; }
        
        [Description("The value of the analysed component at the said timestamp.")]
        public string Value { get; set; }
    }
}