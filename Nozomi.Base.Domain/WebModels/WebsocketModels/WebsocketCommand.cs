using System.Collections;
using System.Collections.Generic;
using Nozomi.Base.Core;

namespace Nozomi.Data.WebModels.WebsocketModels
{
    public class WebsocketCommand : BaseEntityModel
    {
        public long Id { get; set; }
        
        public CommandType CommandType { get; set; }
        
        /// <summary>
        /// The name of the command.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 0 if it doesn't have to be polled, define a number when u have to call this periodically.
        /// </summary>
        public long Delay { get; set; }
        
        public long WebsocketRequestId { get; set; }
        
        public WebsocketRequest WebsocketRequest { get; set; }
        
        public ICollection<WebsocketCommandProperty> WebsocketCommandProperties { get; set; }
    }
}