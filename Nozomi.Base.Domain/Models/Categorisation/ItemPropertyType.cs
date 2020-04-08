using System.Collections.Generic;
using System.ComponentModel;

namespace Nozomi.Data.Models.Categorisation
{
    public class ItemPropertyType
    {
        public long Id { get; set; }
        
        public string Name { get; set; }
        
        public ICollection<ItemProperty> ItemProperties { get; set; }
        
        // Generic = 0,
        // [Description("Website")]
        // Website = 1,
        // [Description("Twitter")]
        // Twitter = 10,
        // [Description("Medium")]
        // Medium = 11
    }
}