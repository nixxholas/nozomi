using System.Collections.Generic;
using Nozomi.Base.BCL;

namespace Nozomi.Data.Models.Web
{
    public class ComponentType : Entity
    {
        public long Id { get; set; }
        
        public string Slug { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public ICollection<Component> Components { get; set; }
    }
}