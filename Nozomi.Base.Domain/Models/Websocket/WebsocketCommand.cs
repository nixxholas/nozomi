using System.Collections.Generic;
using Nozomi.Base.Core.Models;

namespace Nozomi.Data.Models.Websocket
{
    public class WebsocketCommand : Entity
    {
        public long Id { get; set; }

        public CommandType CommandType { get; set; } = CommandType.PlainText;
        
        /// <summary>
        /// The name of the command.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 0 if it doesn't have to be polled, define a number when u have to call this periodically.
        /// </summary>
        public long Delay { get; set; }
        
        public long RequestId { get; set; }
        
        public Request Request { get; set; }
        
        public ICollection<WebsocketCommandProperty> WebsocketCommandProperties { get; set; }
    }
}