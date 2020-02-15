using System.Collections.Generic;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;

namespace Nozomi.Data.ViewModels.WebsocketCommand
{
    public class CreateWebsocketCommandInputModel
    {
        public CommandType Type { get; set; }
        
        public string Name { get; set; }
        
        public long Delay { get; set; }
        
        public ICollection<CreateWebsocketCommandPropertyInputModel> Properties { get; set; }
    }
}