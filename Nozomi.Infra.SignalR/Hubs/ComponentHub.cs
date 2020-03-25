using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Infra.SignalR.Hubs.Services;
using Nozomi.Infra.SignalR.Hubs.Services.Interfaces;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Infra.SignalR.Hubs
{
    /// <summary>
    /// Component SignalR Hub (Ingress)
    /// </summary>
    public class ComponentHub : Hub<IComponentHubService>
    {
        private readonly IComponentEvent _componentEvent;
        private readonly ComponentHubService _componentHubService;

        public ComponentHub(ComponentHubService componentHubService, IComponentEvent componentEvent)
        {
            _componentEvent = componentEvent;
            _componentHubService = componentHubService;
        }

        public async IAsyncEnumerable<IEnumerable<ComponentViewModel>> All(
            int index,
            [EnumeratorCancellation]
            CancellationToken cancellationToken)
        {
            if (index < 0) index = 0;
            
            while (!cancellationToken.IsCancellationRequested)
            {
                // Check the cancellation token regularly so that the server will stop
                // producing items if the client disconnects.
                // cancellationToken.ThrowIfCancellationRequested();

                // yield allows you to return an element one at a time
                // Since we're returning components repeatedly, we try our best
                // to return a collection every fixed amount of milliseconds.
                yield return _componentEvent.ViewAll(index);

                // Use the cancellationToken in other APIs that accept cancellation
                // tokens so the cancellation can flow down to them.
                await Task.Delay(100, cancellationToken);
            }
        }
    }
}