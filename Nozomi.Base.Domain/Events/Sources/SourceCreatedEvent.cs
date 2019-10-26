using System;
using Nozomi.Base.Core.Events;

namespace Nozomi.Data.Events.Sources
{
    public class SourceCreatedEvent : Event
    {
        public SourceCreatedEvent(Guid guid)
        {
            Guid = guid;
            AggregateId = guid;
        }
        
        public Guid Guid { get; set; }
    }
}