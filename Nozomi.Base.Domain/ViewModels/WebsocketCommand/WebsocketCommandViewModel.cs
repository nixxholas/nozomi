using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;

namespace Nozomi.Data.ViewModels.WebsocketCommand
{
    public class WebsocketCommandViewModel : CreateWebsocketCommandInputModel
    {
        public WebsocketCommandViewModel() {}

        public WebsocketCommandViewModel(long id, CommandType type, string name, long delay,
            ICollection<CreateWebsocketCommandPropertyInputModel> properties)
        {
            Id = id;
            Type = type;
            Name = name;
            Delay = delay;
            Properties = properties;
        }

        public WebsocketCommandViewModel(Guid guid, CommandType type, string name, long delay,
            ICollection<CreateWebsocketCommandPropertyInputModel> properties)
        {
            Guid = guid;
            Type = type;
            Name = name;
            Delay = delay;
            Properties = properties;
        }
        
        public long Id { get; set; }
        
        public Guid Guid { get; set; }
    }
}