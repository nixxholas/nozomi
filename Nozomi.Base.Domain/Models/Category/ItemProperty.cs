using System;
using System.Runtime.Serialization;
using Nozomi.Base.BCL;

namespace Nozomi.Data.Models.Category
{
    public class ItemProperty : Entity
    {
        public Guid Guid { get; set; }
        
        public string Name { get; set; }
        
        [DataMember]
        public string Value { get; set; }
        
        public Guid ItemGuid { get; set; }
        
        public Item Item { get; set; }
    }
}