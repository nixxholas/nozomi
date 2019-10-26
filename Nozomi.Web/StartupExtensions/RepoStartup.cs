using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Base.Core.Bus;
using Nozomi.Base.Core.Notifications;
using Nozomi.Data.CommandHandlers;
using Nozomi.Data.Commands;
using Nozomi.Data.EventHandlers;
using Nozomi.Data.Events;
using Nozomi.Data.Interfaces;
using Nozomi.Repo.BCL;
using Nozomi.Repo.Data;

namespace Nozomi.Web.StartupExtensions
{
    public static class RepoStartup
    {
        public static void ConfigureRepoLayer(this IServiceCollection services)
        {
            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<RequestCreatedEvent>, RequestEventHandler>();

            // Domain - Commands
            services.AddScoped<IRequestHandler<CreateRequestCommand, bool>, RequestCommandHandler>();

            // Database
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IUnitOfWork<NozomiDbContext>, UnitOfWork<NozomiDbContext>>();
            services.AddScoped<IDbContext, NozomiDbContext>();
        }
    }
}
