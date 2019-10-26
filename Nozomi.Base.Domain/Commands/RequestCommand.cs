using System;
using Nozomi.Base.Core.Commands;
using Nozomi.Data.Models;

namespace Nozomi.Data.Commands
{
    public abstract class RequestCommand : Command
    {
        public RequestType RequestType { get; set; }

        public ResponseType ResponseType { get; set; }

        public string DataPath { get; set; }

        public int Delay { get; set; }

        public int FailureDelay { get; set; }
        
        public long? CurrencyId { get; set; }
        
        public string CurrencySlug { get; set; }
        
        public long? CurrencyPairId { get; set; }
        
        public long? CurrencyTypeId { get; set; }
    }
}