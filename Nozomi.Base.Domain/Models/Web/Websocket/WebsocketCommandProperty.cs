using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Web.Websocket
{
    public class WebsocketCommandProperty : Entity
    {
        public long Id { get; set; }

        public CommandPropertyType CommandPropertyType { get; set; } = CommandPropertyType.Default;
        
        public string Key { get; set; }
        
        public string Value { get; set; }
        
        public long WebsocketCommandId { get; set; }
        
        public WebsocketCommand WebsocketCommand { get; set; }
    }
}