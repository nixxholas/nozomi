using System;

namespace Nozomi.Base.Core.Models
{
    public abstract class Entity
    {
        public Guid Guid { get; protected set; }
        
        public bool IsEnabled { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long CreatedBy { get; set; }

        public long ModifiedBy { get; set; }

        public long DeletedBy { get; set; }
        
        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Guid.Equals(compareTo.Guid);
        }
        
        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Guid.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Guid + "]";
        }
    }
}