using System;
using System.Runtime.Serialization;

namespace Nozomi.Data.Models.Category
{
    public class ItemProperty
    {
        public Guid Guid { get; set; }
        
        [DataMember]
        public string Value { get; set; }
        
        public long CurrencyId { get; set; }
        
        public Item Item { get; set; }
    }
}