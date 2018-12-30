using System;

namespace Nozomi.Base.Core
{
    public abstract class BaseEntityModel
    {
        public bool IsEnabled { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long CreatedBy { get; set; }

        public long ModifiedBy { get; set; }

        public long DeletedBy { get; set; }
    }
}