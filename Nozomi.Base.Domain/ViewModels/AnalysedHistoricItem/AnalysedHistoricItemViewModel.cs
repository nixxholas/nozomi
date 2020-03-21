using System;
using System.ComponentModel;

namespace Nozomi.Data.ViewModels.AnalysedHistoricItem
{
    public class AnalysedHistoricItemViewModel
    {
        /// <summary>
        /// The timestamp this value was generated.
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// The value of the analysed component at the said timestamp.
        /// </summary>
        public string Value { get; set; }
    }
}