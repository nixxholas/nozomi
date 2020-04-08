using System;
using System.Runtime.Serialization;
using Nozomi.Base.BCL;

namespace Nozomi.Data.Models.Categorisation
{
    [DataContract]
    public class ItemProperty : Entity
    {
        public long Id { get; set; }
        
        public Guid Guid { get; set; }
        
        public long TypeId { get; set; }
        
        public ItemPropertyType Type { get; set; }
        
        [DataMember]
        public string Value { get; set; }
        
        public long ItemId { get; set; }
        
        public Item Item { get; set; }
    }
}