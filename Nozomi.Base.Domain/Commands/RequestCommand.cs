using System;
using Nozomi.Base.Core.Commands;
using Nozomi.Data.Models;

namespace Nozomi.Data.Commands
{
    public abstract class RequestCommand : Command
    {
        public Guid Guid { get; protected set; }

        public string DataPath { get; protected set; }

        public RequestType Type { get; protected set; }
        
        public ResponseType ResponseType { get; protected set; }
    }
}