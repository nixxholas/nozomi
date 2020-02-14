using System;
using System.Runtime.Serialization;
using Nozomi.Base.BCL;

namespace Nozomi.Data.Models.Web
{
    [DataContract]
    public class RequestProperty : Entity
    {
        public RequestProperty() {}

        /// <summary>
        /// Manual-based construction
        /// </summary>
        /// <param name="requestPropertyType"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public RequestProperty(Guid guid, RequestPropertyType requestPropertyType, string key, string value)
        {
            Guid = guid;
            RequestPropertyType = requestPropertyType;
            Key = key;
            Value = value;
        }
        
        public long Id { get; set; }
        
        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public RequestPropertyType RequestPropertyType { get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
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
