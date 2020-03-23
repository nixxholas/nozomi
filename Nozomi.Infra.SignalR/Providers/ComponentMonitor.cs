using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Infra.SignalR.Providers.Interfaces;

namespace Nozomi.Infra.SignalR.Providers
{
    /// <summary>
    /// The Component Entity Data Provider for SignalR.
    /// </summary>
    public class ComponentMonitor : IObservable<ComponentViewModel>, IComponentMonitor
    {
        private ICollection<IObserver<ComponentViewModel>> _observers;

        public ComponentMonitor()
        {
            _observers = new List<IObserver<ComponentViewModel>>();
        }
        
        public IDisposable Subscribe(IObserver<ComponentViewModel> observer)
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
            private ICollection<IObserver<ComponentViewModel>> _observers;
            private IObserver<ComponentViewModel> _observer;

            public Unsubscriber(ICollection<IObserver<ComponentViewModel>> observers, 
                IObserver<ComponentViewModel> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose() 
            {
                if (_observer != null) _observers.Remove(_observer);
            }
        }

        public ICollection<ComponentViewModel> GetComponents()
        {
            throw new NotImplementedException();
        }
    }
}