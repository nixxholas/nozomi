using System.Collections.Generic;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;

namespace Nozomi.Data.ViewModels.WebsocketCommand
{
    public class WebsocketCommandViewModel
    {
        public WebsocketCommandViewModel() {}

        public WebsocketCommandViewModel(CommandType type, string name, long delay,
            ICollection<WebsocketCommandPropertyViewModel> properties)
        {
            Type = type;
            Name = name;
            Delay = delay;
            Properties = properties;
        }
        
        public CommandType Type { get; set; }
        
        public string Name { get; set; }
        
        public long Delay { get; set; }
        
        public ICollection<WebsocketCommandPropertyViewModel> Properties { get; set; } 
    }
}