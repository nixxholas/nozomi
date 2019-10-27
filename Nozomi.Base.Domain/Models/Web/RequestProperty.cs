using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Web
{
    public class RequestProperty : Entity
    {
        public long Id { get; set; }

        public RequestPropertyType RequestPropertyType { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public long RequestId { get; set; }
        public Request Request { get; set; }

        protected bool Equals(RequestProperty other)
        {
            return RequestPropertyType == other.RequestPropertyType
                   && string.Equals(Key, other.Key) && string.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) RequestPropertyType;
                hashCode = (hashCode * 397) ^ (Key != null ? Key.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ RequestId.GetHashCode();
                hashCode = (hashCode * 397) ^ (Request != null ? Request.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
