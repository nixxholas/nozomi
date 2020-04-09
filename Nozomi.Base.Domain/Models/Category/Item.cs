using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Nozomi.Base.BCL;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Data.Models.Category
{
    public class Item : Entity
    {
        public Guid Guid { get; set; }

        public Guid ItemTypeGuid { get; set; }
        public ItemType ItemType { get; set; }
        
        public string LogoPath { get; set; }

        [DataMember]
        public string Abbreviation { get; set; } // USD? MYR? IND?
        
        [DataMember]
        public string Slug { get; set; }

        [DataMember]
        public string Name { get; set; }
        
        public string Description { get; set; }

        [DataMember]
        public int Denominations { get; set; } = 0;
        
        [DataMember]
        public string DenominationName { get; set; }
        
        public ICollection<ItemProperty> ItemProperties { get; set; }
        
        public ICollection<ItemSource> ItemSources { get; set; }
        
        public ICollection<Request> Requests { get; set; }
    }
}