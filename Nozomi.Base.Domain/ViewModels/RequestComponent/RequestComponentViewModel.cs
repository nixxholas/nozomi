using System;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.ViewModels.RequestComponent
{
    public class RequestComponentViewModel
    {
        public Guid Guid { get; set; }
        
        public ComponentType Type { get; set; }
        
        public string Value { get; set; }
        
        public bool IsDenominated { get; set; }
    }
}