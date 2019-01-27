using System.Collections.Generic;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Data.WebModels.WebsocketModels
{
    public class WebsocketRequest : CurrencyPairRequest
    {
        
        
        public ICollection<WebsocketCommand> WebsocketCommands { get; set; }
    }
}