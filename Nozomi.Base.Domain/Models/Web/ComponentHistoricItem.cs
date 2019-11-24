using System;
using System.Runtime.Serialization;
using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Web
{
    [DataContract]
    public class ComponentHistoricItem : Entity
    {
        public long Id { get; set; }
        
        [DataMember]
        public DateTime HistoricDateTime { get; set; }

        [DataMember]
        public string Value { get; set; }
        
        public long RequestComponentId { get; set; }
        public Component Component { get; set; }
    }
}