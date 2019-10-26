using Newtonsoft.Json;
using Nozomi.Base.Core.Events;
using Nozomi.Base.Core.Interfaces;
using Nozomi.Data.Repositories;

namespace Nozomi.Repo.Store
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IUser _user;

        public EventStore(IEventStoreRepository eventStoreRepository, IUser user)
        {
            _eventStoreRepository = eventStoreRepository;
            _user = user;
        }

        public void Save<T>(T theEvent) where T : Event
        {
            var serializedData = JsonConvert.SerializeObject(theEvent);

            var storedEvent = new StoredEvent(
                theEvent,
                serializedData,
                _user.UserName);

            _eventStoreRepository.Store(storedEvent);
        }
    }
}