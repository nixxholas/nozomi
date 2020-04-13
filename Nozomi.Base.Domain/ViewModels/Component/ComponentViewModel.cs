using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.ComponentHistoricItem;

namespace Nozomi.Data.ViewModels.Component
{
    public class ComponentViewModel
    {
        public Guid Guid { get; set; }
        
        /// <summary>
        /// ComponentTypeId
        /// </summary>
        public long Type { get; set; }
        
        public string Value { get; set; }
        
        public string Identifier { get; set; }
        
        public string Query { get; set; }
        
        public bool IsDenominated { get; set; }
        
        public IEnumerable<ComponentHistoricItemViewModel> History { get; set; }
    }
}