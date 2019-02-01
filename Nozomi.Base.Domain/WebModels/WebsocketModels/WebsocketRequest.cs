using System.Collections.Generic;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Data.WebModels.WebsocketModels
{
    public class WebsocketRequest : Request
    {
        public long CurrencyPairId { get; set; }
        
        public CurrencyPair CurrencyPair { get; set; }
        
        public ICollection<WebsocketCommand> WebsocketCommands { get; set; }
    }
}