using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nozomi.Base.Core.Bus;
using Nozomi.Base.Core.Notifications;
using Nozomi.Data.Commands.Sources;
using Nozomi.Data.Events.Sources;
using Nozomi.Data.Interfaces;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Repositories;

namespace Nozomi.Data.CommandHandlers
{
    public class SourceCommandHandler : CommandHandler,
        IRequestHandler<CreateSourceCommand, bool>
    {
        private readonly IMediatorHandler _bus;
        private readonly ISourceRepository _sourceRepository;
        
        public SourceCommandHandler(IUnitOfWork uow, IMediatorHandler bus, 
            INotificationHandler<DomainNotification> notifications,
            ISourceRepository sourceRepository) : base(uow, bus, notifications)
        {
            _bus = bus;
            _sourceRepository = sourceRepository;
        }

        public Task<bool> Handle(CreateSourceCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var source = new Source(message.Abbreviation, message.Name, message.APIDocsURL);
            
            _sourceRepository.Add(source);

            if (Commit())
            {
                _bus.RaiseEvent(new SourceCreatedEvent(source.Guid));
            }

            return Task.FromResult(true);
        }
    }
}