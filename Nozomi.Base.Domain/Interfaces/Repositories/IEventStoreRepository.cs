using System;
using System.Collections.Generic;
using Nozomi.Base.Core.Events;

namespace Nozomi.Data.Interfaces.Repositories
{
    public interface IEventStoreRepository : IDisposable
    {
        void Store(StoredEvent theEvent);
        IList<StoredEvent> All(Guid aggregateId);
    }
}