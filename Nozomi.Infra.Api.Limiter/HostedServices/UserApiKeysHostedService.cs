using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Infra.Api.Limiter.HostedServices
{
    public class UserApiKeysHostedService : BaseHostedService<UserApiKeysHostedService>
    {
        public UserApiKeysHostedService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }
    }
}