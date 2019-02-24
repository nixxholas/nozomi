using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.Models.Web.Websocket
{
    public class WebsocketRequest : Request
    {
        public long CurrencyPairId { get; set; }
        
        public CurrencyPair CurrencyPair { get; set; }
        
        public ICollection<WebsocketCommand> WebsocketCommands { get; set; }
    }
}