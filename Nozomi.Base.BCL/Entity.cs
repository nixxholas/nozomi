using System;
using Nozomi.Base.Auth.Models;

namespace Nozomi.Base.BCL
{
    public abstract class Entity
    {
        public bool IsEnabled { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
        
        public string CreatedById { get; set; }
        
        public string ModifiedById { get; set; }
        
        public string DeletedById { get; set; }
    }
}