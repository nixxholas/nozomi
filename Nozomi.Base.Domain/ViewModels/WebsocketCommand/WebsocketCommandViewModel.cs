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
            bool isEnabled, ICollection<WebsocketCommandPropertyViewModel> properties)
        {
            Id = id;
            Type = type;
            Name = name;
            Delay = delay;
            IsEnabled = isEnabled;
            Properties = properties;
        }

        public WebsocketCommandViewModel(string guid, CommandType type, string name, long delay,
            bool isEnabled, ICollection<WebsocketCommandPropertyViewModel> properties)
        {
            Guid = guid;
            Type = type;
            Name = name;
            Delay = delay;
            IsEnabled = isEnabled;
            Properties = properties;
        }
        
        public long Id { get; set; }
        
        public string Guid { get; set; }
        
        public new IEnumerable<WebsocketCommandPropertyViewModel> Properties { get; set; }
    }
}