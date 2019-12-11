using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Web.Websocket
{
    [DataContract]
    public class WebsocketCommand : Entity
    {
        public long Id { get; set; }
        
        public Guid Guid { get; set; }

        public CommandType CommandType { get; set; } = CommandType.PlainText;
        
        /// <summary>
        /// The name of the command.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        
        /// <summary>
        /// 0 if it doesn't have to be polled, define a number when u have to call this periodically.
        /// </summary>
        [DataMember]
        public long Delay { get; set; }
        
        public long RequestId { get; set; }
        
        public Request Request { get; set; }
        
        public ICollection<WebsocketCommandProperty> WebsocketCommandProperties { get; set; }
    }
}