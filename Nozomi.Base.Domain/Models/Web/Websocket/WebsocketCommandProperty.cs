using System;
using System.Runtime.Serialization;
using Nozomi.Base.BCL;

namespace Nozomi.Data.Models.Web.Websocket
{
    [DataContract]
    public class WebsocketCommandProperty : Entity
    {
        public long Id { get; set; }
        
        public Guid Guid { get; set; }

        [DataMember]
        public CommandPropertyType CommandPropertyType { get; set; } = CommandPropertyType.Default;
        
        [DataMember]
        public string Key { get; set; }
        
        [DataMember]
        public string Value { get; set; }
        
        public long WebsocketCommandId { get; set; }
        
        public WebsocketCommand WebsocketCommand { get; set; }
    }
}