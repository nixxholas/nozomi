using System;
using System.Runtime.Serialization;
using Nozomi.Base.BCL;

namespace Nozomi.Data.Models.Categorisation
{
    [DataContract]
    public class CurrencyProperty : Entity
    {
        public long Id { get; set; }
        
        public Guid Guid { get; set; }
        
        [DataMember]
        public CurrencyPropertyType Type { get; set; }
        
        [DataMember]
        public string Value { get; set; }
        
        public long CurrencyId { get; set; }
        
        public Item Item { get; set; }

        public bool IsValid()
        {
            return Type >= 0 && !string.IsNullOrEmpty(Value) && CurrencyId > 0;
        }
    }
}