using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;

namespace Nozomi.Data.ViewModels.WebsocketCommand
{
    public class WebsocketCommandViewModel
    {
        public WebsocketCommandViewModel() {}

        public WebsocketCommandViewModel(Guid guid, CommandType type, string name, long delay,
            bool isEnabled, ICollection<WebsocketCommandPropertyViewModel> properties, string requestGuid)
        {
            Guid = guid;
            Type = type;
            Name = name;
            Delay = delay;
            IsEnabled = isEnabled;
            Properties = properties;
            if (System.Guid.TryParse(requestGuid, out var parsedGuid))
                RequestGuid = parsedGuid;
            else
                throw new InvalidCastException("Invalid request guid.");
        }
        
        /// <summary>
        /// The type of the command.
        /// </summary>
        public CommandType Type { get; set; }
        
        /// <summary>
        /// Name of the command
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The delay of the command in milliseconds. Can be greater or equal to -1; Where -1 equals
        /// to a self-repeating command.
        /// </summary>
        public long Delay { get; set; }
        
        /// <summary>
        /// The unique identifier of the request this command is linked to.
        /// </summary>
        public Guid RequestGuid { get; set; }

        /// <summary>
        /// Is this enabled?
        /// </summary>
        public bool IsEnabled { get; set; }
        
        /// <summary>
        /// The obsolete identifier of the websocket command.
        /// </summary>
        [Obsolete]
        public long Id { get; set; }
        
        /// <summary>
        /// The unique GUID identifier of he websocket command.
        /// </summary>
        public Guid Guid { get; set; }
        
        /// <summary>
        /// The collection of properties linked to this websocket command.
        /// </summary>
        public ICollection<WebsocketCommandPropertyViewModel> Properties { get; set; }
    }
}