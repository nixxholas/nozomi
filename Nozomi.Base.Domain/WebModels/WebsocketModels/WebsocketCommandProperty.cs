using Nozomi.Base.Core;

namespace Nozomi.Data.WebModels.WebsocketModels
{
    public class WebsocketCommandProperty : BaseEntityModel
    {
        public long Id { get; set; }
        
        public CommandPropertyType CommandPropertyType { get; set; }
        
        public string Value { get; set; }
        
        public long WebsocketCommandId { get; set; }
        
        public WebsocketCommand WebsocketCommand { get; set; }
    }
}