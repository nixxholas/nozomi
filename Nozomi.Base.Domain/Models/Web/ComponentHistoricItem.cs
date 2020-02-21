using System;
using System.Runtime.Serialization;
using Nozomi.Base.BCL;

namespace Nozomi.Data.Models.Web
{
    [DataContract]
    public class ComponentHistoricItem : Entity
    {        
        public Guid Guid { get; set; }
        
        [DataMember]
        public DateTime HistoricDateTime { get; set; }

        [DataMember]
        public string Value { get; set; }
        
        public long RequestComponentId { get; set; }
        public Component Component { get; set; }
    }
}