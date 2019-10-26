using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Base.Core.Bus;
using Nozomi.Base.Core.Events;
using Nozomi.Base.Core.Interfaces;
using Nozomi.Base.Core.Models;
using Nozomi.Base.Core.Notifications;
using Nozomi.Data.CommandHandlers;
using Nozomi.Data.Commands;
using Nozomi.Data.Commands.Sources;
using Nozomi.Data.EventHandlers;
using Nozomi.Data.Events;
using Nozomi.Data.Events.Sources;
using Nozomi.Data.Interfaces;
using Nozomi.Data.Repositories;
using Nozomi.Repo.BCL;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Repo.Store;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web.StartupExtensions
{
    public static class RepoStartup
    {
        public static void ConfigureRepoLayer(this IServiceCollection services)
        {
            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            services.AddScoped<INewRequestService, NewRequestService>();
            services.AddScoped<ISourceService, SourceService>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<RequestCreatedEvent>, RequestEventHandler>();
            services.AddScoped<INotificationHandler<SourceCreatedEvent>, SourceEventHandler>();

            // Domain - Commands
            services.AddScoped<IRequestHandler<CreateRequestCommand, bool>, RequestCommandHandler>();
            services.AddScoped<IRequestHandler<CreateSourceCommand, bool>, SourceCommandHandler>();

            // Database
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ISourceRepository, SourceRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();

            services.AddScoped<IUnitOfWork<NozomiDbContext>, UnitOfWork<NozomiDbContext>>();
            services.AddScoped<IDbContext, NozomiDbContext>();

            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreRepository>();
            services.AddScoped<IEventStore, EventStore>();
            services.AddScoped<EventStoreContext>();
            services.AddScoped<IDbContext, EventStoreContext>();

            // Infra - Identity
            services.AddScoped<IUser, WebUser>();
        }
    }
}
