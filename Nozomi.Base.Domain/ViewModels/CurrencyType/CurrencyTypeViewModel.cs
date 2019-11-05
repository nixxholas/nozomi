using System;

namespace Nozomi.Data.ViewModels.CurrencyType
{
    public class CurrencyTypeViewModel
    {
        public Guid Guid { get; set; }
        
        public string TypeShortForm { get; set; }
        
        public string Name { get; set; }
    }
}