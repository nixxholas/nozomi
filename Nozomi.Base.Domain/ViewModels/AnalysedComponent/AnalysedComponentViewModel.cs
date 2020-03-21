using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ViewModels.AnalysedHistoricItem;

namespace Nozomi.Data.ViewModels.AnalysedComponent
{
    public class AnalysedComponentViewModel : CreateAnalysedComponentViewModel
    {
        /// <summary>
        /// The unique identifier of the analysed component.
        /// </summary>
        public Guid Guid { get; set; }
        
        /// <summary>
        /// The current value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Self explanatory, if its enabled then its a true of course
        /// </summary>
        public bool? IsEnabled { get; set; }

        /// <summary>
        /// The collection of historical values for this entity.
        /// </summary>
        public IEnumerable<AnalysedHistoricItemViewModel> History { get; set; }
    }
}