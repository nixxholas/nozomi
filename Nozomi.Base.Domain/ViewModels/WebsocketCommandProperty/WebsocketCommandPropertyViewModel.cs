using Nozomi.Data.Models.Web.Websocket;

namespace Nozomi.Data.ViewModels.WebsocketCommandProperty
{
    public class WebsocketCommandPropertyViewModel
    {
        public long Id { get; set; }
        
        public CommandPropertyType Type { get; set; }
        
        public string Key { get; set; }
        
        public string Value { get; set; }
    }
}