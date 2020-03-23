using System.Collections;
using System.Collections.Generic;
using Nozomi.Data.ViewModels.Component;

namespace Nozomi.Data.ViewModels.ComponentType
{
    public class ComponentTypeViewModel
    {
        public long Id { get; set; }
        
        public string Slug { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public ICollection<ComponentViewModel> Components { get; set; }
    }
}