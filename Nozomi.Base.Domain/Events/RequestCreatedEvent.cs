using System;
using Nozomi.Base.Core.Events;
using Nozomi.Data.Models;

namespace Nozomi.Data.Events
{
    public class RequestCreatedEvent : Event
    {
        public RequestCreatedEvent(Guid guid)
        {
            Guid = guid;
            AggregateId = guid;
        }
        
        public Guid Guid { get; set; }
    }
}