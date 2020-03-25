using System;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.Component;

namespace Nozomi.Infra.SignalR.Observers
{
    /// <summary>
    /// Component Reporter
    ///
    /// Allows the client/s to register for notifications while the provider
    /// monitors data and sends notifications to one or more observers.
    /// </summary>
    public class ComponentReporter : IObserver<ComponentViewModel>
    {
        private readonly ILogger<ComponentReporter> _logger;
        private IDisposable _unsubscriber;
        private bool first = true;
        private ComponentViewModel last;

        public ComponentReporter(ILogger<ComponentReporter> logger)
        {
            _logger = logger;
        }

        public virtual void Subscribe(IObservable<ComponentViewModel> provider)
        {
            _unsubscriber = provider.Subscribe(this);
        }
        
        public virtual void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }
        
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            _logger.LogError($"{typeof(ComponentReporter).FullName}: {error}");
        }

        public void OnNext(ComponentViewModel value)
        {
            throw new NotImplementedException();
        }
    }
}