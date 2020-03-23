using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web;

namespace Nozomi.Infra.SignalR.Providers
{
    public class ComponentMonitor : IObservable<Component>
    {
        private ICollection<IObserver<Component>> _observers;

        public ComponentMonitor()
        {
            _observers = new List<IObserver<Component>>();
        }
        
        public IDisposable Subscribe(IObserver<Component> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new Unsubscriber(_observers, observer);
        }
        
        /// <summary>
        /// Unsubcriber entity to assist in removing observers from the monitor.
        /// </summary>
        private class Unsubscriber : IDisposable
        {
            private ICollection<IObserver<Component>> _observers;
            private IObserver<Component> _observer;

            public Unsubscriber(ICollection<IObserver<Component>> observers, IObserver<Component> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose() 
            {
                if (_observer != null) _observers.Remove(_observer);
            }
        }
    }
}