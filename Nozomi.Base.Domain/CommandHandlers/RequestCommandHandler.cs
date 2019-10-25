using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nozomi.Base.Core.Bus;
using Nozomi.Base.Core.Notifications;
using Nozomi.Data.Commands;
using Nozomi.Data.Events;
using Nozomi.Data.Interfaces;
using Nozomi.Data.Interfaces.Repositories;
using Nozomi.Data.Models;

namespace Nozomi.Data.CommandHandlers
{
    public class RequestCommandHandler : CommandHandler,
        IRequestHandler<CreateRequestCommand, bool>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IMediatorHandler _bus;

        public RequestCommandHandler(IRequestRepository requestRepository, 
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) :base(uow, bus, notifications)
        {
            _requestRepository = requestRepository;
            _bus = bus;
        }

        public Task<bool> Handle(CreateRequestCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var request = new Request(message.RequestType, message.ResponseType, message.DataPath, message.Delay,
                message.FailureDelay, message.CurrencyId, message.CurrencyPairId, message.CurrencyTypeId);

            // Sample
//            if (_customerRepository.GetByEmail(customer.Email) != null)
//            {
//                Bus.RaiseEvent(new DomainNotification(message.MessageType, "The customer e-mail has already been taken."));
//                return Task.FromResult(false);
//            }
            
            _requestRepository.Add(request);

            if (Commit())
            {
                _bus.RaiseEvent(new RequestCreatedEvent(request.Guid));
            }

            return Task.FromResult(true);
        }

        public void Dispose()
        {
            _requestRepository.Dispose();
        }
    }
}