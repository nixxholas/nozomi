using System;

namespace Nozomi.Data.ViewModels.ComponentHistoricItem
{
    public class ComponentHistoricItemViewModel
    {
        /// <summary>
        /// The timestamp of this historical value.
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// The value of this historical item.
        /// </summary>
        public string Value { get; set; }
    }
}