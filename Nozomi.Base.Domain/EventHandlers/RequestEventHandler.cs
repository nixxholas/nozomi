using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nozomi.Data.Events;

namespace Nozomi.Data.EventHandlers
{
    public class RequestEventHandler :
        INotificationHandler<RequestCreatedEvent>
    {
        public Task Handle(RequestCreatedEvent message, CancellationToken cancellationToken)
        {
            // Send some notification e-mail

            return Task.CompletedTask;
        }
    }
}