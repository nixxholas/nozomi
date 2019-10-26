using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nozomi.Data.Events.Sources;

namespace Nozomi.Data.EventHandlers
{
    public class SourceEventHandler : INotificationHandler<SourceCreatedEvent>
    {
        public Task Handle(SourceCreatedEvent notification, CancellationToken cancellationToken)
        {
            
            return Task.CompletedTask;
        }
    }
}