using System;

namespace Nozomi.Base.Core
{
    public abstract class Entity
    {
        public bool IsEnabled { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid ModifiedBy { get; set; }

        public Guid DeletedBy { get; set; }
    }
}